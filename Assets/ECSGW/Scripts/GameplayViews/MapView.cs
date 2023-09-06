using Nashet.Controllers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Nashet.GameplayView
{
	public class MapView : MonoBehaviour
	{
		[SerializeField] GameObject cellViewPrefab;
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
		private IEnumerator Start()
		{
			yield return new WaitWhile(() => mapController == null || !mapController.IsReady);
			GenerateView();
		}

		private void GenerateView()
		{
			CreateMap();
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

		private void ExplosionHappened(Vector2Int where, int amount)
		{
			AnimateExposion(where, amount);
		}

		private void CreateMap()
		{
			var newCellViewList = new List<List<CellView>>();
			var xOffset = mapController.GetSize().x / 2f - 0.5f;
			var yOffset = mapController.GetSize().y / 2f - 0.5f;

			for (int y = 0; y < mapController.GetSize().y; y++)
			{
				var newRow = new List<CellView>();
				newCellViewList.Add(newRow);
				for (int x = 0; x < mapController.GetSize().x; x++)
				{
					//todo Should be in controller - view vs view
					var cell = Instantiate(cellViewPrefab, new Vector3((x - xOffset), (y - yOffset), 0), Quaternion.identity);
					cell.transform.parent = transform;
					cell.name = $"CellView({x},{y})";

					var cellView = cell.GetComponent<CellView>();

					var sprite = mapController.GetSprite(x, y);

					cellView.SetUp(mapController, sprite, x, y);// todo view should not change other view. Or is it a controller actually?? I think its controllerc
					newRow.Add(cellView);
				}
			}
			this.cellViewList = newCellViewList;
		}
	}
}
