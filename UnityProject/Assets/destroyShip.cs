using UnityEngine;
using System.Collections;

public class destroyShip : MonoBehaviour
{
	public GameObject explosion;
	public GameObject playerExplosion;
	
	void OnTriggerEnter(Collider other) {
		Instantiate (playerExplosion, transform.position, transform.rotation);
		GameObject.Find("ship").GetComponent<MultiplayerControl>().speed = 0;
		StartCoroutine(Example(2.0F));
	}

	IEnumerator Example(float wait) {
		yield return new WaitForSeconds(wait);
		Destroy (GameObject.Find("ship"));
		Destroy (GameObject.Find("spaceShip(Clone)"));
		Network.Disconnect();
		MasterServer.UnregisterHost();
		Application.LoadLevel("menu");
	}
}