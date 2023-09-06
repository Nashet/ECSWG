using Nashet.Controllers;
using System.Collections.Generic;
using UnityEngine;

namespace Nashet.GameplayView
{
	public class MapView : MonoBehaviour
	{


		private List<List<CellView>> cellViewList;
		private IMapController mapController;

		private void AnimateExposion(Vector2Int where, int amount)
		{

		}

		public void Subscribe(IMapController mapController)
		{
			this.mapController = mapController;
			mapController.ExplosionHappened += ExplosionHappened;
			mapController.WaypointsRefresh += WaypointsRefreshHandler;
			mapController.UnitMoved += UnitMovedHandler;
		}

		private void OnDestroy()
		{
			if (mapController != null)
			{
				mapController.ExplosionHappened -= ExplosionHappened;
				mapController.WaypointsRefresh -= WaypointsRefreshHandler;
				mapController.UnitMoved -= UnitMovedHandler;
			}
		}

		private void UnitMovedHandler(Vector2Int from, Vector2Int toPosition, int unitId)
		{
			//Debug.LogError($"moving from {from} to {toPosition}");
			cellViewList[toPosition.y][toPosition.x] = cellViewList[from.y][from.x];
			ClearWaypoints();
		}

		private void WaypointsRefreshHandler(HashSet<Vector2Int> wayPoints)
		{
			ClearWaypoints();

			if (wayPoints != null)
			{
				foreach (var item in wayPoints)
				{
					if (item.x >= 0 && item.y >= 0 && item.y < cellViewList.Count && item.x < cellViewList[0].Count)
						cellViewList[item.y][item.x].SetWaypoint(true);
				}
			}
		}

		private void ClearWaypoints()
		{
			foreach (var column in cellViewList)
			{
				foreach (var cell in column)
				{
					cell.SetWaypoint(false); //todo view shouldnt change another view
				}
			}
		}

		//todo why setup and injection? cellViewList)
		internal void SetUp(List<List<CellView>> cellViewList)
		{
			this.cellViewList = cellViewList;
		}

		private void ExplosionHappened(Vector2Int where, int amount)
		{
			AnimateExposion(where, amount);
		}
	}
}
