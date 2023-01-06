using UnityEngine;

public class Auth : MonoBehaviour
{
    public GameObject authButton;
    public bool isAuth = false;

    //public static Auth Instance;

    private void Awake()
    {/*
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
#if UNITY_WEBGL && !UNITY_EDITOR
            isAuth = WebGLPluginJS.GetAuth();
#endif
            CheckAuth();
        }
        else
        {
            Destroy(gameObject);
        }
        */

#if UNITY_WEBGL && !UNITY_EDITOR
            isAuth = WebGLPluginJS.GetAuth();
#endif
        
    }
    void Start()
    {
        CheckAuth();
    }
    private void Update()
    {
        if (PlayerPrefs.HasKey("isAuth"))
        {
            if (PlayerPrefs.GetInt("isAuth") == 1)
            {
                isAuth = true;
            }
            else
            {
                isAuth = false;
            }
        }
        CheckAuth();
    }

    public void Authorization()
    {
#if UNITY_WEBGL && !UNITY_EDITOR
    	WebGLPluginJS.SetAuth();
        isAuth = WebGLPluginJS.GetAuth();
#endif
        if (isAuth)
        {
            PlayerPrefs.SetInt("isAuth", 1);
        }
        else
        {
            PlayerPrefs.SetInt("isAuth", 0);
        }
        CheckAuth();
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
