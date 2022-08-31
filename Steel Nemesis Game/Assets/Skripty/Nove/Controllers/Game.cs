using System;
using System.Collections;
using System.Collections.Generic;
using Other;
using Triggers;
using UnityEngine;

namespace Controllers
{
    public class Game : MonoBehaviour
    {
        public static Game Instance;

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

        public void GameOver()
        {
            TriggerType[] trigs;
            trigs = FindObjectsOfType<TriggerType>();

            List<GameObject> ships = new List<GameObject>();

            foreach (TriggerType type in trigs)
            {
                if(type.triggerType == TypeOfTrigger.ship)
                    ships.Add(type.transform.root.gameObject);
            }

            foreach (GameObject ship in ships)
            {
                ship.GetComponent<Death>().DeathEvent();
            }
        }
    }
}
