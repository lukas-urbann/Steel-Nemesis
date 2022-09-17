using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLaser : MonoBehaviour {
	[SerializeField] private float speed = 5;
	public GameObject destroyAnim;

    void Update()
    {
        transform.Translate(Vector2.up * Time.deltaTime * speed);
    }

    void OnBecameInvisible() {
        Destroy(gameObject);
    }
}
