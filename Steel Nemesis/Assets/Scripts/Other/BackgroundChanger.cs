using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Other
{
    public class BackgroundChanger : MonoBehaviour
    {
        public List<Sprite> backgrounds = new List<Sprite>();
        private SpriteRenderer background;
        private Animator anim;

        private void Start()
        {
            Controllers.Wave.Instance.onWaveStart += StartBackgroundChangeAnimation;
            anim = GetComponent<Animator>();
            //background.sprite = backgrounds[Random.Range(0, backgrounds.Count)]; wtf
            //Controllers.Wave.Instance.onWaveStart += () => StartBackgroundChangeAnimation(); // Checkne jestli je pozadí správné každý konec kola
        }

        public void OnEnable()
        {
            background = GetComponent<SpriteRenderer>();
        }

        private void CheckBackground()
        {
            Debug.Log("Změna pozadí");
            int level = Mathf.FloorToInt(Controllers.Wave.Instance.GetLevel() / 5);

            if (level > backgrounds.Count)
            {
                background.sprite = backgrounds[backgrounds.Count-1];
            }
            else
            {
                background.sprite = backgrounds[level];
            }
        }

        public void StartBackgroundChangeAnimation()
        {
            if(Controllers.Wave.Instance.GetLevel() % 5 == 0)
                anim.Play("BackgroundChange");
        }
    }
}