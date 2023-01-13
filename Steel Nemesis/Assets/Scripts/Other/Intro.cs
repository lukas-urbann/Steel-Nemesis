using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Other
{
    public class Intro : MonoBehaviour
    {
        private void Start()
        {
            StartCoroutine(SkipIntro());
        }

        private IEnumerator SkipIntro()
        {
            yield return new WaitForSeconds(4);
            GetComponent<SceneChanger>().ChangeScene("Menu");
        }
    }
}

