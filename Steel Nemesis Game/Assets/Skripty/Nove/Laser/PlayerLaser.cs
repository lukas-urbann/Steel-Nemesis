using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Laser
{
    public class PlayerLaser : BasicLaser
    {
        private int damage;

        public void SetDamage(int dmg)
        {
            damage = dmg;
        }

        private int GetDamage()
        {
            return damage;
        }
    }
}
