using UnityEngine;

namespace Nashet.Initialization
{
	public class BootLoader : MonoBehaviour
	{
		void Awake()
		{
			var game = new Initializer();
			game.Initialise();
		}
	}
}
