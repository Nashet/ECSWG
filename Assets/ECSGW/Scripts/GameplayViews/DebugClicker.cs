using UnityEngine;

namespace Nashet.GameplayView
{
	public delegate void OnSimulateStepHappened();

	public class DebugClicker : MonoBehaviour
	{
		public event OnSimulateStepHappened SimulateStepHappened;
		public void ClickHandler()
		{
			SimulateStepHappened?.Invoke();
		}

		public void RefreshFieldClickHandler()
		{

		}
	}
}
