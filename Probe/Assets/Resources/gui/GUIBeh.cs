using UnityEngine;
using System.Collections;

public class GUIBeh : MonoBehaviour {

	GUISkin startSkin;
	void Start () {
		startSkin=(GUISkin) Resources.Load("gui/skins/button_start");
	}
	
	void OnGUI()
	{
		if(GUI.Button(new Rect(20,20,100,30),"",startSkin.button))
		{
			//ACTION
		}
	}
}
