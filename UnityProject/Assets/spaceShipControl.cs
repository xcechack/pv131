using UnityEngine;
using System.Collections;

public class spaceShipControl : MonoBehaviour {

	GameObject shot;
	public Transform weapon;
	public float shotLifeTime=3f;
	public float rotationSpeed = 2f;  
	public float speed=50f;
	private float rotationY;
	private float rotationX;
	private float rotationZ;
	private Object actualShot;

	void Start(){
		shot = (GameObject)Resources.Load("shoot2");
	}


	
	void Update(){
		if (Input.GetKeyDown ("space")) {
			actualShot = Instantiate (shot, weapon.position, weapon.rotation);
			Destroy (actualShot,shotLifeTime);
		}
	}
	
	void FixedUpdate () {
						Quaternion rotX = Quaternion.AngleAxis (
			Input.GetAxis ("Roll") * rotationSpeed, Vector3.right);
						Quaternion rotY = Quaternion.AngleAxis (
			Input.GetAxis ("Yaw") * rotationSpeed, Vector3.up);
						Quaternion rotZ = Quaternion.AngleAxis (
			Input.GetAxis ("Pitch") * rotationSpeed, Vector3.forward);

						transform.rotation = transform.rotation * rotX;
						transform.rotation = transform.rotation * rotY;
						transform.rotation = transform.rotation * rotZ;

						Quaternion transf = transform.rotation;
						transform.position += transform.forward * speed * Time.deltaTime;
	}

	float ClampAngle (float angle,float min,float max) {
		if (angle < -360)
			angle +=360 ;
		if (angle > 360)
			angle -= 360;
		return Mathf.Clamp (angle, min, max);
	}
	
}
