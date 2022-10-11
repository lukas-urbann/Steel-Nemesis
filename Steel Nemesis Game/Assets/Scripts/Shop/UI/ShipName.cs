using TMPro;
using UnityEngine;

namespace Shop.UI
{
    public class ShipName : MonoBehaviour
    {
        private TMP_Text shipName;

        private void OnEnable()
        {
            shipName = GetComponent<TMP_Text>();
        }
        
        private void Start()
        {
            Controllers.Shop.Instance.onOpen += UpdateLabel;
            Player.Controller.StatsInstance.onShipUpgrade += UpdateLabel;
            
            UpdateLabel();
        }

        private void UpdateLabel()
        {
            shipName.text = Player.Controller.Instance.shipType.shipName;
        }
    }
}
