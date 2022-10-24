using System;
using System.Collections;
using System.Collections.Generic;
using Controllers;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Other
{
    public class CreditDrop : MonoBehaviour
    {
        private SpriteRenderer sprite;
        private Color spriteColor;
        private Vector3 movement;
        public GameObject pickupEffect;

        private void OnEnable()
        {
            sprite = GetComponent<SpriteRenderer>();
        }

        private void Start()
        {
            movement = new Vector3(Random.Range(-2, 2f), Random.Range(-2f, 2f),0);
            spriteColor = sprite.color;
        }

        private void Update()
        {
            if (Pause.Instance.GetPauseState())
                return;
                
            movement = new Vector3(movement.x - Time.deltaTime * 0.2f, movement.y - Time.deltaTime * 0.2f, 0);
            transform.position += movement * Time.deltaTime;
            transform.Rotate(0,0, movement.x);
            spriteColor = new Color (spriteColor.r, spriteColor.g, spriteColor.b, spriteColor.a - Time.deltaTime * 0.05f);

            if (spriteColor.a <= 0)
                Destroy(gameObject);
        }

        private void OnBecameInvisible()
        {
            Destroy(gameObject);
        }

        public void PickUp()
        {
            Instantiate(pickupEffect, transform.position, Quaternion.identity);
        }
    }
}
