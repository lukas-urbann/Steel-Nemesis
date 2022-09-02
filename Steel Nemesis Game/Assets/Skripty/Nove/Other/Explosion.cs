using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    
	private int rotation = 0;
	
	private void Start ()
	{
		rotation = Random.Range(0,360);
		transform.rotation = Quaternion.Euler(0,0,rotation);
	}
	
	public void Destroy()
	{
		Destroy(gameObject);
	}
}
