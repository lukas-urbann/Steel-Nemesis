using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Enemy
{
    public enum EnemyType
    {
        Basic,
        Attacker,
        Tank,
        Fighter,
        Kamikadze,
        Mothership
    }
    
    public enum FlyDirection
    {
        Top,
        Right,
        Down,
        Left
    }

   public class BasicEnemy : MonoBehaviour
    {
        protected float speed;

        protected FlyDirection flyDirection;

        [SerializeField] protected GameObject laser;
        [SerializeField] protected List<Transform> barrelPoints = new List<Transform>();

        protected void SetSpeed(float spd)
        {
            speed = spd;
        }

        protected void ChangeDirection(FlyDirection side)
        {
            
        } 

        protected void Fire()
        {
            
        }

        private void OnBecameInvisible()
        {
            
        }
    }
}
