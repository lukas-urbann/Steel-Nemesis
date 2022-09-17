using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Laser;

namespace Player
{
    public class Laser : BasicLaser
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
