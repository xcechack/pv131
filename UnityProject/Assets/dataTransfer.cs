using UnityEngine;
using System.Collections;

public class dataTransfer : MonoBehaviour {
	public int data;
	// Use this for initialization
	void Start () {
		DontDestroyOnLoad (this);
		data=0;
	}
	
	// Update is called once per frame

}
