using UnityEngine;
using System.Collections;

public class spaceShipControl_single : MonoBehaviour {
		
	GameObject shot;
	public int life=5;
	public int maxLife=5;
	public Transform weapon;
	public float shotLifeTime=3f;
	public float rotationSpeed = 2f;  
	public float speed=50f;
	private float rotationY;
	private float rotationX;
	private float rotationZ;
	private Object actualShot;

	public Light light1;
	public Light light2;
	public Light light3;
	public Light light4;
	public Light light5;
		

	public void addLife(){
		if (life < maxLife) {
						life++;
						activateLastLifeButton();
				}
			
	}

	public void removeLife(){
		if (life == 1) {
			Application.LoadLevel("gameOver");
		} else {
						life--;
						deactivateLastLifeButton();

		}
	}

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

		void OnTriggerEnter(Collider other) 
		{
			if (other.name == "Sphere") {
						other.transform.position = Vector3.zero;
						Destroy (other);
						Destroy (Instantiate ((GameObject)Resources.Load ("explosion"), transform.position, transform.rotation), 2f);
			
						removeLife ();
						return;
			
				} else if (other.gameObject.tag == "PickUp") {
						print("DEBUG: Player just picked up a life!");
						Destroy(other);
						addLife();

				}


	
		
	}
	//turns on the light on the last life indicator
	void activateLastLifeButton(){
		print ("DEBUG: Going to activate a Life Button on the screen.");
		Light lightToTurnOn = null;

		if (light2.intensity == 0)
			lightToTurnOn = light2;
		else if (light3.intensity == 0)
			lightToTurnOn = light3;
		else if (light4.intensity == 0)
			lightToTurnOn = light4;
		else if (light5.intensity == 0)
			lightToTurnOn = light5;
		print ("DEBUG: light to turn on: " + lightToTurnOn.ToString ());
		if (lightToTurnOn != null)
						lightToTurnOn.intensity = 8; //turns on the light
		else
						print ("DEBUG: lightToTurnOn is null, something is wrong..."); //should never be null but just to be sure :-)


	}

	//turns off the light on the last life indicator
	void deactivateLastLifeButton(){
		print ("DEBUG: Going to deactivate a Life Button on the screen.");
		Light lightToTurnOff = null;

		if (light5.intensity != 0)
						lightToTurnOff = light5;
				else if (light4.intensity != 0)
						lightToTurnOff = light4;
				else if (light3.intensity != 0)
						lightToTurnOff = light3;
				else if (light2.intensity != 0)
						lightToTurnOff = light2;

		if (lightToTurnOff!=null)lightToTurnOff.intensity = 0; //turns off the light
		//should never be null but just to be sure 

	}
}
