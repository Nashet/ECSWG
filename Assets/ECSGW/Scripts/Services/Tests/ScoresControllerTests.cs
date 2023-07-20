
//using NUnit.Framework;
//using Nashet.Models;
//using Nashet.Controllers;
//using NSubstitute;
//using Nashet.Services;
//using System.Collections.Generic;

//namespace Tests
//{
//	public class ScoresControllerTests
//	{
//		[Test]
//		public void AddScores_ScoresChangedEventRaised()
//		{
//			// Arrange
//			var model = new ScoresModel();
//			var controller = new ScoresController(model);
//			int? oldValue = null;
//			int? newValue = null;
//			controller.ScoreChanged += (o, n) =>
//			{
//				oldValue = o;
//				newValue = n;
//			};

//			// Act
//			controller.AddScores(10);

//			// Assert
//			Assert.IsNotNull(oldValue);
//			Assert.IsNotNull(newValue);
//			Assert.AreEqual(0, oldValue.Value);
//			Assert.AreEqual(10, newValue.Value);
//		}

//		[Test]
//		public void ExplosionHappenedHandler_AddScoresCalled()
//		{
//			// Arrange
//			var model = new ScoresModel();
//			var controller = new ScoresController(model);
//			var config = Substitute.For<IConfigService>();
//			var cellType = new CellType("red");
//			var mapData = new List<List<GameCell>> { new List<GameCell> { new GameCell(cellType), new GameCell(cellType), new GameCell(cellType), } };
//			var amountOfExploded = 3;
//			var mapController = new MapController(config, new Map(mapData, amountOfExploded, new List<CellType> { cellType }));

//			controller.Subscribe(mapController);

//			// Act
//			mapController.SimulateOneStep();

//			// Assert
//			Assert.AreEqual(amountOfExploded * 10, model.Scores);
//		}
//	}
//}