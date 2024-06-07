using GeoLibrary;
using System;
namespace ConsoleApp52
{
	internal class Program
	{
		static void Main(string[] args)
		{
			GeoJSONData geoPoint1 = new GeoJSONData();
			GeoJSONData geoPoint2 = new GeoJSONData();
			GeoJSONData geoPolygon = new GeoJSONData();
			GeoJSONHelper jsonHelp = new GeoJSONHelper();
			geoPoint1 = jsonHelp.LoadGeoJSON("GeoPoint_1.json");
			geoPoint2 = jsonHelp.LoadGeoJSON("GeoPoint_2.json");
			geoPolygon = jsonHelp.LoadGeoJSON("polygon.json");
			Console.WriteLine($"Весь файл:\n{geoPoint1.Type}\n{geoPoint1.Geometry}\n{geoPoint1.Properties}");
			jsonHelp.SaveGeoJSON("geoJSON.json", geoPoint1);//сохранение
			Console.WriteLine($"Тип:\n{jsonHelp.GetGeometryType(geoPoint1)}");
			Console.WriteLine($"Координаты:\n{jsonHelp.GetCoordinates(geoPoint1)}");
			Console.WriteLine($"Площадь полигона:\n{jsonHelp.CalculateArea(geoPolygon)}");
			Console.WriteLine($"Расстояние между точками:\n{jsonHelp.CalculateDistance(geoPoint1, geoPoint2)}");
		}
	}
}