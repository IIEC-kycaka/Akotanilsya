using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class Auth : MonoBehaviour
{
    public GameObject authButton;
    public bool isAuth = false;

    public static Auth Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
           // DontDestroyOnLoad(gameObject);
            CheckAuth();
        }
        else
        {
            Destroy(gameObject);
        }
    }
    void Start()
    {
        authButton.SetActive(!isAuth);
        
    }
    public void Authorization()
    {
#if UNITY_WEBGL && !UNITY_EDITOR
    	CheckAuth();
        if (!isAuth)
        {
            WebGLPluginJS.SetAuth();
            StartCoroutine("Wait");
        }  
#endif
        authButton.GetComponent<Button>().interactable = false;
    }
    public void CheckAuth()
    {
#if UNITY_WEBGL && !UNITY_EDITOR
        
        isAuth = WebGLPluginJS.GetAuth() == "yes" ? true : false;
#endif   
    }
    IEnumerator Wait()
    {
        yield return new WaitForSeconds(3f);
        CheckAuth();
        authButton.SetActive(!isAuth);
        authButton.GetComponent<Button>().interactable = true;
    }
}
