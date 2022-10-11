using System;
using System.Collections.Generic;
using Controllers;
using Player;
using UnityEngine;
using UnityEngine.UI;

namespace Shop
{
    public class Attribute : MonoBehaviour
    {
        [SerializeField] protected ShipStats statType;
        [SerializeField] protected Player.StatOrigin statOrigin;
        [SerializeField] protected float increaseValue;
        private GameObject slots;
        protected List<Image> upgradeIndicators = new List<Image>();
        [SerializeField] protected Sprite takenIndicator, openIndicator;
        protected int statLevel = 0, maxStatLevel = 10;

        public Button plusButton, minusButton;

        private void Start()
        {
            slots = transform.Find("Slots").gameObject;
            
            foreach (Transform child in slots.transform)
                upgradeIndicators.Add(child.GetComponent<Image>());
            
            CheckLevel();
        }

        protected void DisableButton(bool isPositive)
        {
            switch (isPositive)
            {
                case true:
                    plusButton.interactable = false;
                    break;
                case false:
                    minusButton.interactable = false;
                    break;
            }
        }
        
        protected void EnableButton(bool isPositive)
        {
            switch (isPositive)
            {
                case true:
                    plusButton.interactable = true;
                    break;
                case false:
                    minusButton.interactable = true;
                    break;
            }
        }

        public void UpdateLabel()
        {
            
        }

        public void Increase()
        {
            switch (statOrigin)
            {
                case StatOrigin.Skillpoint:
                    break;
                case StatOrigin.Upgrade:
                    Player.Controller.StatsInstance.UpgradeShipStats(statType, statOrigin, increaseValue);
                    break;
                default:
                    Debug.LogError("Unknown Attribute Type");
                    break;
            }
            statLevel++;
            CheckLevel();
        }

        public void Decrease()
        {
            switch (statOrigin)
            {
                case StatOrigin.Skillpoint:
                    break;
                case StatOrigin.Upgrade:
                    Player.Controller.StatsInstance.UpgradeShipStats(statType, statOrigin, -increaseValue);
                    break;
                default:
                    Debug.LogError("Unknown Attribute Type");
                    break;
            }
            statLevel--;
            CheckLevel();
        }

        protected void CalculatePrice()
        {
            
        }

        protected void CheckLevel()
        {
            CustomButtonAction();
            
            EnableButton(true);
            EnableButton(false);
            
            if(statLevel <= 0)
                DisableButton(false);
            
            if(statLevel >= maxStatLevel)
                DisableButton(true);
                
            for (int i = 0; i < upgradeIndicators.Count; i++)
            {
                upgradeIndicators[i].sprite = openIndicator;
            }

            for (int i = 0; i < statLevel; i++)
            {
                upgradeIndicators[i].sprite = takenIndicator;
            }
        }

        protected virtual void CustomButtonAction() {}
    }
}