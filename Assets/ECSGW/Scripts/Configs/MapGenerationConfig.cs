using System.Collections.Generic;
using UnityEngine;

namespace Nashet.Configs

{
	[CreateAssetMenu(fileName = "MapGenerationConfig", menuName = "ECSWGConfigs/MapGenerationConfig")]
	public class MapGenerationConfig : ScriptableObject
	{
		public int xSize = 30;
		public int ySize = 20;

		public List<CellTypeConfig> AllowedCellTypes = new List<CellTypeConfig>();
	}
}
