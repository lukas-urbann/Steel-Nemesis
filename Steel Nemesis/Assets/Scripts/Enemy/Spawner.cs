using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Enemy
{
    public class Spawner : MonoBehaviour
    {
        public EnemyType enemyType;

        public GameObject enemyShip, enemyExplosion, enemyPortal;

        private void OnEnable()
        {
            enemyShip.SetActive(false);
            enemyExplosion.SetActive(false);
            enemyPortal.SetActive(false);
            StartCoroutine(SpawnEnemy());
        }

        private IEnumerator SpawnEnemy()
        {
            enemyPortal.SetActive(true);
            yield return new WaitForSeconds(0.2f);
            enemyShip.SetActive(true);
        }

        public void Death()
        {
            
        }
    }
}
