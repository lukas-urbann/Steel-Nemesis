using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Enemy
{
    public class Tank : Enemy
    {
        private bool attacking = false;

        protected override void EarlyStart()
        {
            
        }

        protected override void AssignScriptShipValues()
        {
            SetDropValues(1,1,2,2,2);
            hp = defaultShipValues ? 120 : (120 + (Controllers.Wave.Instance.GetLevel() * 2f));
            speed = defaultShipValues ? 1 : (1f + (Controllers.Wave.Instance.GetLevel() * 0.005f));
            damage = 5;
        }

        private void Start()
        {
            StartCoroutine(AttackCooldown());
            flyDirection = FlyDirection.Down;
            ChangeDirection(flyDirection);
        }

        private void Update()
        {
            Fly();

            if (attacking && visible)
            {
                FacePlayer(2f);
                Shoot(2f);
            }
            else
            {
                flyDirection = FlyDirection.Down;
                ChangeDirection(flyDirection);
                FaceDown(2);
            }
        }

        private IEnumerator Attack()
        {
            yield return new WaitForSeconds(Random.Range(1,3));
            attacking = true;
            StartCoroutine(AttackCooldown());
        }

        private IEnumerator AttackCooldown()
        {
            yield return new WaitForSeconds(Random.Range(3, 5));
            attacking = false;
            StartCoroutine(Attack());
        }
    }
}