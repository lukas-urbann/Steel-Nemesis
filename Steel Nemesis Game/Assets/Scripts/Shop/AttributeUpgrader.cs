using System;
using TMPro;

namespace Shop.Attributes
{
    public class AttributeUpgrader : Attribute
    {
        public TMP_Text level;
        
        protected override void CustomButtonAction()
        {
            level.text = base.statLevel + "/" + base.maxStatLevel;
        }
    }
}