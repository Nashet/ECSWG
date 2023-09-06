using Nashet.Controllers;
using System;
using UnityEngine;

namespace Nashet.GameplayView
{
	public class CellView : MonoBehaviour
	{
		[SerializeField] private SpriteRenderer spriteRenderer;
		[SerializeField] private SpriteRenderer wayPoint;

		public Vector2Int coords { get; private set; }

		public void SetUp(IMapController mapController, Sprite sprite, int x, int y)
		{
			SetSprite(sprite);
			coords = new Vector2Int(x, y);
			mapController.UnitAppeared += UnitAppearedHandler;
			//InputSystem.onEvent
			//.Where(e => e.HasButtonPress())
			//.CallOnce(ctrl => Debug.Log($"Button {ctrl} pressed"));
		}

		private void UnitAppearedHandler(Vector2Int position, string unitType, int unitId)
		{

		}

		internal void SetSprite(Sprite sprite)
		{
			spriteRenderer.sprite = sprite;
		}

		public void SetWaypoint(bool state)
		{
			wayPoint.enabled = state;
		}
	}
}
