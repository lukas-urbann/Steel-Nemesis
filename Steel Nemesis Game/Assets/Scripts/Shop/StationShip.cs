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
            AnimationPlayer("StationFlyBy");
            StartCoroutine(FlyInCoroutine(300));
        }
        
        private IEnumerator FlyInCoroutine(int miliseconds)
        {
            yield return new WaitForSeconds(miliseconds / 60);
            StartCoroutine(StayCoroutine());
        }

        private IEnumerator StayCoroutine()
        {
            TradeZoneSwitcher();
            yield return new WaitForSeconds(3);
            TradeZoneSwitcher();
            PrepareShop();
        }

        private void PrepareShop()
        {
            if(playerInShop)
                Controllers.Shop.Instance.OpenShop();
            else
            {
                FlyOut();
                Controllers.Wave.Instance.SignalWaveStart();
            }
        }

        private void AnimationPlayer(string animationName)
        {
            anim.Play(animationName);
        }

        private void TradeZoneSwitcher()
        {
            tradingSpotCircle.SetActive(!tradingSpotCircle.activeSelf);
        }
        
        private void FlyOut()
        {
            anim.PlayInFixedTime("StationLeave");
            playerInShop = false;
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