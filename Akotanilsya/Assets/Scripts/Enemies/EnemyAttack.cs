using System.Collections;
using UnityEngine;

public class EnemyAttack : MonoBehaviour {
	public Transform hand;
	public GameObject weapon;
	public float attackDuration = 1f;
	public float attackCooldown = 1f;
	public bool isAttacking;

	[Space]
	public AudioSource attackSound;

	private Animator animator;
	private float lastAttackStartTime;

	private void Start() {
		animator = GetComponent<Animator>();

		weapon.SetActive(false);
	}

	public void Attack(Vector2 direction) {
		if (isAttacking || Time.time - lastAttackStartTime < attackDuration + attackCooldown) return;
		isAttacking = true;
		lastAttackStartTime = Time.time;

		float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

		Direction dir = GetDirectionFromAngle(angle);
		Vector2 dirVec;

		Vector3 localScale = transform.localScale;
		if (dir == Direction.Right)
			localScale.x = -1;
		else
			localScale.x = 1;
		transform.localScale = localScale;

		if (dir == Direction.Right)
			dirVec = dir.Inverted().ToVector();
		else
			dirVec = dir.ToVector();

		angle = Mathf.Atan2(dirVec.y, dirVec.x) * Mathf.Rad2Deg;
		hand.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

		StartCoroutine(CoAttack(dirVec));
	}

	private Direction GetDirectionFromAngle(float angle) {
		if (angle > -135 && angle <= -45)
			return Direction.Down;
		if (angle > -45 && angle <= 45)
			return Direction.Right;
		if (angle > 45 && angle <= 135)
			return Direction.Up;

		return Direction.Left;
	}

	private IEnumerator CoAttack(Vector2 dirVec) {
		weapon.SetActive(true);
		HandleAttackAnimation(dirVec);
		attackSound.Play();
		yield return new WaitForSeconds(attackDuration);
		weapon.SetActive(false);
		isAttacking = false;
	}

	private void HandleAttackAnimation(Vector2 direction) {
		Vector2 dir = direction;

		if (dir.y == 0) {
			if (dir.x != 0) animator.Play("Attack - Side");
		}
		else {
			animator.Play(dir.y > 0 ? "Attack - Up" : "Attack - Down");
		}
	}
}
