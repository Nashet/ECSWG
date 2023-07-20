using Nashet.Controllers;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace Nashet.GameplayView
{
	public class MapView : MonoBehaviour
	{
		[SerializeField] GameObject unitPrefab;
		private List<List<CellView>> cellViewList;
		private IMapController mapController;
		private float xOffset;
		private float yOffset;

		//private const float cellScaleMultiplier = 1.01f;		

		private void AnimateCellSlide(Vector2Int from, Vector2Int to)
		{
			cellViewList[from.y][from.x].SetSprite(mapController.GetSprite(from.x, from.y));
			cellViewList[to.y][to.x].SetSprite(mapController.GetSprite(to.x, to.y));
		}

		private void AnimateCellFalling(Vector2Int value)
		{
			throw new NotImplementedException();
		}

		private void AnimateExposion(Vector2Int where, int amount)
		{

		}

		public void Subscribe(IMapController mapController)
		{
			this.mapController = mapController;
			mapController.ExplosionHappened += ExplosionHappened;
			mapController.WaypointsRefresh += WaypointsRefreshHandler;
			mapController.UnitMoved += UnitMovedHandler;
			mapController.UnitAppeared += UnitAppearedHandler;
			xOffset = mapController.GetSize().x / 2f - 0.5f;
			yOffset = mapController.GetSize().y / 2f - 0.5f;//todo fixit
		}

		private void OnDestroy()
		{
			if (mapController != null)
			{
				mapController.ExplosionHappened -= ExplosionHappened;
				mapController.WaypointsRefresh -= WaypointsRefreshHandler;
				mapController.UnitMoved -= UnitMovedHandler;
				mapController.UnitAppeared -= UnitAppearedHandler;
			}
		}

		private void UnitMovedHandler(Vector2Int from, Vector2Int toPosition)
		{
			cellViewList[from.y][from.x].unitView.transform.position = GetOffsetedPosition(toPosition);
			ClearWaypoints();
		}

		private void UnitAppearedHandler(Vector2Int position, string unitType)
		{
			var unit = Instantiate(unitPrefab, GetOffsetedPosition(position), Quaternion.identity);
			unit.transform.parent = transform;
			unit.name = $"Unit {unitType})";
			cellViewList[position.y][position.x].unitView = unit.GetComponent<UnitView>();
		}

		private Vector3 GetOffsetedPosition(Vector2Int position)
		{
			return new Vector3((position.x - xOffset), (position.y - yOffset), 0);
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
