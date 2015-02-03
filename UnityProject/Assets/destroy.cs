using UnityEngine;
using System.Collections;

public class destroy : MonoBehaviour
{
	public GameObject explosion;
	public GameObject playerExplosion;
	//private static int counter = 0;
	
	void OnTriggerEnter(Collider other) 
	{
		if (other.tag == "Player") {
			Instantiate (playerExplosion, transform.position, transform.rotation);
			GameObject.Find ("ship").GetComponent<MultiplayerControl> ().speed = 0;
			GameObject.Find ("spaceShip(Clone)").GetComponent<MultiplayerControl> ().speed = 0;
			StartCoroutine (Example (2.0F));
			//GUI.Label(new Rect(100,100,100,100), "Player won!!");
		} else {
			
			Destroy (Instantiate (playerExplosion, other.transform.position, other.transform.rotation), 2f);
			Destroy (other.gameObject);
			Destroy (gameObject);
			//counter++;
			//TextMesh score = GameObject.Find ("Score").GetComponent<TextMesh> ();
			//score.text = counter.ToString();
		}
	}
	
	IEnumerator Example(float wait) {
		yield return new WaitForSeconds(wait);
		//Destroy (GameObject.Find("spaceShip(Clone)"));
		Network.Disconnect();
		MasterServer.UnregisterHost();
		Application.LoadLevel("menu");
	}
}