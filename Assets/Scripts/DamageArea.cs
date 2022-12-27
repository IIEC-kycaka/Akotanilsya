using UnityEngine;

public class DamageArea : MonoBehaviour {
	public int damage = 1;

	private void OnCollisionEnter2D(Collision2D other) {
		OnEnter(other.collider);
	}

	private void OnTriggerEnter2D(Collider2D other) {
		OnEnter(other);
	}

	private void OnEnter(Collider2D other) {
		Health health = other.gameObject.GetComponent<Health>();
		if (health != null) {
			health.Damage(damage);
		}
	}
}
