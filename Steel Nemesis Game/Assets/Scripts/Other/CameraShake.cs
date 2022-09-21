using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Other
{
    public class CameraShake : MonoBehaviour
    {
        public static CameraShake Instance;
        private Camera gameCamera;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
            }
            else
            {
                Instance = this;
            }
        }

        private void Start()
        {
            gameCamera = GetComponent<Camera>();
        }

        public void ShakeScreen(float time, float strength)
        {
            StartCoroutine(Shake(time, strength));
        }

        public IEnumerator Shake(float duration, float magnitude)
        {
            float elapsedTime = 0f;

            while (elapsedTime < duration)
            {
                float fovOffset = Random.Range(5f - magnitude, 5.35f);
                float rotOffset = Random.Range(-(2 + magnitude ), 2 + magnitude);

                gameCamera.orthographicSize = Mathf.Lerp(gameCamera.orthographicSize, fovOffset, 0.7f);
                transform.localRotation = Quaternion.Euler(0,0,Mathf.Lerp(transform.localRotation.z, rotOffset, 0.7f));
                
                elapsedTime += Time.deltaTime;
                
                yield return null;
            }
            gameCamera.orthographicSize = 5;
            transform.localRotation = Quaternion.Euler(0,0,0);
        }
    }
}