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
        private bool shopOpen = false;
        public GameObject shopWindow; //Dosadit z inspectoru
        private float chanceToAppear = 100;
        private int wavesWithoutShop = 0;

        public delegate void ShopDelegate();
        public ShopDelegate onClose;
        public ShopDelegate onOpen;

        private void Awake()
        {
            if (Instance != null && Instance != this)
                Destroy(gameObject);
            else
                Instance = this;
        }

        public bool SignalShop()
        {
            shopOpen = TryToOpenShop();
            
            IncreaseChance();
            return shopOpen;
        }

        public void OpenShop()
        {
            Debug.Log("Shop Emerging...");
            
            if(!shopWindow.activeSelf)
                shopWindow.SetActive(true);
            
            SetCursorVisible(true);
            //onOpen.Invoke();
            
            shopWindow.SetActive(true);
            ResetChanceValues();

            shopWindow.GetComponent<Animator>().Play("ShopAppear");
        }

        private void ResetChanceValues()
        {
            chanceToAppear = 0;
            wavesWithoutShop = 0;
        }

        public void CloseShop()
        {
            Debug.Log("Shop Disabling...");
            
            SetCursorVisible(false);
            onClose.Invoke();

            shopWindow.GetComponent<Animator>().Play("ShopDisappear");
        }

        private void SetCursorVisible(bool boolean)
        {
            Cursor.visible = boolean;
        }
        
        public bool TryToOpenShop()
        {
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

            chanceToAppear += (wavesWithoutShop * 1.4f);
        }
    }
}