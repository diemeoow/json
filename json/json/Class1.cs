using System;
using System.IO;
using Newtonsoft.Json;

namespace GeoLibrary
{
	public class GeoJSONData
	{
		public string Type { get; set; }
		public dynamic Geometry { get; set; }
		public dynamic Properties { get; set; }
	}

	public class GeoJSONHelper
	{
		public GeoJSONData LoadGeoJSON(string filePath)
		{
			string json = File.ReadAllText(filePath);
			return JsonConvert.DeserializeObject<GeoJSONData>(json);
		}

		public void SaveGeoJSON(string filePath, GeoJSONData data)
		{
			string json = JsonConvert.SerializeObject(data);
			File.WriteAllText(filePath, json);
		}

		public string GetGeometryType(GeoJSONData data)
		{
			return data.Geometry.type;
		}

		public dynamic GetCoordinates(GeoJSONData data)
		{
			return data.Geometry.coordinates;
		}

		public double CalculateArea(GeoJSONData data)
		{
			if (data.Geometry.type == "Polygon")
			{
				return CalculatePolygonArea(data.Geometry.coordinates[0]);
			}
			else if (data.Geometry.type == "MultiPolygon")
			{
				double totalArea = 0;
				foreach (var polygon in data.Geometry.coordinates)
				{
					totalArea += CalculatePolygonArea(polygon[0]);
				}
				return totalArea;
			}
			else
			{
				Console.WriteLine("Не полигон");
				return 0;
			}
		}

		private double CalculatePolygonArea(dynamic coordinates)
		{
			double area = 0;
			int j = coordinates.Count - 1;
			for (int i = 0; i < coordinates.Count; i++)
			{
				double xi = coordinates[i][0];
				double yi = coordinates[i][1];
				double xj = coordinates[j][0];
				double yj = coordinates[j][1];
				area += (xj + xi) * (yj - yi);
				j = i;
			}
			return Math.Abs(area / 2.0);
		}

		public double CalculateDistance(GeoJSONData data1, GeoJSONData data2)
		{
			if (data1.Geometry.type == "Point" && data2.Geometry.type == "Point")
			{
				double lat1 = data1.Geometry.coordinates[1];
				double lon1 = data1.Geometry.coordinates[0];
				double lat2 = data2.Geometry.coordinates[1];
				double lon2 = data2.Geometry.coordinates[0];
				return HaversineDistance(lat1, lon1, lat2, lon2);
			}
			else
			{
				Console.WriteLine("Не являются точками");
				return 0;
			}
		}

		private double HaversineDistance(double lat1, double lon1, double lat2, double lon2)
		{
			const double R = 6371e3;
			double phi1 = lat1 * Math.PI / 180;
			double phi2 = lat2 * Math.PI / 180;
			double deltaPhi = (lat2 - lat1) * Math.PI / 180;
			double deltaLambda = (lon2 - lon1) * Math.PI / 180;

			double a = Math.Sin(deltaPhi / 2) * Math.Sin(deltaPhi / 2) +
					   Math.Cos(phi1) * Math.Cos(phi2) *
					   Math.Sin(deltaLambda / 2) * Math.Sin(deltaLambda / 2);
			double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));

			return R * c;
		}
	}
}