using TMPro;
using UnityEngine;

namespace Shop.UI
{
    public class CostValue : MonoBehaviour
    {
        private TMP_Text costLabel;
        
        public delegate void ItemCost(int amount);
        public ItemCost onItemHover;

        private void Start()
        {
            costLabel = GetComponent<TMP_Text>();
            onItemHover = UpdateLabel;
        }
        
        private void UpdateLabel(int amount = 0)
        {
            if (amount > 0)
                costLabel.text = "Upgrade cost: " + amount + " credits.";
            else
                costLabel.text = "You will gain: " + amount + " credits.";
        }
    }
}
