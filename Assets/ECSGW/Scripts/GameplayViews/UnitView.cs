using Nashet.Controllers;
using UnityEngine;

namespace Nashet.GameplayView
{
	public class UnitView : MonoBehaviour
	{
		[SerializeField]
		private SpriteRenderer sprite;

		private int unitId;
		private IMapController mapController;

		private void SetFlag(Sprite sprite)
		{
			this.sprite.sprite = sprite;
		}

		private void Start()
		{
			
		}

		public void Subscribe(IMapController mapController, int unitID, Sprite sprite)
		{
			this.unitId = unitID;
			mapController.OnUnitDied += UnitDiedHandler;
			mapController.UnitMoved += UnitMovedHandler;
			this.mapController = mapController;
			SetFlag(sprite);
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
