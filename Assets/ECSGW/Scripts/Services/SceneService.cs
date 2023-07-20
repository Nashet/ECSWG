using Nashet.Utils;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Nashet.Services
{
	public class SceneService : ISceneService
	{
		private AsyncOperation LoadScene(string sceneName)
		{
			return SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single);
		}

		public AsyncOperation LoadGameplayScene()
		{
			return LoadScene("GameplayScene");
		}

		public AsyncOperation LoadMainMenu()
		{
			return LoadScene("MenuScene");
		}
	}

	public interface ISceneService : IService
	{
		AsyncOperation LoadGameplayScene();
		AsyncOperation LoadMainMenu();
	}
}
