using System;
using System.Collections.Generic;
using Controllers;
using Player;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Shop
{
    public class Attribute : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] protected ShipStats statType;
        [SerializeField] protected StatOrigin statOrigin;
        [SerializeField] protected float actualCost, baseCost;
        [SerializeField] protected float increaseValue;
        private GameObject slots;
        protected List<Image> upgradeIndicators = new List<Image>();
        [SerializeField] protected Sprite takenIndicator, openIndicator;
        protected float statLevel = 0, maxStatLevel = 1;

        public Button plusButton, minusButton;

        private void Start()
        {
            slots = transform.Find("Slots").gameObject;
            AssignMaxShipStats();
            
            foreach (Transform child in slots.transform)
                upgradeIndicators.Add(child.GetComponent<Image>());
            
            CheckLevel();
            CalculatePrice();
            CustomStart();
        }

        protected virtual void CustomStart() { }

        private void AssignMaxShipStats()
        {
            for (int i = 0; i < Player.Controller.StatsInstance.shipStatsList.Count; i++)
                if(statType == Player.Controller.StatsInstance.shipStatsList[i])
                    maxStatLevel = Mathf.FloorToInt(Player.Controller.StatsInstance.GetShipStats(statType, StatOrigin.MaxStat));
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
            actualCost = ((baseCost * (Wave.Instance.GetLevel() * 0.03f)) * statLevel) * Player.Controller.Instance.shipType.costMultiplier;
        }

        protected void CheckLevel()
        {
            CustomButtonAction();
            CalculatePrice();
            
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


            float points = Player.Controller.StatsInstance.GetShipStats(statType, StatOrigin.Upgrade) /
                           ((maxStatLevel * increaseValue) / 100);
            
            for (int i = 0; i < points; i += 10)
                upgradeIndicators[i/10].sprite = takenIndicator;
        }

        protected virtual void CustomButtonAction() {}
        
        public void OnPointerEnter(PointerEventData eventData)
        {
            OnCursorEnter();
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            OnCursorExit();
        }
        
        protected virtual void OnCursorEnter() {}
        protected virtual void OnCursorExit() {}
    }
}