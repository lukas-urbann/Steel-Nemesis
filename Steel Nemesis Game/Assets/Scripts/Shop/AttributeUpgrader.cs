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
        
        protected override void CustomButtonAction()
        {
            level.text = statLevel + "/" + maxStatLevel;
        }

        protected override void OnCursorEnter()
        {
            Shop.UI.CostTyper.Instance.TypeText(">> Item cost: " + actualCost);
        }

        protected override void OnCursorExit()
        {
            Shop.UI.CostTyper.Instance.TextReset();
        }
    }
}