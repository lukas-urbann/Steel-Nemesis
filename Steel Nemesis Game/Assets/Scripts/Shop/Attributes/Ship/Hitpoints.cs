using System;
using Controllers;

namespace Shop.Attributes.Ship
{
    public class Hitpoints : Attribute
    {
        private void OnEnable()
        {
            statLevel = 0;
            statType = ShipStats.HitPoints;
            attributeType = AttributeType.Ship;
        }
    }
}