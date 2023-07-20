using System;
using System.Collections.Generic;
using UnityEngine;

namespace Nashet.ECS
{
	public struct MapComponent
	{
		public int xSize => map.Count;
		public int ySize => map[0].Count;

		private List<List<GameCell>> map;
		private int explodeLineTreshold;
		private List<string> allowedElements;

		public MapComponent(List<List<GameCell>> list1, int explodeLineTreshold, List<string> list2) : this()
		{
			this.map = list1;
			this.explodeLineTreshold = explodeLineTreshold;
			this.allowedElements = list2;
		}

		internal string GetElement(int x, int y)
		{
			return map[x][y].CellType;
		}

		internal void LoadFrom(MapComponent loadMap)
		{			
			map = loadMap.map;
			explodeLineTreshold = loadMap.explodeLineTreshold;
			allowedElements = loadMap.allowedElements;
		}

		internal void CopyTo(PositionComponent position, Vector2Int clickedCell)
		{
			//throw new NotImplementedException();
		}
	}

}