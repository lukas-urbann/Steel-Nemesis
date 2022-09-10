using UnityEngine;

namespace Enemy
{
    public class Laser : MonoBehaviour
    {
        private float damage;

        public void SetDamage(float dmg)
        {
            damage = dmg;
        }
    }
}