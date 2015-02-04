using UnityEngine;
using System.Collections;

public class PickUpRotation : MonoBehaviour {
	
	bool pickedUp = false;
	
	void Start () {
		
	}
	
	void Update() {
		// Slowly rotate the object around axis at 180 degree/second.
		transform.Rotate(Vector3.down * Time.deltaTime*180);

		}
	}
	

