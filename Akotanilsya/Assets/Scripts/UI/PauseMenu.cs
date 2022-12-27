using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour {
	public GameObject pauseMenu;
	public SceneReference mainmenuScene;

	private bool isPaused;

	private void Start() {
		pauseMenu.SetActive(false);
	}

	private void Update() {
		if (Input.GetButtonDown("Cancel")) {
			if (isPaused)
				Unpause();
			else
				Pause();
		}
	}

	public void Pause() {
		if (isPaused) return;

		Time.timeScale = 0;
		isPaused = true;
		pauseMenu.SetActive(true);
	}

	public void Unpause() {
		if (!isPaused) return;

		Time.timeScale = 1;
		isPaused = false;
		pauseMenu.SetActive(false);
	}

	public void ReturnToMenu() {
		Time.timeScale = 1;
		SceneManager.LoadScene(mainmenuScene);
	}
}
