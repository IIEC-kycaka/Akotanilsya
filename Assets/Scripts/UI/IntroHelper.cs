using UnityEngine;
using UnityEngine.SceneManagement;

public class IntroHelper : MonoBehaviour {
	public SceneReference menuScene;

	private void HandleIntroEnd() {
		SceneManager.LoadScene(menuScene);
	}
}
