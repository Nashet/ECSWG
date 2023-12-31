﻿using Nashet.Controllers;
using Nashet.ECS;
using Nashet.GameplayView;
using Nashet.Services;
using Nashet.Utils;
using System.Collections;
using UnityEngine;

namespace Nashet.Initialization
{
	public class MonoInjector : MonoBehaviour
	{
		[SerializeField] ECSRunner ECSRunner;
		[SerializeField] MapView mapView;
		[SerializeField] MapViewSound mapViewSound;
		[SerializeField] MapViewClicker mapViewClicker;
		[SerializeField] DebugClicker debugClicker;
		[SerializeField] IScoresView scoresView;
		[SerializeField] SelectedUnitView selectedUnitView;
		[SerializeField] MapController mapController;

		private IEnumerator Start()
		{
			yield return new WaitUntil(() => ServiceLocator.Instance.initialized);
			yield return new WaitUntil(() => ECSRunner.IsReady);

			var configService = ServiceLocator.Instance.Get<IConfigService>();

			var map = ECSRunner.world.GetSingleComponent<MapComponent>();

			mapController.Initialize(configService, map, ECSRunner); //todo may be delete it completly

			mapView.Subscribe(mapController);

			new SoundController(mapViewSound);

			mapViewClicker.CellClicked += mapController.HandleCellClicked;
			debugClicker.SimulateStepHappened += mapController.SimulateOneStep;//TODO its better to subscribe inside controller


			selectedUnitView.Subscribe(mapController);

			var scoresController = new ScoresController(scoresView);//new ScoresModel()
			scoresController.Subscribe(mapController);
			//scoresView.Subscribe(scoresController);
		}
	}
}
