using System;
using UnityEngine;

namespace Enemy
{
    public class Fighter : BasicEnemy
    {
        private Rigidbody2D rb;
       
        private void Start()
        {
            dropValues = new int[] { 0, 0, 1, 0, 0};
            
            rb = GetComponent<Rigidbody2D>();
            hp = 70 + (Controllers.Wave.Instance.GetLevel() * 1f);

            if(speed < 3.5f)
                speed = 2f + (Controllers.Wave.Instance.GetLevel() * 0.005f);

            InitMaxHp();
            StartCoroutine(ChangeDirections(2f));
        }

        private void Update()
        {
            Fly();

            if (visible)
            {
                Shoot(1.5f); 
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