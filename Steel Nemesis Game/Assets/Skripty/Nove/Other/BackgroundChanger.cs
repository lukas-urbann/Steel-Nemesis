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

        private void Start()
        {
            background.sprite = backgrounds[Random.Range(0, backgrounds.Count)];
            Controllers.Wave.Instance.onWaveEnd += () => CheckBackground();
        }

        public void OnEnable()
        {
            background = GetComponent<SpriteRenderer>();
        }

        private void CheckBackground()
        {
            int level = Mathf.FloorToInt(Controllers.Wave.Instance.GetLevel() / 20);

            if (level > backgrounds.Count)
            {
                background.sprite = backgrounds[backgrounds.Count-1];
            }
            else
            {
                background.sprite = backgrounds[level];
            }
        }
    }
}