using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Laser
{
    public class BasicLaser : MonoBehaviour
    {
        public GameObject collisionEffect;
        
        protected void OnBecameInvisible() {
            Destroy(gameObject);
        }

        protected void OnCollisionEnter2D(Collision2D col)
        {
            if (col.gameObject.CompareTag("Laser"))
            {
                Destroy(gameObject);
                Instantiate(collisionEffect, transform.position, Quaternion.identity);
            }
        }
    }
}
