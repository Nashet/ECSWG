using Leopotam.EcsLite;
using UnityEngine;

namespace Nashet.ECS
{
	public class ECSRunner : MonoBehaviour
	{
		public bool IsReady { get; private set; }

		public EcsWorld world { get; private set; }
		private IEcsSystems initSystems;
		private IEcsSystems updateSystems;
		private IEcsSystems fixedUpdateSystems;
		//[SerializeField] private ConfigurationSO configuration;
		//[SerializeField] private Text coinCounter;
		//[SerializeField] private GameObject gameOverPanel;
		//[SerializeField] private GameObject playerWonPanel;

		private void Start()
		{
			world = new EcsWorld();
			//var gameData = new GameData();

			//gameData.configuration = configuration;
			//gameData.coinCounter = coinCounter;
			//gameData.gameOverPanel = gameOverPanel;
			//gameData.playerWonPanel = playerWonPanel;
			//gameData.sceneService = Service<SceneService>.Get(true);

			initSystems = new EcsSystems(world)//, gameData)
				.Add(new InitSystem())

				;

			initSystems.Init();
			IsReady = true;

			updateSystems = new EcsSystems(world)//, gameData)	

#if UNITY_EDITOR
		// add debug systems for custom worlds here, for example:
		// .Add (new Leopotam.EcsLite.UnityEditor.EcsWorldDebugSystem ("events"))
		.Add(new Leopotam.EcsLite.UnityEditor.EcsWorldDebugSystem(entityNameFormat: "000"))
#endif
		.Add(new BattleSystem())
				;

			updateSystems.Init();

			updateSystems.Run();

			fixedUpdateSystems = new EcsSystems(world)// gameData)
				;

			fixedUpdateSystems.Init();
		}

		//private void Update()
		//{
		//	updateSystems.Run();
		//}

		//private void FixedUpdate()
		//{
		//	fixedUpdateSystems.Run();
		//}

		private void OnDestroy()
		{
			initSystems.Destroy();
			updateSystems.Destroy();
			fixedUpdateSystems.Destroy();
			world.Destroy();
		}
	}
}