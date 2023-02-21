using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Crosshair
{
    public class Hitmark : MonoBehaviour
    {
        [SerializeField] private Image hitmarker;
        [SerializeField] private float appearTime = 0.1f;
        public AudioClip hitSnd;

        private IEnumerator Appear()
        {
            Controllers.Audio.Instance.PlaySound(hitSnd);
            hitmarker.enabled = true;
            yield return new WaitForSeconds(appearTime);
            hitmarker.enabled = false;
        }
    }
}
