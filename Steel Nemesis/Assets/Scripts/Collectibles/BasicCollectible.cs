using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Collectible
{
    public enum CollectibleType
    {
        HP,
        Energy,
        Cooldown,
        Firepower,
        Damage,
        Speed,
        Barrier
    }
    
    public enum CollectiblePickupType
    {
        Positive,
        Negative
    }
    
    public class BasicCollectible : MonoBehaviour
    {
        public CollectibleType pickupType;
        public CollectiblePickupType type;
        public GameObject pickUpEffect;
        private Vector3 move;
        protected float value;

        private void Awake()
        {
            switch (type)
            {
                case CollectiblePickupType.Negative:
                    value = -0.1f;
                    break;
                case CollectiblePickupType.Positive:
                    value = 0.1f;
                    break;
            }
        }

        private void Start()
        {
            move = new Vector3(0, -1 + (Controllers.Wave.Instance.GetLevel() * 0.02f), 0);
        }

        private void Update()
        {
            transform.position += move * Time.deltaTime;
        }
        
        public float GetValue()
        {
            return value;
        }

        private void OnBecameInvisible()
        {
            Destroy(gameObject);
        }

        public void Collect()
        {
            Instantiate(pickUpEffect, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}
