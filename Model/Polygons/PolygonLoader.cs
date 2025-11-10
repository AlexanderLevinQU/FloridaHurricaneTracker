using NetTopologySuite.Features;
using NetTopologySuite.Geometries;
using NetTopologySuite.IO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FloridaHurricaneTracker.Model.Polygons
{
    internal class PolygonLoader
    {
        public static Geometry LoadPolygon(string geoJsonPath)
        {
            string geoJson = File.ReadAllText(geoJsonPath);
            GeoJsonReader reader = new GeoJsonReader();

            FeatureCollection featureCollection = reader.Read<FeatureCollection>(geoJson);

            // Only one feature
            Geometry geometry = featureCollection[0].Geometry;

            if (geometry is Polygon || geometry is MultiPolygon)
                return geometry;

            throw new Exception("Unexpected geometry type in Florida GeoJSON");
        }

    }
}
