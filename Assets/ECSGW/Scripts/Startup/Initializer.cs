using Nashet.Services;
using Nashet.Utils;
using System;
using System.Threading.Tasks;

namespace Nashet.Initialization
{
	public class Initializer
	{
		public Action GameInitialized;
		public Initializer()
		{

		}

		public void Initialise()
		{
			var configService = new ConfigService();
			var mapLoaderService = new MapLoaderService(configService);//new TestMapGeneratorService(configService);			
			var sceneManager = new SceneService();


			ServiceLocator.Instance.Add(configService);
			ServiceLocator.Instance.Add(mapLoaderService);
			ServiceLocator.Instance.Add(sceneManager);

			ServiceLocator.Instance.MarkReady();

			//todo move to some special class
			//SomeLoading();
		}

		private async void SomeLoading()
		{
			await Task.Delay(200);

			var task = ServiceLocator.Instance.Get<ISceneService>().LoadMainMenu();
			while (!task.isDone)
				await Task.Delay(20);
			NotifyInitializationIsDone();
		}

		private void NotifyInitializationIsDone()
		{
			GameInitialized?.Invoke();
		}
	}
}
