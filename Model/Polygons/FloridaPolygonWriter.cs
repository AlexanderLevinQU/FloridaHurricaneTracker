using Microsoft.Extensions.Configuration;
using NetTopologySuite.Features;
using NetTopologySuite.Geometries;
using NetTopologySuite.IO;
using System.IO;

namespace FloridaHurricaneTracker.Model.Polygons
{
    internal class FloridaPolygonWriter
    {

        public FloridaPolygonWriter() { }

        public void CreateFloridaPolygon()
        {
            //Data for polygon downloaded from https://www2.census.gov/geo/tiger/TIGER2025/STATE/

            IConfigurationRoot config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile(ConfigParameters.APP_SETTINGS)
                .Build();

            // Get paths
            string shapefilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, config[ConfigParameters.SHAPE_FILE_PATH]);
            string outputGeoJson = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, config[ConfigParameters.FLORIDA_GEOJSON]);

            // Check if GeoJSON already exists
            if (File.Exists(outputGeoJson))
            {
                Console.WriteLine("Florida GeoJSON already exists. Skipping creation.");
                return;
            }

            var geometryFactory = new GeometryFactory();
            //"Model/Polygons/tl_2025_us_state/tl_2025_us_state.shp"
            using ShapefileDataReader reader = new ShapefileDataReader(shapefilePath, geometryFactory);
            var features = new FeatureCollection();

            while (reader.Read())
            {
                var geometry = reader.Geometry;

                var name = reader.GetString(reader.GetOrdinal("NAME"));
                if (name != "Florida")
                    continue; // Skip other states. If expand could have a 50 state list of files on create. Loop through each state. 

                var attributes = new AttributesTable();
                for (int i = 0; i < reader.DbaseHeader.NumFields; i++)
                {
                    string fieldName = reader.DbaseHeader.Fields[i].Name;
                    attributes.Add(fieldName, reader.GetValue(i));
                }

                var feature = new Feature(geometry, attributes);
                features.Add(feature);
            }

            // Write GeoJSON
            var geoJsonWriter = new GeoJsonWriter();
            string geoJsonText = geoJsonWriter.Write(features);
            // Ensure directory exists
            Directory.CreateDirectory(Path.GetDirectoryName(outputGeoJson)!);
            File.WriteAllText(outputGeoJson, geoJsonText);
        }
    }
}
