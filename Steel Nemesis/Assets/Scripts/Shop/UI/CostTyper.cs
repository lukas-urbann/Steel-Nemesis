using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Shop.UI
{
    public class CostTyper : MonoBehaviour
    {
        private TMP_Text cost;
        private char[] charString;
        private string displayString;
        private int index = 0;
        public AudioClip letterType;
        public static CostTyper Instance;

        private void Awake()
        {
            if (Instance != this && Instance != null)
                Destroy(gameObject);
            else
                Instance = this;
        }

        public void SetCharString(char[] toProcess)
        {
            charString = toProcess;
        }

        private void Start()
        {
            cost = GetComponent<TMP_Text>();
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

        public void TypeText(string toProcess)
        {
            TextReset();
            SetCharString(toProcess.ToCharArray());
            StartCoroutine(SlowTyping());
        }
        
        public void TextReset()
        {
            charString = new char[0];
            displayString = "";
            cost.text = "";
        }
    }
}

