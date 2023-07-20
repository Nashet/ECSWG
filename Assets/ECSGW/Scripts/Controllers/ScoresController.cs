using Leopotam.EcsLite;
using Nashet.GameplayView;
using System;
using UnityEngine;

namespace Nashet.Controllers
{
	public class ScoresController
	{
		private EcsWorld _model;
		private IScoresView view;

		public ScoresController(IScoresView view)//EcsWorld model, 
		{
			_model = null; //model
			this.view = view;
			//_model.ScoresChanged += OnScoresChanged;
		}

		public void AddScores(int scoresToAdd)
		{
			//_model.AddScores(scoresToAdd);
		}

		private void OnScoresChanged(int oldValue, int newValue)
		{
			view.DisplayScores(newValue);
		}

		public void Subscribe(MapController mapController)
		{
			mapController.ExplosionHappened += ExplosionHappenedHandler;
		}

		private void ExplosionHappenedHandler(Vector2Int where, int amount)
		{
			AddScores(amount * 10)
;		}
	}
}
