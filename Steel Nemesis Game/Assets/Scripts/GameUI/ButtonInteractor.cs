using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

namespace GameUI
{
    public class ButtonInteractor : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        public bool changeTextColor = true;
        private TMP_Text buttonText;
        private Image buttonImage;
        
        [HideInInspector]
        public Color inactiveTextColor = Color.white, activeTextColor;
        
        public AudioClip hoverSound, clickSound;
        
        private void Start()
        {
            activeTextColor = new Color(0.3f, 0.8f, 1, 1);
            
            if (GetComponent<Image>() != null)
                buttonImage = GetComponent<Image>();

            if (GetComponentInChildren<TMP_Text>() != null)
                buttonText = GetComponentInChildren<TMP_Text>();
        }

        private void OnEnable()
        {
            ButtonReset();
        }

        public void SelectButton()
        {
            if (hoverSound != null)
                Controllers.Audio.Instance.PlaySound(clickSound);
            
            ButtonReset();
        }
        
        public void OnPointerEnter(PointerEventData eventData)
        {
            if (hoverSound != null)
                Controllers.Audio.Instance.PlaySound(hoverSound);

            if (buttonText != null)
                buttonText.color = activeTextColor;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (buttonText != null)
                    buttonText.color = inactiveTextColor;
        }

        public void ButtonReset()
        {
            if (buttonText != null)
                buttonText.color = inactiveTextColor;
        }
    }
}
