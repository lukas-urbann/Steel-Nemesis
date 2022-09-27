using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Controllers
{
    
    
    public class Shop : MonoBehaviour
    {
        /*
        //Singleton
        public static Shop Instance;
        public GameObject shopWindow; //Dosadit z inspectoru
        private float chanceToAppear = 50;
        private int wavesWithoutShop = 0;
        private int upgradeCost;

        public List<GameObject> shopBreakObjects = new List<GameObject>();

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
            shopWindow.SetActive(true);
            chanceToAppear = 0;

            //Wave.Instance.SetBreak(true);
            shopWindow.GetComponent<Animator>().Play("ShopAppear");
            
            PostShopOpen();
        }

        public void CloseShop()
        {
            Debug.Log("Shop Disabling...");

            //Wave.Instance.SetBreak(false);
            shopWindow.GetComponent<Animator>().Play("ShopDisappear");
            
            PostShopClose();
        }

        private void PostShopOpen()
        {
            Cursor.visible = true;

            foreach (GameObject obj in shopBreakObjects)
            {
                if (obj == null)
                    continue;
                
                if(obj.activeSelf)
                    obj.SetActive(false);
            }
        }

        private void PostShopClose()
        {
            Cursor.visible = false;
            
            foreach (GameObject obj in shopBreakObjects)
            {
                if(obj != null)
                    obj.SetActive(true);
            }
            
            Player.Controller.Instance.PostShopAction();
            
            Wave.Instance.ResumeFromShop();
            Wave.Instance.ResumeGame();
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
        */
    }
}