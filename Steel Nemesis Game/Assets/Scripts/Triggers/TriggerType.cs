using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Triggers
{
    public enum TypeOfTrigger {
        laser,
        border,
        ship
    }
    
    public class TriggerType : MonoBehaviour
    {
        public TypeOfTrigger triggerType;
    }
}
