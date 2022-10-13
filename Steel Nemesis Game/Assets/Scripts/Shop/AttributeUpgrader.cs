using TMPro;

namespace Shop.Attributes
{
    public class AttributeUpgrader : Attribute
    {
        public TMP_Text level;
        
        protected override void CustomButtonAction()
        {
            level.text = statLevel + "/" + maxStatLevel;
        }
    }
}