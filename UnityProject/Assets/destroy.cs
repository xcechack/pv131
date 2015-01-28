using UnityEngine;
using System.Collections;

public class destroy : MonoBehaviour
{
	public GameObject explosion;
	public GameObject playerExplosion;
	private static int counter = 0;

	void OnTriggerEnter(Collider other) 
	{
		Instantiate(explosion, transform.position, transform.rotation);
		if (other.tag == "Player")
		{
			Instantiate(playerExplosion, other.transform.position, other.transform.rotation);
		}
		Destroy(other.gameObject);
		Destroy(gameObject);
		counter++;
		TextMesh score = GameObject.Find ("Score").GetComponent<TextMesh> ();
		score.text = counter.ToString();
	}
}