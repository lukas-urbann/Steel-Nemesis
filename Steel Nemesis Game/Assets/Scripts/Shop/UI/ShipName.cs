using TMPro;
using UnityEngine;

namespace Shop.UI
{
    public class ShipName : MonoBehaviour
    {
        private TMP_Text shipName;

        private void Start()
        {
            shipName = GetComponent<TMP_Text>();
            Controllers.Shop.Instance.onOpen += UpdateLabel;
        }

        private void UpdateLabel()
        {
            shipName.text = Player.Controller.Instance.shipType.shipName;
        }
    }
}
