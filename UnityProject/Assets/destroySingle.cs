using UnityEngine;
using System.Collections;

public class destroySingle : MonoBehaviour {

		public GameObject explosion;
		public GameObject playerExplosion;
		public static int counter = 0;
		
		void OnTriggerEnter(Collider other) 
		{
			Destroy(Instantiate(explosion, transform.position, transform.rotation),2f);
			if (other.tag == "Player")
			{
				Destroy(Instantiate(playerExplosion, other.transform.position, other.transform.rotation),2f);
			}
		Destroy(gameObject);
		if (other.name == "spaceShip2") {
				spaceShipControl_single script = other.GetComponent<spaceShipControl_single>();
				script.removeLife();
				return;
			}
			Destroy(other.gameObject);
			
			counter++;
			GameObject.Find ("dataTransfer").GetComponent<dataTransfer> ().data = counter;
			TextMesh score = GameObject.Find ("Score").GetComponent<TextMesh> ();
			score.text = counter.ToString();
		}
	}