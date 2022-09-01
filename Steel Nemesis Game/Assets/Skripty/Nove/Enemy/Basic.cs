using System;
using System.Collections;
using System.Collections.Generic;
using Laser;
using UnityEngine;

namespace Enemy
{
    public class Basic : BasicEnemy
    {
        private Vector3 move;
        

        private void Start()
        {
            hp = 50;
            move = new Vector3(0, -4 * (Controllers.Wave.Instance.GetLevel() * 0.1f), 0);
            flyDirection = FlyDirection.Down;
        }

        private void Update()
        {
            switch (flyDirection)
            {
                case FlyDirection.Down:
                    transform.position += move * (Time.deltaTime * 2);
                    break;
            }
        }
        
        
    }
}
