
//using NUnit.Framework;
//using Nashet.Services;
//using Nashet.Models;
//using UnityEngine;
//using Nashet.Controllers; //todo maybe remove controllses and test only model?
//using System.Collections.Generic;

//namespace Tests
//{
//	public class MapControllerTests
//	{
//		[Test]
//		public void TestImpossibleSlides1()
//		{
//			MockConfigService configService = new MockConfigService(6, 4);
//			var genService = new MapGeneratorService(configService);

//			var movesController = new MapController(configService, genService.Generate());
//			var res = movesController.HandleSlideHappened(new Vector2Int(0, 0), new Vector2Int(5, 3));
//			Assert.That(res, Is.False);
//		}

//		[Test]
//		public void TestImpossibleSlides2()
//		{
//			MockConfigService configService = new MockConfigService(6, 4);
//			var generationService = new MapGeneratorService(configService);

//			var simService = new MapController(configService, generationService.Generate());
//			var res = simService.HandleSlideHappened(new Vector2Int(0, 0), new Vector2Int(-1, 0));
//			Assert.That(res, Is.False);
//		}

//		[Test]
//		public void TestImpossibleSlides3()
//		{
//			MockConfigService configService = new MockConfigService(6, 4);
//			var genService = new MapGeneratorService(configService);

//			var simService = new MapController(configService, genService.Generate());
//			var res = simService.HandleSlideHappened(new Vector2Int(5, 3), new Vector2Int(6, 3));
//			Assert.That(res, Is.False);
//		}


//		[Test]
//		public void TestSlideResult()
//		{
//			MockConfigService configService = new MockConfigService(4, 6);
//			var generationService = new MapGeneratorService(configService);

//			Map map = generationService.Generate();
//			var simService = new MapController(configService, map);

//			var from = new Vector2Int(2, 3);
//			var to = new Vector2Int(2, 2);

//			var originalType = map.GetElement(from).CellType;
//			var originalType2 = map.GetElement(to).CellType;
//			//Debug.LogWarning($"originalType {originalType}, originalType2 {originalType2}");

//			simService.HandleSlideHappened(from, to);
//			Assert.That(map.GetElement(from).CellType, Is.EqualTo(originalType2));
//			Assert.That(map.GetElement(to).CellType, Is.EqualTo(originalType));
//		}

//		[Test]
//		public void TestCheckRowsForCoincidence()
//		{
//			// Arrange
//			var cellType = new CellType("red");
//			var defaultCellType = new CellType("default");
//			var map = new Map(new List<List<GameCell>>
//	{
//		new List<GameCell> { new GameCell(cellType), new GameCell(cellType), new GameCell(cellType) },
//		new List<GameCell> { new GameCell(defaultCellType), new GameCell(defaultCellType), new GameCell(defaultCellType) },
//		new List<GameCell> { new GameCell(defaultCellType), new GameCell(defaultCellType), new GameCell(defaultCellType) }
//	}, 3, new List<CellType> { cellType });
//			var explosionHandlerCalled = false;
//			map.ExplosionHappened += (where, amount, direction) => explosionHandlerCalled = true;

//			// Act
//			map.CheckRowsForCoincidence();

//			// Assert
//			Assert.IsTrue(explosionHandlerCalled);
//		}
//	}
//}