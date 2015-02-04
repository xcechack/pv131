using UnityEngine;
using System.Collections;

public class getScore : MonoBehaviour {

	// Use this for initialization
	void Start () {
				GameObject data = GameObject.Find ("dataTransfer");
		if (data != null) {
			print(data.GetComponent<dataTransfer> ().data.ToString());
			gameObject.GetComponent<TextMesh> ().text = "your score is: " + data.GetComponent<dataTransfer> ().data.ToString ();
	
		}
	}
}
