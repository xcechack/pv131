using UnityEngine;
using System.Collections;

public class SimpleMovement : MonoBehaviour {

	Rigidbody2D myPlace;
	Vector2 movement;
	float speed = 500f;
	float jumpHeight = 500f;
	Transform myTrans;


	void Start(){

		//myTrans = this.transform;
		//myPlace = this.rigidbody2D;
	
	}

	void Update(){
		//Move (Input.GetAxisRaw("Horizontal"));
		this.transform.Translate(Input.GetAxis("Horizontal"), 0, 0);
		this.transform.Translate (0, Input.GetAxis("Vertical"), 0);
	}

	//void Move (float horizontal){
//	
//		movement = myPlace.velocity;
//		movement.x = horizontal*speed*Time.deltaTime;
//		myPlace.velocity = movement;
//		transform.position.x (horizontal);
//	}


}
