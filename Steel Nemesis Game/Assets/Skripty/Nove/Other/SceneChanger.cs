using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Other
{
    public class SceneChanger : MonoBehaviour
    {
        public void ChangeScene(string levelName)
        {
            SceneManager.LoadScene(levelName, LoadSceneMode.Single);
        }
    }
}
