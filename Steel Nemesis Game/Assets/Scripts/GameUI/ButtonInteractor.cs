using Controllers;
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

        public Sprite buttonImageHover, buttonImageNormal; 
        
        [HideInInspector]
        public Color inactiveTextColor = Color.white, activeTextColor;
        
        public AudioClip hoverSound, clickSound;
        
        private void Start()
        {
            activeTextColor = new Color(0.86f, 0.86f, 0.86f, 1);
            
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

            if (buttonImage != null)
                buttonImage.sprite = buttonImageHover;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (buttonText != null)
                    buttonText.color = inactiveTextColor;
            
            if (buttonImage != null)
                buttonImage.sprite = buttonImageNormal;
        }

        public void ButtonReset()
        {
            if (buttonText != null)
                buttonText.color = inactiveTextColor;
            
            if (buttonImage != null)
                buttonImage.sprite = buttonImageNormal;
        }

        public void PlayClickSound()
        {
            Audio.Instance.PlaySound(clickSound);
        }
    }
}
