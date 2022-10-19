using System;
using System.Collections;
using System.Collections.Generic;
using Controllers;
using Player;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Shop
{
    public class RepairButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        public char[] charString;
        public string displayString;
        public int index = 0;
        public AudioClip letterType;
        public TMP_Text cost;
        private float repairCost;
        public AudioClip repairSound;
        
        private void Start()
        {
            Controllers.Shop.Instance.onOpen += CheckPlayerHp;
            cost = PrefabHolder.Instance.costText;
            
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
            repairCost = 2;
        }
        
        public void OnPointerEnter(PointerEventData eventData)
        {
            TextReset();
            charString = (">> Repair Cost: " + repairCost).ToCharArray();
            StartCoroutine(SlowTyping());
        }

        private IEnumerator SlowTyping()
        {
            float timer = 0.025f;
            index = 0;

            while (charString.Length != displayString.ToCharArray().Length)
            {
                timer -= 1 * Time.deltaTime;

                if (timer <= 0)
                {
                    displayString += charString[index];
                    index++;
                    timer = 0.025f;
                    Controllers.Audio.Instance.PlaySound(letterType);
                }
                cost.text = displayString;
                yield return null;
            }
            yield return null;
            //Sem nepsat, už se nespustí
        }
        
        public void OnPointerExit(PointerEventData eventData)
        {
            TextReset();
        }

        private void TextReset()
        {
            charString = new char[0];
            displayString = "";
            cost.text = "";
        }

        public void RepairPlayer()
        {
            if ((int)repairCost <= Controllers.Credit.Instance.GetCredit())
            {
                Controllers.Credit.Instance.AddCredit((int) -repairCost);
                Controllers.Audio.Instance.PlaySound(repairSound);
                
                Player.Controller.Instance.RepairPlayer();
                gameObject.SetActive(false);
            }
        }
    }
}
