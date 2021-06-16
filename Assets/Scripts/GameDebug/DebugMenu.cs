using RapidGUI;
using UnityEngine;

namespace GameDebug
{
	public class DebugMenu : MonoBehaviour
	{
		private WindowLaunchers _launchers = null!;

		public void Start()
		{
			// if WindowLaunchers.isWindow == true(default)
			// WindowLaunchers will be wrapped in window.
			// child windows automaticaly aligned.
			_launchers = new WindowLaunchers
			{
				name = "Debug Menu"
			};
            
			_launchers.Add("Debug Status Menu", typeof(DebugHarmMenu));
		}

		private void OnGUI()
		{
			_launchers.DoGUI();
		}
	}
}
