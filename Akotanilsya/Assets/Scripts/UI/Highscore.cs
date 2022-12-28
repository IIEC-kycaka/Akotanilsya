using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Highscore : MonoBehaviour {
	public GameObject scoreOneContainer;
	public TextMeshProUGUI scoreOne;
	public GameObject scoreTwoContainer;
	public TextMeshProUGUI scoreTwo;
	public GameObject scoreThreeContainer;
	public TextMeshProUGUI scoreThree;

	public SceneReference menuScene;
	public SceneReference gameScene;

	private string[] scores;

	private void Start() {
		scores = PlayerPrefs.GetString("highscore", "0-0-0").Split('-');

		UpdateTextValues();
	}

	private void UpdateTextValues() {
		scoreOneContainer.SetActive(Int16.Parse(scores[0]) > 0);
		scoreOne.text = Int16.Parse(scores[0]) > 0 ? scores[0] : "";
        int best = Int16.Parse(scores[0]) > 0 ? Int16.Parse(scores[0]) : 0;
       // Debug.Log(best);
#if UNITY_WEBGL && !UNITY_EDITOR
    	WebGLPluginJS.SetLeder(best);
#endif

        scoreTwoContainer.SetActive(Int16.Parse(scores[1]) > 0);
		scoreTwo.text = Int16.Parse(scores[1]) > 0 ?scores[1] : "";

		scoreThreeContainer.SetActive(Int16.Parse(scores[2]) > 0);
		scoreThree.text = Int16.Parse(scores[2]) > 0 ? scores[2] : "";
	}

	public void ReturnToMenu() {
		SceneManager.LoadScene(menuScene);
	}

	public void Retry() {
		SceneManager.LoadScene(gameScene);
	}
}
