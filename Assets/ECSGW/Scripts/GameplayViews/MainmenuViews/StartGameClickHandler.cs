
using UnityEngine;

namespace Nashet.MainMenu
{
	public delegate void OnGameStartClicked();
	public class StartGameClickHandler : MonoBehaviour
	{

		public event OnGameStartClicked gameStartClicked;
		
		public void ClickHandler()
		{
			gameStartClicked?.Invoke();
		}
	}
}