
//using NUnit.Framework;
//using Nashet.Services;
//using Nashet.Models;
//using Nashet.Configs;
//using System.Collections.Generic;
//using UnityEngine;

//namespace Tests
//{
//	public class MockConfigService : ConfigService
//	{
//		private readonly int xSize;
//		private readonly int ySize;

//		public MockConfigService(int xSize, int ySize)
//		{
//			this.xSize = xSize;
//			this.ySize = ySize;
//		}

//		public override T GetConfig<T>() where T : class
//		{
//			var config = base.GetConfig<T>();
//			if (config is MapGenerationConfig)
//			{
//				var mockConfig = ScriptableObject.CreateInstance<MapGenerationConfig>();

//				mockConfig.xSize = xSize;
//				mockConfig.ySize = ySize;
//				mockConfig.AllowedCellTypes = new List<CellTypeConfig>
//				{
//					MakeCellType("1"),
//					MakeCellType("2"),
//					MakeCellType("3")
//				};
//				return mockConfig as T;
//			}
//			else
//				return config;
//		}

//		private static CellTypeConfig MakeCellType(string Id)
//		{
//			var cellType = ScriptableObject.CreateInstance<CellTypeConfig>();
//			cellType.Id = Id;
//			return cellType;
//		}
//	}
//	public class MapGeneratorServiceTests
//	{
//		[Test]
//		public void TestSize()
//		{
//			var service = new MapGeneratorService(new MockConfigService(16, 4));
//			Map map = service.Generate();
//			Assert.That(map.xSize, Is.EqualTo(16));
//			Assert.That(map.ySize, Is.EqualTo(4));
//		}

//		[Test]
//		public void TestSize2()
//		{
//			var service = new MapGeneratorService(new MockConfigService(6, 4));
//			Map map = service.Generate();

//			Assert.That(map.GetElement(0, 0), Is.Not.Null);
//			Assert.That(map.GetElement(0, 3), Is.Not.Null);
//			Assert.That(map.GetElement(5, 0), Is.Not.Null);
//			Assert.That(map.GetElement(5, 3), Is.Not.Null);
//		}

//		[Test]
//		public void TestAmountOfDifferentElements()
//		{
//			var configservice = new MockConfigService(6, 4);
//			var generationService = new MapGeneratorService(configservice);
//			Map map = generationService.Generate();

//			var foundTypes = new List<CellType>();
//			for (int y = 0; y < map.ySize; y++)
//			{
//				for (int x = 0; x < map.xSize - 1; x++)
//				{
//					var element = map.GetElement(x, y);
//					if (!foundTypes.Contains(element.CellType))
//					{
//						foundTypes.Add(element.CellType);
//					}
//				}
//			}

//			Assert.That(foundTypes.Count, Is.EqualTo(3));//mockConfig.AllowedCellTypes			
//		}
//	}
//}