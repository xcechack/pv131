using UnityEngine;
using System.Collections;

public class shotScript : MonoBehaviour {
	public float speed=10;
	// Use this for initialization
	void Start () {
		rigidbody.velocity = transform.up*speed;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
