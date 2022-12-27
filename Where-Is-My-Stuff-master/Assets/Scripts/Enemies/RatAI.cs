using UnityEngine;

public class RatAI : MonoBehaviour {
	public Collider2D navigationCollider;
	public Transform followTarget;
	public float slowRadius = 4f;
	public float stopRadius = 1f;

	[Space] public float maxVelocity = 4f;

	public float visionDistance = 5f;
	public float maxSeekForce = 0.5f;
	public float maxAvoidForce = 1f;

	[Space] public bool seeking = true;

	public bool collisionAvoidance = true;
	public bool allowMove = true;

	[Space] public float attackRange = 2f;

	private readonly RaycastHit2D[] raycastHits = new RaycastHit2D[2];
	private EnemyAttack enemyAttack;
	private int enemyLayer;
	private int obstacleLayer;

	private Rigidbody2D rb2d;
	private Vector2 velocity;

	private void Start() {
		rb2d = GetComponent<Rigidbody2D>();
		enemyAttack = GetComponent<EnemyAttack>();
		obstacleLayer = 1 << LayerMask.NameToLayer("Obstacle");
		enemyLayer = 1 << LayerMask.NameToLayer("Enemy");

		if (followTarget == null)
			followTarget = GameObject.FindWithTag("Player").transform;
	}

	private void Update() {
		float sqrDistanceToTarget = (transform.position - followTarget.position).sqrMagnitude;
		if (sqrDistanceToTarget <= attackRange * attackRange) Attack();
	}

	private void FixedUpdate() {
		Vector2 steering = Vector2.zero;

		if (seeking)
			steering += Seek(followTarget.position);

		if (collisionAvoidance)
			steering += AvoidObstacles();

		velocity = Vector2.ClampMagnitude(velocity + steering, maxVelocity);
		if (allowMove)
			rb2d.velocity = velocity;
	}

	private void Attack() {
		enemyAttack.Attack(followTarget.position - transform.position);
	}

	private Vector2 Seek(Vector2 target) {
		Vector2 direction = target - (Vector2) transform.position;
		float distance = direction.magnitude;

		Vector2 desiredVelocity = Vector2.zero;

		if (distance >= stopRadius) {
			desiredVelocity = direction.normalized * maxVelocity;

			if (distance < slowRadius)
				desiredVelocity *= Mathf.Max(distance - stopRadius, 0) / (slowRadius - stopRadius);

			Debug.DrawRay(transform.position, desiredVelocity, Color.red);
		}

		Vector2 steering = desiredVelocity - velocity;
		Debug.DrawRay(transform.position + (Vector3) velocity, steering, Color.blue);

		return Vector2.ClampMagnitude(steering, maxSeekForce);
	}

	private Vector2 AvoidObstacles() {
		Vector2 self = transform.position;

		RaycastHit2D hit =
			RaycastExcludeSelf(self, velocity, visionDistance, obstacleLayer | enemyLayer);
		if (hit.collider == null) return Vector2.zero;

		Debug.DrawLine(transform.position, hit.point, Color.yellow);

		Vector2 obstacle = hit.collider.bounds.center;
		Vector2 selfToObstacle = obstacle - self;
		Vector2 velocityNormalized = velocity.normalized;

		float p = Vector2.Dot(selfToObstacle, velocityNormalized);

		Vector2 avoidance = self + velocityNormalized * p - obstacle;

		Debug.DrawRay(hit.collider.bounds.center, avoidance, Color.magenta);

		return avoidance.normalized * maxAvoidForce;
	}

	private RaycastHit2D RaycastExcludeSelf(
		Vector3 origin, Vector3 direction, float maxDistance, int layerMask) {
		int numHits = Physics2D.RaycastNonAlloc(origin, direction, raycastHits, maxDistance, layerMask);
		for (int i = 0; i < numHits; i++)
			if (raycastHits[i].collider != navigationCollider)
				return raycastHits[i];

		return default;
	}
}
