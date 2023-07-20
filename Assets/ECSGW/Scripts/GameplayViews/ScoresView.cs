using System;
using UnityEngine;
using TMPro;

namespace Nashet.GameplayView
{
	public interface IScoresView
	{
		void DisplayScores(int scores);
	}

	public class ScoresView : MonoBehaviour, IScoresView
	{
		[SerializeField] private TextMeshProUGUI textMeshPro;
		public void DisplayScores(int scores)
		{
			textMeshPro.text = scores.ToString();
		}

		//public void Subscribe(ScoresController scoresController)
		//{
		//	scoresController.ScoreChanged += ScoreChangedHandler;
		//}

		private void ScoreChangedHandler(int oldValue, int newValue)
		{
			DisplayScores(newValue);
		}
	}
}
