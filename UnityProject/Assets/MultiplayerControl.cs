using UnityEngine;
using System.Collections;

public class MultiplayerControl : MonoBehaviour {
		
		GameObject shot;
		public Transform weapon;
		public float shotLifeTime=3f;
		public float rotationSpeed = 2f;  
		public float speed=50f;
		private float rotationY;
		private float rotationX;
		private float rotationZ;
		private Object actualShot;
	private float lastSynchronizationTime = 0f;
	private float syncDelay = 0f;
	private float syncTime = 0f;
	private Vector3 syncStartPosition = Vector3.zero;
	private Vector3 syncEndPosition = Vector3.zero;

		void Start(){
			shot = (GameObject)Resources.Load("shoot");
		}
		
		void Awake () { Destroy (actualShot,shotLifeTime); }
		
		void Update(){
		if (networkView.isMine) {
						if (Input.GetKeyDown ("space")) {
								actualShot = Instantiate (shot, weapon.position, weapon.rotation);
								Awake ();	
						}
				}
		}
		
		void FixedUpdate () {
			if (networkView.isMine) {
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
				} else {
					SyncedMovement();
				}
		}

	private void SyncedMovement()
	{
		syncTime += Time.deltaTime;
		rigidbody.position = Vector3.Lerp(syncStartPosition, syncEndPosition, syncTime / syncDelay);
	}

	void OnSerializeNetworkView(BitStream stream, NetworkMessageInfo info)
	{
		Vector3 syncPosition = Vector3.zero;
		Vector3 syncVelocity = Vector3.zero;
		if (stream.isWriting)
		{
			syncPosition = rigidbody.position;
			stream.Serialize(ref syncPosition);
			
			syncVelocity = rigidbody.velocity;
			stream.Serialize(ref syncVelocity);
		}
		else
		{
			stream.Serialize(ref syncPosition);
			stream.Serialize(ref syncVelocity);
			
			syncTime = 0f;
			syncDelay = Time.time - lastSynchronizationTime;
			lastSynchronizationTime = Time.time;
			
			syncEndPosition = syncPosition + syncVelocity * syncDelay;
			syncStartPosition = rigidbody.position;
		}
	}
		
		float ClampAngle (float angle,float min,float max) {
			if (angle < -360)
				angle +=360 ;
			if (angle > 360)
				angle -= 360;
			return Mathf.Clamp (angle, min, max);
		}
		
	}
