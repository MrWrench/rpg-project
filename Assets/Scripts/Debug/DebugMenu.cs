using RapidGUI;
using UnityEngine;

public class DebugMenu : MonoBehaviour
{
	WindowLaunchers launchers = null!;

	public void Start()
	{
		// if WindowLaunchers.isWindow == true(default)
		// WindowLaunchers will be wrapped in window.
		// child windows automaticaly aligned.
		launchers = new WindowLaunchers
		{
			name = "Debug Menu"
		};
            
		launchers.Add("Debug Status Menu", typeof(DebugHarmMenu));
	}
	
	void OnGUI()
	{
		launchers.DoGUI();
	}
}
