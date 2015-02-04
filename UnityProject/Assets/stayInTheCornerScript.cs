using UnityEngine;
using System.Collections;

public class stayInTheCornerScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
		transform.position = Camera.main.ViewportToWorldPoint(new Vector3(0,1,0));
	}
	
	// Update is called once per frame
	void Update () {
	

	}
}
