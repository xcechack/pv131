using UnityEngine;
using System.Collections;

public class IfSpaceGoToMainMenu : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update(){
		//quit game if escape key is pressed
		if (Input.GetKey(KeyCode.Space)) { Application.Quit();
			Application.LoadLevel("menu");
		}else if (Input.GetKey(KeyCode.Escape)) { Application.Quit();
			Application.LoadLevel("menu");
		}
	}
}
