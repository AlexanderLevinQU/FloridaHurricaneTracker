using FloridaHurricaneTracker.Model.HurricaneInfo;
using FloridaHurricaneTracker.Model.Polygons;
using Microsoft.Extensions.Configuration;
using NetTopologySuite.Features;
using NetTopologySuite.Geometries;
using NetTopologySuite.IO;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Shapes;
using Polygon = NetTopologySuite.Geometries.Polygon;


namespace FloridaHurricaneTracker.Model
{
    internal class HurricaneParser
    {

        private readonly string _filePath;
        private Geometry _floridaGeometry;

        public HurricaneParser()
        {

            IConfigurationRoot config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile(ConfigParameters.APP_SETTINGS)
                .Build();

            _filePath = config[ConfigParameters.HURDAT2_FILEPATH];
            _floridaGeometry = PolygonLoader.LoadPolygon(config[ConfigParameters.FLORIDA_GEOJSON]);

            if (string.IsNullOrEmpty(_filePath))
            {
                throw new Exception("CsvParserHelper(): Was not able to read Hurricane Data file.");
            }
        }


        public List<Hurricane> ParseHurricanes()
        {
            List<Hurricane> hurricanes = new List<Hurricane>();
            string[] file = File.ReadAllLines(_filePath);
            int index = 0;
            while (index < file.Length)
            {
                if (file[index].StartsWith("AL")) //Means we hit an entry and then we can create a candidate and parase
                {
                    // parse header
                    string[] headerSplit = file[index].Split(',', StringSplitOptions.TrimEntries); //Trim to get rid of extra spaces
                    int? year = int.Parse(headerSplit[0].Substring(4));
                    if (year != null && year < 1900)
                    {
                        int entries = int.Parse(headerSplit[2]) + 1; //can just skip faster. The third number tells how many entries there are
                        index += entries;
                        continue;
                    }

                    HurricaneCandidate hurricaneCandidate = new HurricaneCandidate()
                    {
                        Id = headerSplit[0], // example AL071889
                        Name = headerSplit[1], // example UNNAMED
                        MaxWindSpeed = int.MinValue, // So next wind speed breaks it
                        HitsFloridaAsHurricane = false
                    };

                    //Loop through entries until next AL
                    index = ParseHurricaneEntries(file, index + 1, hurricaneCandidate);

                    if (hurricaneCandidate.HitsFloridaAsHurricane)
                    {
                        //Create hurricane
                        Hurricane hurricane = new Hurricane(hurricaneCandidate.Name, hurricaneCandidate.TimeStamp, hurricaneCandidate.MaxWindSpeed);
                        hurricanes.Add(hurricane);
                    }
                }
                else
                {
                    index += 1; // if it doesn't start with AL don't skip
                }
            }

            return hurricanes;
        }

        private int ParseHurricaneEntries(string[] file, int index, HurricaneCandidate hurricaneCandidate)
        {
            while (index < file.Length && !file[index].StartsWith("AL"))
            {

                string[] entry = file[index].Split(',', StringSplitOptions.TrimEntries);
                string date = entry[0];
                string time = entry[1];
                string landfall = entry[2]; // care for L, determines landfall. Most of the time null
                string status = entry[3]; // if hurricane
                double latitude = ParseCoordinate(entry[4]);
                double longitude = ParseCoordinate(entry[5]);
                double windSpeed = double.Parse(entry[6]); //parse wind
                DateTime timestamp = DateTime.ParseExact($"{date}{time}", "yyyyMMddHHmm", null);

                HurricaneEntry hurricaneEntry = new HurricaneEntry()
                {
                    Timestamp = timestamp,
                    IsLandFall = landfall == "L",
                    IsHurricane = status == "HU",
                    Latitude = latitude,
                    Longitude = longitude,
                    WindSpeed = windSpeed
                };

                if (hurricaneEntry.WindSpeed > hurricaneCandidate.MaxWindSpeed)
                {
                    hurricaneCandidate.MaxWindSpeed = hurricaneEntry.WindSpeed;
                }

                if (hurricaneEntry.IsLandFall && hurricaneEntry.IsHurricane && !hurricaneCandidate.HitsFloridaAsHurricane) // don't want to check it twice and change if it already hit landfall
                {

                    // Check if it is in florida. Sometimes there are multiple landfalls so we can ignore
                    hurricaneCandidate.HitsFloridaAsHurricane = IsPointInFlorida(hurricaneEntry.Latitude, hurricaneEntry.Longitude);
                    hurricaneCandidate.TimeStamp = timestamp;
                }

                hurricaneCandidate.Entries.Add(hurricaneEntry);
                index += 1;
            }

            return index;
        }

        private double ParseCoordinate(string coord)
        {
            if (string.IsNullOrEmpty(coord))
                return double.NaN;

            coord = coord.Trim().ToUpperInvariant();
            double coordinate = double.Parse(coord.Substring(0, coord.Length - 1), CultureInfo.InvariantCulture); // Get last value besides direction. Could use regex here instead
            char direction = coord[coord.Length-1]; // direction
            if (direction == 'S' || direction == 'W') // if south or west make negative
                coordinate = -coordinate;
            return coordinate;
        }

        public bool IsPointInFlorida(double latitude, double longitude) // could move somewhere else
        {
            var geometryFactory = new GeometryFactory();
            var point = geometryFactory.CreatePoint(new Coordinate(longitude, latitude));

            MultiPolygon multiPolygon = _floridaGeometry as MultiPolygon;

            foreach (Polygon poly in multiPolygon.Geometries.OfType<Polygon>())
            {
                if (poly.Contains(point))
                    return true; 
            }

            return false;
        }
    }
}
