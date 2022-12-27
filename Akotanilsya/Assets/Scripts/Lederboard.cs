using UnityEngine;


public class Lederboard : MonoBehaviour
{
    
    void Start()
    {
        SetHighScoreOnLederboard();

    }

    public void SetHighScoreOnLederboard()
    {
        
        int best = PlayerPrefs.GetInt("HighScore");
#if UNITY_WEBGL && !UNITY_EDITOR
    	WebGLPluginJS.SetLeder(best);
#endif
    }

}
