using UnityEngine;
using System.Collections;

public class crosshair : MonoBehaviour {

	Texture2D crosshairTexture;
	Rect position;
	static bool OriginalOn = true;
	
	void Start()
	{
		crosshairTexture=(Texture2D)Resources.Load("crosshair");
		position = new Rect((Screen.width - crosshairTexture.width) / 2, (Screen.height - 
		             		crosshairTexture.height) /2, crosshairTexture.width, crosshairTexture.height);
	}
	void Update(){
		position = new Rect((Screen.width - crosshairTexture.width) / 2, (Screen.height - 
		                                                                  crosshairTexture.height) /2, crosshairTexture.width, crosshairTexture.height);

	}
	void OnGUI()
	{
		if(OriginalOn == true)
		{
			GUI.DrawTexture(position, crosshairTexture);
		}
	}
}
