using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Controllers
{
    
    
    public class Shop : MonoBehaviour
    {
        //Singleton
        public static Shop Instance;
        private float chanceToAppear = 0;
        private int wavesWithoutShop = 0;
        private int upgradeCost;
        
        private void Awake()
        {
            if (Instance != null && Instance != this)
                Destroy(gameObject);
            else
                Instance = this;
        }

        public void OpenShop()
        {
            Debug.Log("Shop Emerging...");
            chanceToAppear = 0;
            
            //void na call
        }

        public bool PostWaveShopCall()
        {
            IncreaseChance();
            float chance = Random.Range(0f, 100f);

            if (chance <= chanceToAppear)
            {
                OpenShop();
                return true;
            }

            return false;
        }

        private void IncreaseChance()
        {
            wavesWithoutShop++;
            
            if (chanceToAppear >= 100)
                chanceToAppear = 100;

            chanceToAppear += (wavesWithoutShop * 2);
        }

        private void CloseShop()
        {
            Wave.Instance.ResumeGame();
        }
    }
}