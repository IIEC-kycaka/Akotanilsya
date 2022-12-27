using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {
	public SceneReference GameScene;


	public void StartGame() {
		SceneManager.LoadScene(GameScene);
	}

	public void QuitGame() {
		#if UNITY_EDITOR
				UnityEditor.EditorApplication.ExitPlaymode();
		#else
				Application.Quit();
		#endif
	}
}
