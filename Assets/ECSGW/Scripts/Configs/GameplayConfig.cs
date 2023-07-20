using UnityEngine;

namespace Nashet.Configs

{
	[CreateAssetMenu(fileName = "GameplayConfig", menuName = "ECSWGConfigs/GameplayConfig")]
	public class GameplayConfig : ScriptableObject
	{
		public int ExplodeLineTreshold = 3;
	}
}
