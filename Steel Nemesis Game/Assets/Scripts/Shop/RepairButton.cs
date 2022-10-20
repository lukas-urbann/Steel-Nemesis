using System;
using System.Collections;
using System.Collections.Generic;
using Controllers;
using Player;
using Shop.UI;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Shop
{
    public class RepairButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        private float repairCost;
        public AudioClip repairSound;
        
        private void Start()
        {
            Controllers.Shop.Instance.onOpen += CheckPlayerHp;
            CheckPlayerHp();
        }

        private void CheckPlayerHp()
        {
            if ((int)Player.Controller.Instance.GetHP() <
                (int)Player.Controller.StatsInstance.GetShipStats(ShipStats.MaxHitPoints))
                Appear();
            else
                gameObject.SetActive(false);
        }

        private void Appear()
        {
            gameObject.SetActive(true);
            repairCost = 0;
        }
        
        public void OnPointerEnter(PointerEventData eventData)
        {
            Shop.UI.CostTyper.Instance.TypeText(">> Repair cost: " + repairCost);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            Shop.UI.CostTyper.Instance.TextReset();
        }

        public void RepairPlayer()
        {
            if ((int)repairCost <= Controllers.Credit.Instance.GetCredit())
            {
                Controllers.Credit.Instance.AddCredit((int) -repairCost);
                Controllers.Audio.Instance.PlaySound(repairSound);
                
                Player.Controller.Instance.RepairPlayer();
                gameObject.SetActive(false);
                
                Shop.UI.CostTyper.Instance.TextReset();
                
                Player.Controller.StatsInstance.onShipUpgrade.Invoke();
            }
            else
            {
                Shop.UI.CostTyper.Instance.TypeText(">> Not enough credits");
            }
        }
    }
}
