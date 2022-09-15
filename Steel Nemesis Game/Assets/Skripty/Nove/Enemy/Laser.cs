using Laser;
using UnityEngine;

namespace Enemy
{
    public class Laser : BasicLaser
    {
        private float damage;

        public void SetDamage(float dmg)
        {
            damage = dmg;
        }
    }
}