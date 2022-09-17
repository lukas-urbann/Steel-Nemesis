using System;
using UnityEngine;

namespace Enemy
{
    public class Fighter : BasicEnemy
    {
        private Rigidbody2D rb;
       
        private void Start()
        {
            rb = GetComponent<Rigidbody2D>();
            hp = 70 + (Controllers.Wave.Instance.GetLevel() * 1f);

            if(speed < 3.5f)
                speed = 0.75f + (Controllers.Wave.Instance.GetLevel() * 0.005f);

            InitMaxHP();
            StartCoroutine(ChangeDirections(2f));
        }

        private void Update()
        {
            Fly();

            if (visible)
            {
                Shoot(6); 
                FacePlayer(5);
            }
            else
            {
                FaceDown(5);
                flyDirection = FlyDirection.Down;
                ChangeDirection(flyDirection);
            }
            
            if(hp < maxHp / 4)
                ChangeDirectionRandomly();
        }
    }
}