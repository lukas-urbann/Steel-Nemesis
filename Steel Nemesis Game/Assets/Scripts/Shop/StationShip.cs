using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace Shop
{
    public class StationShip : MonoBehaviour
    {
        private Animator anim;
        private bool playerInShop = false;
        public GameObject tradingSpotCircle; // Dosadit z inspectoru
        
        private void Start()
        {
            anim = GetComponent<Animator>();
            Controllers.Shop.Instance.onClose += FlyOut;
        }

        public void FlyInCall()
        {
            FlyIn();
        }

        private void FlyIn()
        {
            anim.Play("StationFlyBy");
            StartCoroutine(FlyBy());
        }
        
        private IEnumerator FlyBy()
        {
            tradingSpotCircle.SetActive(true);
            yield return new WaitForSeconds(12);
            PrepareShop();
        }

        private void PrepareShop()
        {
            if(playerInShop)
                Controllers.Shop.Instance.OpenShop();
            else
                FlyOut();
            
            tradingSpotCircle.SetActive(false);
        }
        
        private void FlyOut()
        {
            Debug.Log("Shop Station off");
            anim.PlayInFixedTime("StationLeave");
        }
        
        private void OnTriggerEnter2D(Collider2D col)
        {
            if (col.gameObject == Player.Controller.Instance.gameObject)
                playerInShop = true;
        }

        private void OnTriggerExit2D(Collider2D col)
        {
            if (col.gameObject == Player.Controller.Instance.gameObject)
                playerInShop = false;
        }
    }
}