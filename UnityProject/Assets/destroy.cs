using UnityEngine;
using System.Collections;

public class destroy : MonoBehaviour
{
	public GameObject explosion;
	public GameObject playerExplosion;
	private static int counter = 0;

	void OnTriggerEnter(Collider other) 
	{
		Destroy(Instantiate(explosion, transform.position, transform.rotation),2f);
		if (other.tag == "Player")
		{
			Destroy(Instantiate(playerExplosion, other.transform.position, other.transform.rotation),2f);
		}
		Destroy(other.gameObject);
		Destroy(gameObject);
		counter++;
		TextMesh score = GameObject.Find ("Score").GetComponent<TextMesh> ();
		score.text = counter.ToString();
	}
}