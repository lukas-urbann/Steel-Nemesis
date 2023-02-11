using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

    public class SceneTransition : MonoBehaviour
    {
        public static SceneTransition Instance;
        public GameObject loadingScreen;
        
        private void Awake()
        {
            if (Instance != null && Instance != this) 
                Destroy(gameObject); 
            else 
                Instance = this; 
        }

        public void LoadLevel(string levelName)
        {
            loadingScreen.SetActive(true);
            StartCoroutine(LoadAsynchronously(levelName));
        }
    
        private IEnumerator LoadAsynchronously(string levelName)
        {
            Time.timeScale = 1;
            AsyncOperation operation = SceneManager.LoadSceneAsync(levelName, LoadSceneMode.Single);
            
            while (!operation.isDone)
            {
                yield return null;
                Debug.Log("Level Transitioned");
            }
        }
    }
