using UnityEngine;

namespace Nashet.GameplayView
{
	public class CellView : MonoBehaviour
	{
		[SerializeField] private SpriteRenderer spriteRenderer;
		[SerializeField] private SpriteRenderer wayPoint;
		public UnitView unitView; //todo

		public Vector2Int coords { get; private set; }

		public void SetUp(Sprite sprite, int x, int y)
		{
			SetSprite(sprite);
			coords = new Vector2Int(x, y);
			//InputSystem.onEvent
			//.Where(e => e.HasButtonPress())
			//.CallOnce(ctrl => Debug.Log($"Button {ctrl} pressed"));
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
