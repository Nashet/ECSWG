using Nashet.Controllers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Nashet.GameplayView
{
	public class MapViewGenerator : MonoBehaviour
	{
		[SerializeField] GameObject cellViewPrefab;

		[SerializeField] MapView mapView;
		private IMapController mapController;

		private IEnumerator Start()
		{
			yield return new WaitWhile(() => mapController == null || !mapController.IsReady);
			GenerateView();
		}

		private void GenerateView()
		{
			CreateMap();
		}

		public void Subscribe(IMapController mapController)
		{
			this.mapController = mapController;
		}

		private void OnDestroy()
		{
			if (mapController != null)
			{

			}
		}

		private void CreateMap()
		{
			var cellViewList = new List<List<CellView>>();
			var xOffset = mapController.GetSize().x / 2f - 0.5f;
			var yOffset = mapController.GetSize().y / 2f - 0.5f;

			for (int y = 0; y < mapController.GetSize().y; y++)
			{
				var newRow = new List<CellView>();
				cellViewList.Add(newRow);
				for (int x = 0; x < mapController.GetSize().x; x++)
				{
					//todo Should be in controller - view vs view
					var cell = Instantiate(cellViewPrefab, new Vector3((x - xOffset), (y - yOffset), 0), Quaternion.identity);
					cell.transform.parent = mapView.transform;
					cell.name = $"CellView({x},{y})";

					var cellView = cell.GetComponent<CellView>();

					var sprite = mapController.GetSprite(x, y);

					cellView.SetUp(sprite, x, y);// todo view should not change other view. Or is it a controller actually??
					newRow.Add(cellView);
				}
			}
			mapView.SetUp(cellViewList);
			mapController.CreateUnits(); // may be  put it in first tic? todo
		}
	}
}
