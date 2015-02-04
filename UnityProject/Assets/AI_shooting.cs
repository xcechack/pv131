using UnityEngine;
using System.Collections;

public class AI_shooting : MonoBehaviour {
	public float farthestVisibleSpot=10f;
	public float timeToNextShot=0.5f;
	GameObject shot;
	// Use this for initialization
	void Start () {
		shot=(GameObject)Resources.Load("shoot2");
	}
	
	// Update is called once per frame
	void Update () {
		GameObject spaceShip=GameObject.Find("spaceShip2");
		if (Vector3.Distance (spaceShip.transform.position, transform.position) < farthestVisibleSpot) {
			if(timeToNextShot<=0){

				Vector3 dir=(spaceShip.transform.position-transform.position).normalized;
				GameObject a = (GameObject)Instantiate (shot, transform.position+(dir*3), Quaternion.LookRotation(dir)*Quaternion.Euler(new Vector3(90,0,0)));
				Destroy(a,5f);
				timeToNextShot=0.5f;
			}else{
				timeToNextShot-=Time.deltaTime;
			}
		}
	}
}
