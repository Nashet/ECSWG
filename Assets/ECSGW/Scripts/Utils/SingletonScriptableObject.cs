using UnityEngine;

namespace Nashet.Utils
{
	public abstract class SingletonScriptableObject<T> : ScriptableObject where T : ScriptableObject
	{
		static T instance;
		public static T Instance
		{
			get
			{
				if (instance == null)
				{
					instance = Resources.Load<T>("ConfigHolder");//typeof(T).ToString() // todo fix
					(instance as SingletonScriptableObject<T>).OnInitialize();
				}
				return instance;
			}
		}

		protected virtual void OnInitialize() { }

	}
}
