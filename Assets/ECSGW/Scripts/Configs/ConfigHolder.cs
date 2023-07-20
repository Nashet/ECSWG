using Nashet.Utils;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Nashet.Configs

{
	[CreateAssetMenu(fileName = "ConfigHolder", menuName = "ECSWGConfigs/ConfigHolder")]
	public class ConfigHolder : SingletonScriptableObject<ConfigHolder>
	{
		public List<ScriptableObject> AllConfigs;
	}
}
