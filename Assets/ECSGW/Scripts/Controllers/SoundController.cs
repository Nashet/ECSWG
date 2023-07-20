using Nashet.GameplayView;

namespace Nashet.Controllers
{
	public class SoundController
	{
		IMapViewSound view;
		public SoundController(IMapViewSound view)
		{
			this.view = view;
			//view.SlideHappenedHandler();
		}
	}
}