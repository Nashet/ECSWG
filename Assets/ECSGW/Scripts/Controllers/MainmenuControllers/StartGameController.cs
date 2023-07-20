using UnityEngine;
using Nashet.Services;
using Nashet.Utils;

namespace Nashet.Controllers
{
	public class StartGameController : MonoBehaviour
	{
		public void HandleGameStartClicked()
		{
			var service = ServiceLocator.Instance.Get<SceneService>();
			service.LoadGameplayScene();
		}
	}
}