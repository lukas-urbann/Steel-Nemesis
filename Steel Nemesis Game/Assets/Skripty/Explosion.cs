using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour {
	AudioSource audioSource;
    public AudioClip explosion;

	private int rotation = 0;
	void Start () {
		audioSource = GetComponent<AudioSource>();
		audioSource.PlayOneShot(explosion);
		rotation = Random.Range(0,360);
		transform.rotation = Quaternion.Euler(0,0,rotation);
	}
	
	public void destroyexplostion(int destroyed)
	{
		if(destroyed == 1)
		{
			Destroy(gameObject);
		}
	}
}
