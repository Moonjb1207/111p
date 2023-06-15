using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingScene : MonoBehaviour
{
    float curTime = 0.0f;
    float LoadingTime = 3.0f;

    private void Start()
    {
        
    }

    private void Update()
    {
        curTime += Time.deltaTime;
        if (curTime > LoadingTime)
        {
            SceneManager.LoadScene(2);
        }
    }
}
