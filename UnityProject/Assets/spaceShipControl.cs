using UnityEngine;
using System.Collections;

public class spaceShipControl : MonoBehaviour {
	
	
	
	public GameObject shot;
	public Transform weapon;
	public float shotLifeTime=3f;
	public float rotationSpeed = 2f;  
	public float speed=50f;
	//private float minimumY = -360;
	//private float maximumY = 360;
	private float rotationY;
	//private float minimumX = -360;
	//private float maximumX = 360;
	private float rotationX;
	//private float minimumZ = -360;
	//private float maximumZ = 360;
	private float rotationZ;
	private Object actualShot;
	
	void Awake () { Destroy (actualShot,shotLifeTime); }
	
	void Update(){
		if (Input.GetKeyDown ("space")) {
			actualShot = Instantiate (shot, weapon.position, weapon.rotation);
			Awake ();	
		}
	}
	
	void FixedUpdate () {
		
		
		
		Quaternion rotX = Quaternion.AngleAxis(
			Input.GetAxis("Roll") * rotationSpeed, Vector3.right);
		Quaternion rotY = Quaternion.AngleAxis(
			Input.GetAxis("Yaw") * rotationSpeed, Vector3.up);
		Quaternion rotZ = Quaternion.AngleAxis(
			Input.GetAxis("Pitch") * rotationSpeed, Vector3.forward);
		
		// I'm pretty sure Unity uses standard quaternion multiplication order,
		// but if this gives you the wrong results, try reversing the order of
		// multiplication:
		transform.rotation = transform.rotation * rotX;
		transform.rotation = transform.rotation * rotY;
		transform.rotation = transform.rotation * rotZ;
		
		// I'm not sure if Unity has a 'normalize' function for quaternions. If
		// it doesn't though, you can write one yourself:
		Quaternion transf = transform.rotation;
		//transform.rotation = transf;
		
		//rigidbody.transform.Rotate(transf.eulerAngles);
		transform.position += transform.forward * speed * Time.deltaTime;
		//rigidbody.AddForce (transform.forward);
		
		
		
		/*rotationX += Input.GetAxis("Roll") * rotationSpeed;
		rotationX = ClampAngle(rotationX, minimumX, maximumX);
		
		rotationY += Input.GetAxis("Yaw") * rotationSpeed;
		rotationY = ClampAngle(rotationY, minimumY, maximumY);
		
		rotationZ += Input.GetAxis("Pitch") * rotationSpeed;
		rotationZ = ClampAngle(rotationZ, minimumZ, maximumZ);
		
		transform.rotation = Quaternion.AngleAxis(rotationY, Vector3.up);
		transform.rotation *= Quaternion.AngleAxis (rotationX, Vector3.right);
		transform.rotation *= Quaternion.AngleAxis (rotationZ, Vector3.forward);*/
	}
	
	float ClampAngle (float angle,float min,float max) {
		if (angle < -360)
			angle +=360 ;
		if (angle > 360)
			angle -= 360;
		return Mathf.Clamp (angle, min, max);
	}
	
	
	
	
	
	// Use this for initialization
	void Start () {
		
	}
	
}
