using Nashet.FlagGeneration;
using UnityEngine;

namespace Nashet.GameplayView
{
	public class UnitView : MonoBehaviour
	{
		[SerializeField]
		private SpriteRenderer sprite;

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
	}
}
