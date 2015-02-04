using UnityEngine;
using System.Collections;

public class AI_explosion_if_player_is_close : MonoBehaviour {
	public float range =8f;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		GameObject spaceShip=GameObject.Find("spaceShip2");
		if (Vector3.Distance (spaceShip.transform.position, transform.position) < range) {
			Destroy (gameObject);

			GameObject explosion=((GameObject)Instantiate(Resources.Load("explosion"),transform.position,transform.rotation));
			explosion.transform.localScale=new Vector3(5,5,5);
			Destroy(explosion,3f);
			//decrement ship life if is not undestroyable and add to center of tunel, set undestroyable for 3 seconds;

		}
	}

}
