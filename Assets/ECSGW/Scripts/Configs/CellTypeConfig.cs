using UnityEngine;

namespace Nashet.Configs

{
	[CreateAssetMenu(fileName = "CellTypeConfig", menuName = "ECSWGConfigs/CellTypeConfig")]
	public class CellTypeConfig : ScriptableObject
	{
		public string Id;
		public Sprite sprite;
		public float travelCost = 1f;
	}
}
