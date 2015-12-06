using UnityEngine;
using System.Collections;

public class GUIBeh : MonoBehaviour {

	GUISkin startSkin;
	void Start () {
		startSkin=(GUISkin) Resources.Load("gui/skins/button_start");
	}
	
	void OnGUI()
	{
		if(GUI.Button(new Rect(60,20,40,40),"Up"))
		{
			Physics2D.gravity=new Vector2(0,5);
		}
		if(GUI.Button(new Rect(20,60,40,40),"Left"))
		{
			Physics2D.gravity=new Vector2(-5,0);
		}
		if(GUI.Button(new Rect(100,60,40,40),"Right"))
		{
			Physics2D.gravity=new Vector2(5,0);
		}
		if(GUI.Button(new Rect(60,100,40,40),"Down"))
		{
			Physics2D.gravity=new Vector2(0,-5);
		}
	}
}
