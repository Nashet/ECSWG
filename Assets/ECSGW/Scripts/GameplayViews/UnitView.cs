using Nashet.Controllers;
using Nashet.FlagGeneration;
using UnityEngine;

namespace Nashet.GameplayView
{
	public class UnitView : MonoBehaviour
	{
		[SerializeField]
		private SpriteRenderer sprite;

		private int unitId;
		private IMapController mapController;

		public void SetFlag(Sprite sprite)
		{
			this.sprite.sprite = sprite;
		}

		private void Start()
		{
			var texture = FlagGenerator.Generate(128, 128);
			Sprite sprite2 = Sprite.Create(texture, new Rect(0, 0, 128, 128), new Vector2(1f, 1f));
			SetFlag(sprite2);
		}

		public void Subscribe(IMapController mapController, int unitID)
		{
			this.unitId = unitID;
			mapController.OnUnitDied += UnitDiedHandler;
			mapController.UnitMoved += UnitMovedHandler;
			this.mapController = mapController;
		}

		private void UnitMovedHandler(Vector2Int from, Vector2Int toPosition, int unitId)
		{
			if (unitId != this.unitId)
				return;
			transform.position = mapController.GetOffsetedPosition(toPosition);
		}

		private void UnitDiedHandler(Vector2Int position, int unitId)
		{
			if (this.unitId == unitId)
			{
				Destroy(gameObject);//todo use pool
			}
		}
	}
}
