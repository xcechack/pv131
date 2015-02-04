using UnityEngine;
using System.Collections;
using System.Collections.Generic;



public class AI_avoiding_collisions : MonoBehaviour {
	public float speed=5f;
	public float rangeRadius=1f;
	GameObject[] spherearr;
	List<GameObject> collidingObjects;
	// points on nearest point of bezierPath;
	uint positionPointer=0;

	void Start () {
		collidingObjects = new List<GameObject> ();

		gameObject.AddComponent<Rigidbody> ().useGravity=false;
		gameObject.GetComponent<Rigidbody> ().velocity = transform.forward;
		gameObject.AddComponent<SphereCollider> ().isTrigger=true;
		gameObject.transform.GetComponent<SphereCollider>().radius=rangeRadius;
		
		
	}
	bool rotateToNoCollisionDirection(){
		bool colliding = false;
		Vector3 forwardPoint = transform.forward  + transform.position;
		//print ("number of colliding objects = "+collidingObjects.Count);
		bezierPath script = GameObject.Find("bezierPath").GetComponent<bezierPath>();
		//print(Vector3.Distance (forwardPoint, script.vertices [(int)positionPointer])+">>>>>>"+script.tunelScale);

		if (collidingObjects.Count != 0 || 
		    Vector3.Distance (forwardPoint, script.vertices [(int)positionPointer])>script.tunelScale) {
			//print ("colliding");
			
			rigidbody.velocity=Vector3.zero;
			foreach (GameObject o in collidingObjects) {
				if(o==null)
					continue;
				float radius = (o.GetComponent<SphereCollider>().radius*o.transform.localScale.x);
				//print (o.GetComponent<SphereCollider>().radius+"    "+o.transform.localScale.x);

				
				if(Vector3.Distance(forwardPoint,o.transform.position)<radius || 
				   Vector3.Distance (forwardPoint, script.vertices [(int)positionPointer])>script.tunelScale){

					for(int i=0;i<20;i++){
						transform.Rotate(Random.Range(180,-180),Random.Range(180,-180),Random.Range(180,-180));
						if(Vector3.Distance(forwardPoint,o.transform.position)>radius){
							//print ("break for- matching right angles");
							break;
						}else if(Vector3.Distance (forwardPoint, script.vertices [(int)positionPointer])<script.tunelScale){
							break;
						}
					}
				}
				else{
					//print("error of detection "+Vector3.Distance(forwardPoint,o.transform.position) +"< radius=" + radius);
				}
			}
			if(Vector3.Distance (forwardPoint, script.vertices [(int)positionPointer])>script.tunelScale){
				
				for(int i=0;i<20;i++){
					transform.Rotate(Random.Range(180,-180),Random.Range(180,-180),Random.Range(180,-180));
					if(Vector3.Distance (forwardPoint, script.vertices [(int)positionPointer])<script.tunelScale){
						break;
					}
				}
			}
			
			foreach (GameObject o in collidingObjects) {
				if(o==null)
					continue;
				float radius = o.GetComponent<SphereCollider>().radius*o.transform.localScale.x;
				if(Vector3.Distance(forwardPoint,o.transform.position)<radius ){
					//print ("colliding=true");
					colliding=true;
				}else if(Vector3.Distance (forwardPoint, script.vertices [(int)positionPointer])>script.tunelScale){
					colliding=true;
				}
			}
		}
		if(colliding==false)
			rigidbody.velocity = transform.forward*speed;
		return colliding;
	}
	// Update is called once per frame
	void Update () {
		bezierPath script = GameObject.Find("bezierPath").GetComponent<bezierPath>();
		if (positionPointer == 0) {
			uint i=0;
			float dist=Vector3.Distance(script.vertices[(int)i],transform.position);
			float dist2;
			//Vector3 closestVertex;
			foreach (Vector3 vertex in script.vertices) {
				dist2=Vector3.Distance(transform.position,vertex);
				if(dist>dist2){
					dist=dist2;
					positionPointer=i++;
				}
			}
		}
		else{
			float predecessor = Vector3.Distance(transform.position,script.vertices[(int)positionPointer-1]);
			float successor = Vector3.Distance(transform.position,script.vertices[(int)positionPointer+1]);
			float actual = Vector3.Distance(transform.position,script.vertices[(int)positionPointer]);
			if(predecessor<actual)
				positionPointer--;
			if(successor<actual)
				positionPointer++;
			if(positionPointer>99)
				positionPointer=99;
		}
		//print (Vector3.Distance(transform.position,script.vertices[(int)positionPointer])+"<<<<<<<<<<<<<");
		rotateToNoCollisionDirection ();
		Debug.DrawLine(transform.position, transform.position+transform.forward, Color.white);
		
	}
	
	
	void OnTriggerEnter(Collider collision){
		if(collision.collider.GetType()==typeof(SphereCollider))
		if(!collidingObjects.Contains (collision.gameObject))
			collidingObjects.Add (collision.gameObject);
		
	}
	
	
	void OnTriggerExit(Collider other){
		collidingObjects.Remove (other.gameObject);
	}
	
	void OnDrawGizmos() {
		//Gizmos.color = Color.yellow;
		//Gizmos.DrawSphere(gameObject.transform.position, 1f);
	}
	
	List<int> getNoColisionDirection(Collider other){
		/*if (spherearr != null) 
		{
			spherearr [0].transform.position = gameObject.transform.position + (Quaternion.AngleAxis (20, transform.up) * transform.forward).normalized;
			spherearr [1].transform.position = gameObject.transform.position + (Quaternion.AngleAxis (-20, transform.up) * transform.forward).normalized;
			spherearr [2].transform.position = gameObject.transform.position + (Quaternion.AngleAxis (20, transform.right) * transform.forward).normalized;
			spherearr [3].transform.position = gameObject.transform.position + (Quaternion.AngleAxis (-20, transform.right) * transform.forward).normalized;
		}
		List<int> indices= new List<int>();
		if (collider.GetType () != typeof(SphereCollider)) {
			for (int i=0; i<4; i++) {
				if (!other.bounds.Contains (spherearr [i].transform.position))
					indices.Add (i);
			}
		} else {


		}*/
		//spherearr[indices[0]].transform.position=gameObject.transform.position+Quaternion.AngleAxis (150, transform.up)*transform.forward;
		
		//print (indices.Count);
		return null;// indices;
	}
	Vector3 getNewDirection(Vector3 oldDir,Vector3 pos,float shipColiderRadius, Vector3 spherePos, float sphereRadius){
		return new Vector3();
	}
	
	
}
