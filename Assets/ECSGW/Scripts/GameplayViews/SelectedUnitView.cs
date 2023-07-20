using Nashet.Controllers;
using System;
using UnityEngine;

namespace Nashet.GameplayView
{
	public class SelectedUnitView : MonoBehaviour
	{
		[SerializeField] SpriteRenderer selection;
		private MapController mapController;

		internal void Subscribe(MapController mapController) // todo maybe remove interface? Or fix it
		{
			this.mapController = mapController;
			mapController.UnitClicked += UnitClickedHandler;
			mapController.EmptyCellClicked += UnitClickedHandler;
			mapController.UnitMoved += UnitMovedhandler;
		}

		private void OnDestroy()
		{
			mapController.UnitClicked -= UnitClickedHandler;
			mapController.EmptyCellClicked -= UnitClickedHandler;
			mapController.UnitMoved -= UnitMovedhandler;
		}

		private void UnitMovedhandler(Vector2Int from, Vector2Int toPosition)
		{
			ClearSelection();
		}

		private void ClearSelection()
		{
			selection.enabled = false;
		}

		private void UnitClickedHandler(Vector2Int position, Vector3 worldPosition)
		{
			selection.transform.position = worldPosition;
			selection.enabled = true;
		}
	}
}
