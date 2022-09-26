using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace Laser
{
    public class BasicLaser : MonoBehaviour
    {
        public GameObject collisionEffect;
        public AudioClip laserCollisionSfx;
        
        protected void OnBecameInvisible()
        {
            Destroy(gameObject);
        }

        protected void OnCollisionEnter2D(Collision2D col)
        {
            if (col.gameObject.CompareTag("Laser"))
            {
                if(col.gameObject.CompareTag(gameObject.tag) && col.gameObject.layer == gameObject.layer)
                    return;
                
                Controllers.Audio.Instance.PlaySound(laserCollisionSfx);
                Destroy(gameObject);
                Instantiate(collisionEffect, transform.position, Quaternion.identity);
            }
        }
    }
}
