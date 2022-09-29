using Controllers;

namespace Shop.Attributes.Player
{
    public class Hitpoints : Attribute
    {
        private void OnEnable()
        {
            statLevel = 0;
            statType = ShipStats.HitPoints;
            attributeType = AttributeType.Player;
        }
    }
}