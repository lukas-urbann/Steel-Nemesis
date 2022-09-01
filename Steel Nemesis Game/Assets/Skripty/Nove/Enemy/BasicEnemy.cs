using System;
using System.Collections;
using System.Collections.Generic;
using Other;
using Triggers;
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
        protected float hp;

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

        private void OnCollisionEnter2D(Collision2D col)
        {
            if (col.gameObject.CompareTag("Laser") && col.gameObject.layer == 7)
            {
                Debug.Log("laser hit");
                Destroy(col.gameObject);
            }
        }

        private void OnTriggerEnter2D(Collider2D col)
        {
            if (col.CompareTag("Trigger"))
            {
                TypeOfTrigger trigger;
                
                try
                {
                    trigger = col.GetComponent<TriggerType>().triggerType;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }

                switch (trigger)
                {
                    case TypeOfTrigger.border:
                        switch (col.gameObject.layer)
                        {
                            case 3:
                                Debug.Log("Konec hry");
                                Controllers.Game.Instance.GameOver();
                                break;
                            case 10:
                                GetComponent<Boundaries>().enabled = false;
                                break;
                        }
                        break;
                }
            }
        }
    }
}
