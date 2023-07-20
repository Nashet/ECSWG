using Nashet.Controllers;
using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Nashet.GameplayView
{
	public delegate void OnCellClicked(Vector2Int position, Vector3 worldPosition);

	public class MapViewClicker : MonoBehaviour
	{
		public Action<int> MouseClicked;//todo  move to another file		
		public event OnCellClicked CellClicked;

		private CellView previouslySelectedCell;
		private bool previousClicked;

		private void Start()
		{
			MouseClicked += MouseClickedHandler;
		}

		private void OnDestroy()
		{
			MouseClicked -= MouseClickedHandler;
		}

		private void Update()
		{
			if (Input.GetMouseButtonUp(0) && !previousClicked)
				MouseClicked?.Invoke(0);
			//Debug.LogError(Input.mousePosition);
			previousClicked = Input.GetMouseButtonUp(0);
		}

		public void Subscribe(MapController mapController)
		{
		}

		private void MouseClickedHandler(int button)
		{
			if (!EventSystem.current.IsPointerOverGameObject())//!hovering over UI) 
			{
				var collider = ClicksUtils.getRayCastMeshNumber(Camera.main);
				if (collider != null)
				{
					var thatCell = collider.GetComponent<CellView>();
					CellClicked?.Invoke(thatCell.coords, thatCell.transform.position);
					previouslySelectedCell = thatCell;
				}
			}
		}
	}
}
