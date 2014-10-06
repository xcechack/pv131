using UnityEngine;
using System.Collections;

public class asteroidGenerator : MonoBehaviour {
	
	// Use this for initialization
	private GameObject s;
	private ArrayList myNodes;
	public uint numberOfAsteroids=100; 
	void Start ()
	{
		
		
		myNodes = new ArrayList();
		float quadDist = 0.3f;
		for (int i = 0; i < numberOfAsteroids; i++)
		{
			//Physics.gravity = new Vector3 (1, 0, 0);
			s = GameObject.CreatePrimitive(PrimitiveType.Sphere);
			Rigidbody gameObjectsRigidBody = s.AddComponent<Rigidbody>(); // Add the rigidbody.
			gameObjectsRigidBody.mass = 10; // Set the GO's mass to 5 via the Rigidbody.
			gameObjectsRigidBody.useGravity=false;
			gameObjectsRigidBody.angularDrag =1f;
			s.transform.position = new Vector3 ((Random.value*100f), (Random.value*100f), (Random.value*100f));
			
			s.transform.localScale=new Vector3 (Random.value*5+5,Random.value*5+5,Random.value*5+5);
			Vector3[] a = s.GetComponent<MeshFilter>().mesh.vertices;
			bool[] editedVertices = new bool[a.Length];
			for(int l=0;l<a.Length;l++) editedVertices[l]=false;
			
			Vector3 var;
			for(int t=0; t<a.Length;t++){
				float x=Random.value*0.1f;
				float y=Random.value*0.3f;
				float z=Random.value*0.3f;
				var=a[t];
				for(int d=0;d<a.Length;d++){
					//Mathf.Abs(a[t].x-a[d].x)<0.01 && Mathf.Abs(a[t].y-a[d].y)<0.01 && Mathf.Abs(a[t].z-a[d].z)<0.01
					if(Mathf.Abs(var.x-a[d].x)<quadDist && Mathf.Abs(var.y-a[d].y)<quadDist && Mathf.Abs(var.z-a[d].z)<quadDist/*var.Equals(a[d])*/){
						if(editedVertices[d]==false){
							editedVertices[d]=true;
							a[d].x+=x;
							a[d].y+=x;
							a[d].z+=x;
						}
					}
				}
			}
			s.GetComponent<MeshFilter>().mesh.vertices=a;
			s.GetComponent<MeshFilter>().mesh.RecalculateNormals();
			
			s.GetComponent<MeshFilter>().mesh.RecalculateBounds();
			s.GetComponent<MeshFilter>().mesh.Optimize();
			//s.GetComponent<MeshFilter>().mesh.Optimize();
		}
		
		//s.AddComponent<SphereCollider>();
		
		myNodes.Add(s);
		
	}
	// Update is called once per frame
	void Update () {
		/*for (int i=0; i<myNodes.Count; i++) {
			((GameObject)myNodes[i]).rigidbody.useGravity=true;
			((GameObject)myNodes[i]).rigidbody.AddForce (Physics.gravity);
			((GameObject)myNodes[i]).rigidbody.mass = 100;
			((GameObject)myNodes[i]).rigidbody.angularDrag = 0.005f;
			((GameObject)myNodes[i]).rigidbody.useGravity = true;
		}*/
		//rigidbody.velocity += 10 * Time.fixedTime * (largeObject.transform.position - transform.position);
	}
}
