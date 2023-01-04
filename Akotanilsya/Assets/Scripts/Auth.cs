using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Auth : MonoBehaviour
{
    public GameObject authButton;
    public bool isAuth = false; 
    void Start()
    {
        CheckAuth();
    }

    public void Authorization()
    {
#if UNITY_WEBGL && !UNITY_EDITOR
    	WebGLPluginJS.SetAuth();
#endif
    }
    
    public void GetDataAuthorization(bool value)
    {
        isAuth = value;
        CheckAuth();
    }

    public void CheckAuth()
    {
        if (isAuth)
        {
            authButton.SetActive(false);
        }
        else
        {
            authButton.SetActive(true);
        }
    }



}
