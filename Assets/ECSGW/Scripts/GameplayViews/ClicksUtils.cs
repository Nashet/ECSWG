using UnityEngine;
using UnityEngine.EventSystems;
//using UnityEngine.InputSystem;

namespace Nashet.GameplayView
{
	public static class ClicksUtils
	{
		private static bool IsPointerOverGameObject()
		{
			//check touch. priorities on touches
			if (Input.touchCount > 0)
			{
				return (Input.touches[0].phase == TouchPhase.Ended && EventSystem.current.IsPointerOverGameObject(Input.touches[0].fingerId));
			}

			//check mouse
			if (EventSystem.current.IsPointerOverGameObject())
				return true;

			return false;
		}

		// remake it to return mesh collider, on which will be chosen object
		public static Collider getRayCastMeshNumber(Camera camera)
		{
			RaycastHit hit;

			var isHovering = IsPointerOverGameObject();
			if (isHovering)
				return null;// -3; //hovering over UI
			else
			{
				if (!Physics.Raycast(camera.ScreenPointToRay(Input.mousePosition), out hit))
					return null;// -1;
			}
			return hit.collider;
		}
	}
}
