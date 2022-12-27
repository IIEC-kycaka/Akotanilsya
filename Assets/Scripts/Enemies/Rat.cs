using DG.Tweening;
using UnityEngine;

public class Rat : MonoBehaviour {
	private static readonly int speedYRatioHash = Animator.StringToHash("speedYRatio");
	private static readonly int speedXRatioHash = Animator.StringToHash("speedXRatio");

	public Transform backpack;
	public GameObject weapon;
	public Collider2D navigationCollider;
	public GameObject deathEffectPrefab;
	public float deathFadeDuration = 1f;

	private Animator animator;
	private RatAI ratAI;
	private Rigidbody2D rb2d;
	private Toy toy;
	private EnemyAttack enemyAttack;

	private void Start() {
		toy = ToyManager.Instance.CreateRandom();
		toy.transform.SetParent(backpack, false);
		toy.GetComponent<DepthSort>().positionSource = transform;

		GetComponent<Health>().onDeath.AddListener(OnDeath);
		animator = GetComponent<Animator>();
		rb2d = GetComponent<Rigidbody2D>();
		ratAI = GetComponent<RatAI>();
		enemyAttack = GetComponent<EnemyAttack>();
	}

	private void Update() {
		Vector2 velocity = rb2d.velocity;
		float xRatio = velocity.x / ratAI.maxVelocity;
		float yRatio = velocity.y / ratAI.maxVelocity;
		const float flipThreshold = 0.3f;

		if (!enemyAttack.isAttacking) {
			Vector3 localScale = transform.localScale;
			if (xRatio > flipThreshold)
				localScale.x = -1;
			else if (xRatio < -flipThreshold)
				localScale.x = 1;
			transform.localScale = localScale;
		}

		animator.SetFloat(speedYRatioHash, yRatio);
		animator.SetFloat(speedXRatioHash, xRatio);
	}

	private void OnDeath() {
		toy.Drop(transform);

		Instantiate(deathEffectPrefab, transform.position, Quaternion.identity, transform.parent);
		Destroy(gameObject);
	}
}
