using System.Collections;
using UnityEngine;

public class PlayerAttack : MonoBehaviour {
	public Transform hand;
	public GameObject weapon;
	public AudioSource attackSound;
	public float attackDuration = 1f;
	public float attackCooldown = 0.5f;
	public bool isAttacking;

	private Camera mainCamera;
	private PlayerController playerController;
	private Animator animator;
	private SpriteRenderer sweepSpriteRenderer;

	private void Start() {
		mainCamera = Camera.main;
		playerController = GetComponent<PlayerController>();
		animator = GetComponent<Animator>();
		sweepSpriteRenderer = weapon.GetComponent<SpriteRenderer>();

		weapon.SetActive(false);
	}

	private void Update() {
		if (!isAttacking && Input.GetButton("Fire1")) {
			isAttacking = true;

			Vector3 mouseDirection = mainCamera.ScreenToWorldPoint(Input.mousePosition) - transform.position;
			float angle = Mathf.Atan2(mouseDirection.y, mouseDirection.x) * Mathf.Rad2Deg;

			Direction direction = GetDirectionFromAngle(angle);
			Vector2 dirVec = direction.ToVector();

			playerController.FlipObject(dirVec);

			if (direction == Direction.Left) {
				dirVec = direction.Inverted().ToVector();
				sweepSpriteRenderer.flipY = true;
			}
			else {
				dirVec = direction.ToVector();
				if (direction == Direction.Right)
					sweepSpriteRenderer.flipY = true;
				else
					sweepSpriteRenderer.flipY = false;
			}

			angle = Mathf.Atan2(dirVec.y, dirVec.x) * Mathf.Rad2Deg;
			hand.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

			StartCoroutine(CoAttack(dirVec));
		}
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

	private IEnumerator CoAttack(Vector2 direction) {
		weapon.SetActive(true);
		HandleAttackAnimation(direction);

		attackSound.Play();

		yield return new WaitForSeconds(attackDuration);

		weapon.SetActive(false);

		yield return new WaitForSeconds(attackCooldown);

		isAttacking = false;
	}

	private void HandleAttackAnimation(Vector2 direction) {
		Vector2 dir = direction;

		if (dir.y == 0) {
			if (dir.x != 0) {
				animator.Play("Attack - Side");
			}
		}
		else {
			animator.Play(dir.y > 0 ? "Attack - Up" : "Attack - Down");
		}
	}
}
