using UnityEngine;
using UnityEngine.UI;

public class PlayerUIHealth : MonoBehaviour {
	public Image filledHealthImage;

	private Health playerHealth;

	private void Start() {
		playerHealth = GameObject.FindWithTag("Player").GetComponent<Health>();
		playerHealth.onDamageReceived.AddListener(HandlePlayerDamage);
	}

	private void HandlePlayerDamage() {
		float newFillAmount = (float) playerHealth.currentHealth / playerHealth.maxHealth;
		filledHealthImage.fillAmount = newFillAmount;
	}
}
