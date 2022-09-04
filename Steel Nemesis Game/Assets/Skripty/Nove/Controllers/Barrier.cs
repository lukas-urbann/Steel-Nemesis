using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Controllers
{
    public class Barrier : MonoBehaviour
    {
        public static Barrier Instance;
        private int hp = 2;
        public AudioClip barrierHit;
        public List<GameObject> hpSymbols = new List<GameObject>();

        private void Awake()
        {
            if (Instance != null && Instance != this)
                Destroy(gameObject);
            else
                Instance = this;
        }

        private void Start()
        {
            for (int i = 0; i < hp; i++)
            {
                Debug.Log(hpSymbols[i]);
                hpSymbols[i].GetComponent<Animator>().Play("HPAppear");
            }
        }

        public void ChangeHitpoints(int value)
        {
            if (hp == 6 && value > 0)
                return;
            
            hp += value;
            
            if(hp > -1)
                if (value > 0)
                    hpSymbols[hp-1].GetComponent<Animator>().Play("HPAppear");
                else
                    hpSymbols[hp].GetComponent<Animator>().Play("HPDisappear");

            CheckHP();
        }

        private void CheckHP()
        {
            if (hp < 0)
            {
                Debug.Log("Konec hry");
                Controllers.Game.Instance.GameOver();
            }
        }
    }
}
