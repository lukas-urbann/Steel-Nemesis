using System;
using System.Collections;
using System.Collections.Generic;
using Controllers;
using TMPro;
using UnityEngine;

namespace GameUI
{
    public class Slideshow : MonoBehaviour
    {
        [SerializeField] private List<GameObject> slides = new List<GameObject>();
        public GameObject arrowRight, arrowLeft;
        private int slideNumber;
        public TMP_Text slideText;

        private void Start()
        {
            CheckArrows();
            OverwriteSlideNumber();
        }

        private void OverwriteSlideNumber()
        {
            if (slideText.gameObject.activeSelf)
            {
                slideText.text = (GetActiveSlide()+1) + "/" + slides.Count;
            }
        }

        private int GetActiveSlide()
        {
            foreach (GameObject slide in slides)
                if (slide.activeSelf)
                    return slides.IndexOf(slide);

            return 0;
        }

        private void CheckArrows()
        {
            if(GetActiveSlide() < slides.Count-1)
                arrowRight.SetActive(true);
            else
                arrowRight.SetActive(false);
            
            if(GetActiveSlide() > 0)
                arrowLeft.SetActive(true);
            else
                arrowLeft.SetActive(false);
        }

        private void SwitchToNextSlide()
        {
            int slide = GetActiveSlide();
            slides[slide+1].SetActive(true);
            slides[slide].SetActive(false);
            
            CheckArrows();
            OverwriteSlideNumber();
        }
        
        private void SwitchToPreviousSlide()
        {
            int slide = GetActiveSlide();
            slides[slide-1].SetActive(true);
            slides[slide].SetActive(false);
            
            CheckArrows();
            OverwriteSlideNumber();
        }
        
        public void NextSlide()
        {
            SwitchToNextSlide();
        }

        public void PreviousSlide()
        {
            SwitchToPreviousSlide();
        }
    }
}
