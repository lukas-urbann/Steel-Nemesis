using System;
using System.Collections;
using System.Collections.Generic;
using Enemy;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Other
{
    public class Death : MonoBehaviour
    {
        public GameObject explosionPrefab;
        private Spawner spawner;

        private void OnEnable()
        {
            spawner = GetComponent<Spawner>();
        }

        public void DeathEvent()
        {
            if (!gameObject.CompareTag("Player"))
                Instantiate(explosionPrefab, spawner.enemyShip.transform.position, Quaternion.identity);
            else
                Instantiate(explosionPrefab, transform.position, Quaternion.identity);

            Destroy(gameObject);
        }
    }
}
