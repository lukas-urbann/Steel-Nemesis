using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Controllers
{
    public class Prefabs : MonoBehaviour
    {
        public static Prefabs Instance;

        public GameObject basicEnemy, attackerEnemy, fighterEnemy, tankEnemy, scrap;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
            }
            else
            {
                Instance = this;
            }
        }
    }
}
