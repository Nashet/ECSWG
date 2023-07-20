using UnityEngine;

namespace Nashet.ECS
{
	public struct PositionComponent
	{
		public Vector2Int pos;

		public bool[,] area;
		public void Set(Vector2Int pos)
		{
			this.pos = pos;
			area = new bool[11, 11];
		}
	}

	public struct NavigationComponent
	{
		public bool[][] area;
	}
}