#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InitGameDemo : MonoBehaviour
{
    private void Start()
    {
        //Init code here
        SceneManager.LoadScene("Demo1Scene");
    }
}
#endif