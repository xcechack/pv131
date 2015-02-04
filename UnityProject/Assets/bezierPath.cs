using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
//using GL;


public class bezierPath : MonoBehaviour {
	public int range = 10; // range of random controll line difference rotation 
	public int pathScale=20;
	public float tunelScale=20;
	public int numberOfLines=25;
	public bool enemyEnabled = true;

	public float minTimeToNewEnemy=2f;
	public float maxTimeToNewEnemy=8f;
	public float enemyLifeTime=20f;

	GameObject[] enemyTypes;
	List<GameObject> asteroids;
	GameObject pickUp;
	Color c1 = Color.cyan;
	Color c2 = Color.magenta;
	int lengthOfLineRenderer = 100;
	LineRenderer[] renderer; // renderuje ciary okolo tunela
	int positionIndex=10;
	List<Vector3> listOfPoints = new List<Vector3>();
	Vector3[] points;
	public List<Vector3> vertices; //aktualni souradnice tunelu
	uint verticesCounter=0;
	uint divisionLevel = 0;
	
	enum nodes{
		anchor1=0 ,
		anchor2=3 ,
		control1=1 ,
		control2=2 
	};
	GameObject s;
	Vector3[] ngon;
	void Start() {

		enemyTypes = new GameObject[2];
		enemyTypes[0]=(GameObject)Resources.Load("Guardian");
		enemyTypes[1]=(GameObject)Resources.Load("DinamicBomb");

		pickUp = Resources.Load("pickUpObject", typeof(GameObject)) as GameObject;
		asteroids = new List<GameObject> ();
		renderer = new LineRenderer[numberOfLines];
		
		for (int i=0; i<numberOfLines; i++){
			GameObject newLine = new GameObject("Line");
			renderer [i] = newLine.AddComponent<LineRenderer>();
			//renderer [i].material = new Material (Shader.Find ("Particles/Additive"));
			renderer [i].SetColors (c1, c2);
			renderer [i].SetWidth (0.2F, 0.2F);
			renderer [i].SetVertexCount (lengthOfLineRenderer);
			
		}
		
		listOfPoints.Add (new Vector3 (0 , 0 , 0 ));
		listOfPoints.Add (new Vector3 (10 , 0 , 0 ));
		listOfPoints.Add (new Vector3 (15 , 0 , 0 ));
		listOfPoints.Add (new Vector3 (20 , 0 , 0 ));
		generateRandomControlls(ref listOfPoints);
		generateRandomControlls(ref listOfPoints);
		
		print (listOfPoints.Count+" numberOfControlls");		
		string str = "";
		for (int i=0; i<listOfPoints.Count; i++) {
			str+=listOfPoints[i];		
		}
		print (str);
		initStaticBezierCurve(listOfPoints,50);
		generateAsteroidsForLastSegment ();
	}
	
	void generateRandomControlls(ref List <Vector3> points){
		
		Vector3 anchor1 = points[points.Count-1];
		float scalar =  UnityEngine.Random.Range (5f, 8f);
		scalar *= (pathScale/2);
		Vector3 tmpVec=(-points[points.Count-2] + anchor1).normalized*scalar; // normalized vectore to ensure fixed range of scalar multiplication
		
		Vector3 control1 = anchor1 + tmpVec;
		Vector3 control2 = anchor1 + (tmpVec.normalized * 15*pathScale);
		Vector3 anchor2 = Quaternion.Euler (UnityEngine.Random.Range (-30,30),
		                                    UnityEngine.Random.Range (-30, 30),
		                                    UnityEngine.Random.Range (-30,30)) * tmpVec.normalized;
		anchor2 = control2 + (anchor2*scalar);
		Vector3[] outNodes = new Vector3[4];
		points.Add (control1);
		points.Add (control2);
		points.Add (anchor2);
		print (outNodes [0] + " " + outNodes [1] + " " + outNodes [2] + " " + outNodes [3]);
	}
	
	
	float TimeCounter=20f;
	float timeToAddEnemy=2f;
	void Update () {

		if (TimeCounter <= 0) {
			generatePickUpForLastSegment ();
			print ((((int)Time.time) % 20) + " time");
			TimeCounter=20f;
		} else {
			TimeCounter-=Time.deltaTime;
		}
		
		
		
		if (vertices.Count <= 100) {
			generateNewVertices ();
			print("generated new segment");
		}
		
		//Debug.DrawLine(new Vector3(0,0,0),new Vector3(1,1,1));
		
		Vector3[] vec = new Vector3[100];
		for (int i=0; i<100; i++) {
			vec=createNgon(numberOfLines,vertices[i]-vertices[i+1],tunelScale);
			for(int j=0;j<numberOfLines;j++){
				renderer [j].SetPosition (i, vec[j]+vertices[i]);
			}
		}
		
		GameObject ship = GameObject.Find ("spaceShip2");
		if (Vector3.Distance (vertices [positionIndex], ship.transform.position) >
		    Vector3.Distance (vertices [positionIndex + 1], ship.transform.position)) {
			vertices.RemoveAt (0);
			generateAsteroidsForLastSegment();
		}
		
		if(asteroids.Count>=50){
			Destroy(asteroids[0]);
			asteroids.RemoveAt(0);
			print(asteroids.Count+" removed");
		}
		
		if (Input.GetKeyDown ("up")) {
			l++;
			if(l>listOfPoints.Count/3-1)
				l=0;
		}
		
		if (Vector3.Distance (ship.transform.position, vertices [positionIndex]) > tunelScale){
			print("out"+Vector3.Distance (ship.transform.position, vertices [positionIndex]));
			ship.transform.position=vertices[positionIndex];//LineRenderer lineRenderer = GetComponent<LineRenderer>();
		}

		if (enemyEnabled) {
						if (timeToAddEnemy < 0) {
								if (vertices.Count > 50)
										Destroy ((GameObject)Instantiate (getRandomEnemy (), vertices [vertices.Count - UnityEngine.Random.Range (20, 49)], transform.rotation), enemyLifeTime);
								else
										Destroy ((GameObject)Instantiate (getRandomEnemy (), vertices [vertices.Count - 1], transform.rotation), enemyLifeTime);
			
								timeToAddEnemy = UnityEngine.Random.Range (minTimeToNewEnemy, maxTimeToNewEnemy);
								print ("released");
						} else {
								timeToAddEnemy -= Time.deltaTime;
						}
				}

		drawControls ();
	}
	
	//safe just once !!!
	bool done=false;
	void initStaticBezierCurve(List<Vector3> points, uint divisionLevel)
	{	
		if (!done) {
			if (points.Count < 4 || (points.Count % 3) != 1)
				return;
			
			if (this.divisionLevel != divisionLevel) {
				this.divisionLevel = divisionLevel;
			}
		}
		this.vertices = new List<Vector3>();
		done = true;
		countVertices();
		print (verticesCounter + (" dfafs"));
	}
	
	/**	getPointOnCurve(Vertex3D * points, GLfloat param, Vertex3D* outVertex)
*   inputParams :  Vertex3D * points - pointer on first anchor node of BEZIER CURVE segment
*                  GLfloat param     - number between 0 and 1 for parametric equation of BEZIER CURVE
*	outParams	:  Vertex3* outVertex- vertex of bezier curve given by parameter GLfloat param
*/
	Vector3 getPointOnCurve(Vector3[] points, double param){
		
		
		Vector3 outVertex;
		
		outVertex.x = (float)(Math.Pow(param, 3)*(points[(uint)nodes.anchor2].x + 3 * (points[(uint)nodes.control1].x - points[(uint)nodes.control2].x) - points[(uint)nodes.anchor1].x)
		                      + 3 * Math.Pow(param, 2)*(points[(uint)nodes.anchor1].x - 2 * points[(uint)nodes.control1].x + points[(uint)nodes.control2].x)
		                      + 3 * param*(points[(uint)nodes.control1].x - points[(uint)nodes.anchor1].x) + points[(uint)nodes.anchor1].x);
		
		outVertex.y = (float)(Math.Pow(param, 3)*(points[(uint)nodes.anchor2].y + 3 * (points[(uint)nodes.control1].y - points[(uint)nodes.control2].y) - points[(uint)nodes.anchor1].y)
		                      + 3 * Math.Pow(param, 2)*(points[(uint)nodes.anchor1].y - 2 * points[(uint)nodes.control1].y + points[(uint)nodes.control2].y)
		                      + 3 * param*(points[(uint)nodes.control1].y - points[(uint)nodes.anchor1].y) + points[(uint)nodes.anchor1].y);
		
		outVertex.z = (float)(Math.Pow(param, 3)*(points[(uint)nodes.anchor2].z + 3 * (points[(uint)nodes.control1].z - points[(uint)nodes.control2].z) - points[(uint)nodes.anchor1].z)
		                      + 3 * Math.Pow(param, 2)*(points[(uint)nodes.anchor1].z - 2 * points[(uint)nodes.control1].z + points[(uint)nodes.control2].z)
		                      + 3 * param*(points[(uint)nodes.control1].z - points[(uint)nodes.anchor1].z) + points[(uint)nodes.anchor1].z);
		//outVertex *= pathMultiplicator;
		return outVertex;
	}
	
	/*count vertices for controlls in point list, it is neccessary to delete controlls of curves that had been generated because it causes 
	 errors in structure of tunel*/
	void countVertices(){
		
		if ((this.listOfPoints.Count < 4) && ((this.listOfPoints.Count % 3) != 1))
			return;
		
		Vector3 point;
		Vector3[] points=new Vector3[4];
		for (int i = 0; i < ((this.listOfPoints.Count) / 3); i++){
			points[0]=this.listOfPoints[i*3];
			points[1]=this.listOfPoints[i*3+1];
			points[2]=this.listOfPoints[i*3+2];
			points[3]=this.listOfPoints[i*3+3];
			//listOfPoints.RemoveRange(0,4);
			float u = 0;
			for (int j = 0; j < divisionLevel; j++){
				u = u + 1.0f / divisionLevel;
				point = getPointOnCurve(points, u);
				vertices.Add(point);
			}
		}
	}
	
	/* generates new */
	void generateNewVertices(){
		generateRandomControlls (ref listOfPoints);
		listOfPoints.RemoveRange(0, listOfPoints.Count-4);
		countVertices();
		generateAsteroidsForLastSegment ();
		
	}
	
	int l=0;
	void drawControls(){
		
		for (int i=0; (i<vertices.Count-1); i++) {
			Debug.DrawRay(vertices[i + 0], vertices[i + 1]-vertices[i + 0], Color.white);
		}
		
		for (int i=0; i<listOfPoints.Count/3; i++) {
			Debug.DrawRay (listOfPoints [i * 3 + 0], listOfPoints [i * 3 + 1] - listOfPoints [i * 3 + 0], Color.red);
			Debug.DrawRay (listOfPoints [i * 3 + 2], listOfPoints [i * 3 + 3] - listOfPoints [i * 3 + 2], Color.yellow);
		}
		
	}
	
	Vector3[] createNgon(int n,Vector3 p_normal,float scale){
		//GLfloat *ngon = (GLfloat*) malloc(sizeof(GLfloat) * 3 * n);
		Vector3[] ngon=new Vector3[n];
		for (int i = 0; i < n; i += 1) {
			ngon[i].x = ((float) Math.Sin(i / (float) n * 2 * 3.1415926));
			ngon[i].y = ((float) Math.Cos(i / (float) n * 2 * 3.1415926));
			ngon[i].z = 0;
		}
		Vector3 normal = new Vector3 (0, 0, 1);
		Vector3 rotationVector = Vector3.Cross(normal, p_normal);
		float angle = Vector3.Angle (p_normal, normal);
		for(int i=0;i< ngon.Length;i++) {
			ngon[i]=Quaternion.AngleAxis(angle,rotationVector)*ngon[i]*scale;		
		}
		return ngon;
	}
	
	void generateAsteroidsForLastSegment(){
		float quadDist = 0.3f;
		Texture2D inputTexture = (Texture2D)Resources.Load ("asteroid1", typeof(Texture2D));
		for (int i = (int)divisionLevel; i < (int)divisionLevel+1; i++) {
			//Physics.gravity = new Vector3 (1, 0, 0);
			s = GameObject.CreatePrimitive (PrimitiveType.Sphere);
			s.collider.isTrigger = true;
			Rigidbody gameObjectsRigidBody = s.AddComponent<Rigidbody> (); // Add the rigidbody.
			gameObjectsRigidBody.mass = 10; // Set the GO's mass to 5 via the Rigidbody.
			gameObjectsRigidBody.useGravity = false;
			gameObjectsRigidBody.angularDrag = 1f;
			s.transform.position = new Vector3 ((vertices [i].x + UnityEngine.Random.value * tunelScale * 2 - tunelScale), 
			                                    (vertices [i].y + UnityEngine.Random.value * tunelScale * 2 - tunelScale), 
			                                    (vertices [i].z + UnityEngine.Random.value * tunelScale * 2 - tunelScale));
			
			s.transform.localScale = new Vector3 (UnityEngine.Random.value * 5 + 5, UnityEngine.Random.value * 5 + 5, UnityEngine.Random.value * 5 + 5);
			Vector3[] a = s.GetComponent<MeshFilter> ().mesh.vertices;
			bool[] editedVertices = new bool[a.Length];
			for (int l=0; l<a.Length; l++)
				editedVertices [l] = false;
			
			Vector3 var;
			for (int t=0; t<a.Length; t++) {
				float x = UnityEngine.Random.value * 0.1f;
				float y = UnityEngine.Random.value * 0.3f;
				float z = UnityEngine.Random.value * 0.3f;
				var = a [t];
				for (int d=0; d<a.Length; d++) {
					//Mathf.Abs(a[t].x-a[d].x)<0.01 && Mathf.Abs(a[t].y-a[d].y)<0.01 && Mathf.Abs(a[t].z-a[d].z)<0.01
					if (Mathf.Abs (var.x - a [d].x) < quadDist && Mathf.Abs (var.y - a [d].y) < quadDist && Mathf.Abs (var.z - a [d].z) < quadDist/*var.Equals(a[d])*/) {
						if (editedVertices [d] == false) {
							editedVertices [d] = true;
							a [d].x += x;
							a [d].y += x;
							a [d].z += x;
						}
					}
				}
			}
			s.GetComponent<MeshFilter> ().mesh.vertices = a;
			s.GetComponent<MeshFilter> ().mesh.RecalculateNormals ();
			
			//s.GetComponent<MeshFilter> ().mesh.RecalculateBounds ();
			//s.GetComponent<MeshFilter> ().mesh.Optimize ();
			
			if (inputTexture == null)
				print ("texture is null");
			s.renderer.material.mainTexture = inputTexture;
			s.tag = "Player";
			asteroids.Add(s);
		}
	}
	
	//generuje pluska pro pridani zivota
	void generatePickUpForLastSegment(){
		GameObject nextPickUp = (GameObject)Instantiate(pickUp);
		
		//pickUp.AddComponent(
		Vector3 actualTunelPosition = vertices[vertices.Count - 1]; //aktualni souradnice stredu tunelu
		nextPickUp.transform.position = actualTunelPosition;
		
		Destroy (nextPickUp, 40f);
	}
	GameObject getRandomEnemy(){
		return enemyTypes [(UnityEngine.Random.Range(0, 100))%2];
	}
}

