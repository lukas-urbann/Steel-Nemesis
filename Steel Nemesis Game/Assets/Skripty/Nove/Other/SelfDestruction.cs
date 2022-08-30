using UnityEngine;

namespace Other
{
    public class SelfDestruction : MonoBehaviour
    {
        public void DestroyGameObject()
        {
            Destroy(gameObject);
        }
    }
}
