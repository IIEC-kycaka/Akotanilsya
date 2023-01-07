using UnityEngine;

public class Auth : MonoBehaviour
{
    public GameObject authButton;
    public bool isAuth = false;

    public static Auth Instance;

    private void Awake()
    {
        //CheckAuth();
        
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            CheckAuth();
        }
        else
        {
            Destroy(gameObject);
        }
        
        /*
#if UNITY_WEBGL && !UNITY_EDITOR
          WebGLPluginJS.GetAuth();
#endif
        */
    }
    void Start()
    {
        authButton.SetActive(!isAuth);
        // CheckAuth();
    }
    private void Update()
    {
        /*
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
        } */
      //  CheckAuth();
    }

    public void Authorization()
    {
#if UNITY_WEBGL && !UNITY_EDITOR
    	WebGLPluginJS.SetAuth();
        CheckAuth();
#endif
        authButton.SetActive(!isAuth);
        /* if (isAuth)
         {
             PlayerPrefs.SetInt("isAuth", 1);
         }
         else
         {
             PlayerPrefs.SetInt("isAuth", 0);
         }
         */
        // CheckAuth();
    }

    public void GetDataAuthorization(bool value)
    {
       // isAuth = value;
        //CheckAuth();
    }

    public void CheckAuth()
    {
#if UNITY_WEBGL && !UNITY_EDITOR
        
        isAuth = WebGLPluginJS.GetAuth() == "yes" ? true : false;
#endif
        
    }



}
