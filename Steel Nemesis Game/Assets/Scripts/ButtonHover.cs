using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class ButtonHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{ 
    private TMP_Text buttonText;
    private Image buttonIcon;
    private Color selectedColor;
    public AudioClip hoverSound;
    private Animator anim;

    private bool hasAnimation = false;

    private void Start()
    {
        if (transform.Find("Text (TMP)"))
        {
            buttonText = transform.Find("Text (TMP)").GetComponent<TMP_Text>();
        }

        selectedColor = new Color(0.87f, 0.53f, 0.27f, 1);
        
        
        if(GetComponent<Animator>() != null)
        {
            anim = GetComponent<Animator>();
            hasAnimation = true;
        }
    }

    private void OnEnable()
    {
        ColorReset();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (hoverSound != null)
        {
        }
            

        if (buttonText != null)
            buttonText.color = selectedColor;
        
        
        if(hasAnimation)
            anim.SetBool("hover", true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {

        if(buttonText != null)
            buttonText.color = Color.white;
        
        if (buttonIcon != null)
            buttonIcon.color = Color.white;

        if(hasAnimation)
            anim.SetBool("hover", false);
    }

    public void ColorReset()
    {
        if(buttonText != null)
            buttonText.color = Color.white;
        
        if (buttonIcon != null)
            buttonIcon.color = Color.white;

        if (hasAnimation)
        {
            anim.SetBool("hover", false);
            if(buttonText != null)
                buttonText.rectTransform.localScale = new Vector3(1, 1, 1);
        }
    }
}
