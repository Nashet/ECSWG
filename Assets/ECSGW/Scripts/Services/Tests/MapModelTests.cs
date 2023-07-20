//using System;
//using System.Collections.Generic;
//using System.Linq;
//using Nashet.Models;
//using NSubstitute.Core;
//using NUnit.Framework;
//using UnityEngine;

//[TestFixture]
//public class MapModelTests
//{
//	private Map map;


//	private CellType CellTypeRed = new CellType("red");
//	private CellType CellTypeGreen = new CellType("green");
//	private CellType CellTypeBlue = new CellType("blue");
//	private List<CellType> allowedCellTypes;

//	[SetUp]
//	public void Setup()
//	{
//		allowedCellTypes = new List<CellType> { CellTypeRed, CellTypeGreen, CellTypeBlue };

//		// dont change - test relies on it :( Blame gpt
//		var testData = new List<List<GameCell>>
//		{
//			new List<GameCell> { new GameCell(CellTypeRed), new GameCell(CellTypeRed), new GameCell(CellTypeGreen) },
//			new List<GameCell> { new GameCell(CellTypeGreen), new GameCell(CellTypeBlue), new GameCell(CellTypeBlue) },
//			new List<GameCell> { new GameCell(CellTypeRed), new GameCell(CellTypeBlue), new GameCell(CellTypeGreen) },
//		};
//		// Initialize an instance of the Map class with the test data set

//		map = new Map(testData, 3, allowedCellTypes);
//	}

//	//[Test]
//	//public void CheckRowsForCoincidenceInternal_Horizontal_NoCoincidence_NoExplosion()
//	//{
//	//	// Arrange
//	//	var expectedExplosions = 0;

//	//	// Act
//	//	map.CheckRowsForCoincidenceInternal(Direction.horizontal);

//	//	// Assert
//	//	Assert.AreEqual(expectedExplosions, map.GetExplosionCount());
//	//}

//	//[Test]
//	//public void CheckRowsForCoincidenceInternal_Horizontal_HasCoincidence_DealsExplosion()
//	//{
//	//	// Arrange
//	//	var expectedExplosions = 1;

//	//	// Act
//	//	map.CheckRowsForCoincidenceInternal(Direction.horizontal);

//	//	// Assert
//	//	Assert.AreEqual(expectedExplosions, map.GetExplosionCount());
//	//}

//	//[Test]
//	//public void CheckRowsForCoincidenceInternal_Vertical_NoCoincidence_NoExplosion()
//	//{
//	//	// Arrange
//	//	var expectedExplosions = 0;

//	//	// Act
//	//	map.CheckRowsForCoincidenceInternal(Direction.vertical);

//	//	// Assert
//	//	Assert.AreEqual(expectedExplosions, map.GetExplosionCount());
//	//}

//	//[Test]
//	//public void CheckRowsForCoincidenceInternal_Vertical_HasCoincidence_DealsExplosion()
//	//{
//	//	// Arrange
//	//	var expectedExplosions = 2;

//	//	// Act
//	//	map.CheckRowsForCoincidenceInternal(Direction.vertical);

//	//	// Assert
//	//	Assert.AreEqual(expectedExplosions, map.GetExplosionCount());
//	//}

//	/// <summary>
//	/// ////////////////////////////////////
//	/// </summary>

//	[Test]
//	public void IsPossibleSlide_ValidSlide_ReturnsTrue()
//	{
//		// Arrange
//		var from = new Vector2Int(0, 0);
//		var to = new Vector2Int(0, 1);

//		// Act
//		var result = map.IsPossibleSlide(from, to);

//		// Assert
//		Assert.IsTrue(result);
//	}

//	[Test]
//	public void IsPossibleSlide_InvalidSlide_ReturnsFalse()
//	{
//		// Arrange
//		var from = new Vector2Int(0, 0);
//		var to = new Vector2Int(0, 2);

//		// Act
//		var result = map.IsPossibleSlide(from, to);

//		// Assert
//		Assert.IsFalse(result);
//	}

//	[Test]
//	public void MakeSlide_InvalidSlide_DoesNotSlideElements()
//	{
//		// Arrange
//		var from = new Vector2Int(0, 0);
//		var to = new Vector2Int(0, 2);
//		var expectedFromType = CellTypeRed;
//		var expectedToType = CellTypeGreen;

//		var initialMap = new List<List<GameCell>>()
//		{
//			new List<GameCell>() { new GameCell(expectedFromType), new GameCell(CellTypeBlue), new GameCell(expectedToType) }
//		};
//		Map map = new Map(initialMap, int.MaxValue, null);

//		// Act
//		map.MakeSlide(from, to);

//		// Assert
//		Assert.AreEqual(expectedFromType, map.GetElement(from).CellType);
//		Assert.AreEqual(expectedToType, map.GetElement(to).CellType);
//	}

//	[Test]
//	public void MakeExplosion_HorizontalExplosion_DropsColumnAndSetsNewCells()
//	{
//		// Arrange
//		var where = new Vector2Int(0, 1);
//		var amount = 2;
//		var direction = Direction.horizontal;
//		var expectedFirstCellType = CellTypeGreen;
//		var expectedLastCellType = CellTypeBlue;

//		// Act
//		map.MakeExplosion(where, amount, direction, GenerateRandomCellTypes(amount));

//		// Assert
//		Assert.AreEqual(expectedFirstCellType, map.GetElement(new Vector2Int(0, 0)).CellType);
//		Assert.AreEqual(expectedLastCellType, map.GetElement(new Vector2Int(0, 2)).CellType);
//	}

//	[Test]
//	public void MakeExplosion_HorizontalExplosion_DropsColumnAndSetsNewCells2()
//	{
//		// Arrange
//		var where = new Vector2Int(0, 1);
//		var amount = 2;
//		var direction = Direction.horizontal;
//		var expectedFirstCellType = CellTypeGreen;
//		var expectedLastCellType = CellTypeBlue;

//		// Generate a map with the required dimensions and initial cell types
//		var initialCellTypes = new List<List<GameCell>>()
//	{
//		new List<GameCell>() { new GameCell(CellTypeRed), new GameCell(CellTypeBlue), new GameCell(CellTypeBlue) },
//		new List<GameCell>() { new GameCell(expectedFirstCellType), new GameCell(CellTypeBlue), new GameCell(expectedLastCellType) },
//		new List<GameCell>() { new GameCell(CellTypeBlue), new GameCell(CellTypeBlue), new GameCell(CellTypeGreen) }
//	};
//		Map map = new Map(initialCellTypes, 2, GenerateRandomCellTypes(2));

//		// Act
//		map.MakeExplosion(where, amount, direction, new List<CellType> { expectedFirstCellType, expectedLastCellType });

//		// Assert
//		Assert.AreEqual(expectedFirstCellType, map.GetElement(new Vector2Int(0, 0)).CellType);
//		Assert.AreEqual(expectedLastCellType, map.GetElement(new Vector2Int(0, 2)).CellType);
//	}

//	[Test]
//	public void MakeExplosion_VerticalExplosion_DropsRowAndSetsNewCells()
//	{
//		// Arrange
//		var where = new Vector2Int(1, 1);
//		var amount = 2;
//		var direction = Direction.vertical;
//		var expectedFirstCellType = CellTypeGreen;
//		var expectedLastCellType = CellTypeGreen;

//		// Act
//		map.MakeExplosion(where, amount, direction, GenerateRandomCellTypes(amount));

//		// Assert
//		Assert.AreEqual(expectedFirstCellType, map.GetElement(new Vector2Int(0, 1)).CellType);
//		Assert.AreEqual(expectedLastCellType, map.GetElement(new Vector2Int(2, 1)).CellType);
//	}

//	private T GetRandomElement<T>(IEnumerable<T> collection)
//	{
//		var count = collection.Count();
//		if (count == 0)
//		{
//			throw new ArgumentException("Collection cannot be empty.", nameof(collection));
//		}
//		var random = new System.Random();
//		var index = random.Next(count);
//		return collection.ElementAt(index);
//	}

//	private List<CellType> GenerateRandomCellTypes(int size)
//	{
//		var random = new System.Random();
//		return Enumerable.Range(1, size)
//						 .Select(x => GetRandomElement(allowedCellTypes))
//						 .ToList();
//	}
//}