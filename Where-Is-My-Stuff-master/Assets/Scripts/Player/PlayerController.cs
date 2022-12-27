using System;
using UnityEngine;

public class PlayerController : MonoBehaviour {
	private static readonly int speedXHash = Animator.StringToHash("speedX");
	private static readonly int speedYHash = Animator.StringToHash("speedY");

	public float speed = 10f;

	public Vector2 lastNonZeroMovement;

	private Rigidbody2D rb2d;
	private Animator animator;
	private PlayerAttack playerAttack;

	private Vector2 movement;

	private void Start() {
		rb2d = GetComponent<Rigidbody2D>();
		animator = GetComponent<Animator>();
		playerAttack = GetComponent<PlayerAttack>();
	}

	private void Update() {
		movement = new Vector2(Input.GetAxisRaw("Horizontal"),
		                       Input.GetAxisRaw("Vertical"));

		if (movement.x != 0 || movement.y != 0)
			lastNonZeroMovement = movement;

		if (!playerAttack.isAttacking)
			FlipObject(movement);

		animator.SetFloat(speedXHash, movement.x);
		animator.SetFloat(speedYHash, movement.y);
	}

	public void FlipObject(Vector2 newDirection) {
		Vector3 localScale = transform.localScale;
		if (newDirection.x > 0 || (newDirection.y != 0))
			localScale.x = 1;
		else if (newDirection.x < 0)
			localScale.x = -1;
		transform.localScale = localScale;
	}

	private void FixedUpdate() {
		Vector2 delta = movement.normalized * (speed * Time.deltaTime);
		rb2d.MovePosition(transform.position + (Vector3)delta);
	}
}
