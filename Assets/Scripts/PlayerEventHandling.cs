using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerEventHandling : MonoBehaviour {
	public Health health;
	public SceneReference gameOverScene;

	public SpriteRenderer spriteRenderer;
	public float flickerAlpha;
	public float flickerDuration = 0.5f;
	public float flickerInterval = 0.1f;

	private bool receivedDamage;

	private void Start() {
		health.onDeath.AddListener(HandleDeath);
		health.onDamageReceived.AddListener(HandleReceivedDamage);

		if (!PlayerPrefs.HasKey("highscore"))
			PlayerPrefs.SetString("highscore", "0-0-0");

		if (!PlayerPrefs.HasKey("lastscore"))
			PlayerPrefs.SetInt("lastscore", 0);
	}

	private void HandleReceivedDamage() {
		if (receivedDamage) return;

		StartCoroutine(CoHandleReceivedDamage());
	}

	private IEnumerator CoHandleReceivedDamage() {
		health.isInvulnerable = true;
		receivedDamage = true;
		bool isFlickerActive = false;
		for (float i = 0; i < flickerDuration; i += flickerInterval) {
			Color col = spriteRenderer.color;
			col.a = isFlickerActive ? 1 : flickerAlpha;
			spriteRenderer.color = col;

			isFlickerActive = !isFlickerActive;
			yield return new WaitForSeconds(flickerInterval);
		}

		Color color = spriteRenderer.color;
		color.a = 1;
		spriteRenderer.color = color;
		health.isInvulnerable = false;
		receivedDamage = false;
	}

	private void HandleDeath() {
		HandlePotentialHighscore();

		SceneManager.LoadScene(gameOverScene);
	}

	private void HandlePotentialHighscore() {
		string[] score = PlayerPrefs.GetString("highscore").Split('-');

		for (int i = 0; i < score.Length; i++)
			if (GameManager.Instance.TotalScore > int.Parse(score[i])) {
				if (i == 0)
					score[2] = score[1];
				if (i == 0 || i == 1)
					score[1] = score[0];

				score[i] = GameManager.Instance.TotalScore.ToString();
				break;
			}

		PlayerPrefs.SetString("highscore", $"{score[0]}-{score[1]}-{score[2]}");
		PlayerPrefs.SetInt("lastscore", GameManager.Instance.TotalScore);
	}
}
