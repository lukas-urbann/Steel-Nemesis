using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

namespace Shop.Attributes
{
    public class AttributeUpgrader : Attribute
    {
        public TMP_Text level;
        public TMP_Text cost;
        public char[] charString;
        public string displayString;
        public int index = 0;
        public AudioClip letterType;

        protected override void CustomStart()
        {
            cost = PrefabHolder.Instance.costText;
        }
        
        protected override void CustomButtonAction()
        {
            level.text = statLevel + "/" + maxStatLevel;
        }

        protected override void OnCursorEnter()
        {
            TextReset();
            charString = (">> Item Cost: " + base.actualCost).ToCharArray();
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
        
        protected override void OnCursorExit()
        {
            TextReset();
        }

        private void TextReset()
        {
            charString = new char[0];
            displayString = "";
            cost.text = "";
        }
    }
}