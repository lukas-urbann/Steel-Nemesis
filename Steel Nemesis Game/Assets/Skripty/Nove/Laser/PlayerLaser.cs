using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Laser
{
    public class PlayerLaser : BasicLaser
    {
        private float damage;

        public void SetDamage(float dmg)
        {
            damage = dmg;
        }

        private float GetDamage()
        {
            return damage;
        }
    }
}
