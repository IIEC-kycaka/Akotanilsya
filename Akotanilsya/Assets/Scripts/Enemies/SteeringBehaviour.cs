using System;
using System.Linq;
using UnityEngine;

public class SteeringBehaviour : MonoBehaviour {
	public Collider2D navigationCollider;
	public Transform followTarget;
	public Transform[] obstacles;
	public float visionDistance = 5f;
	public float maxVelocity = 4f;
	public float maxForce = 0.1f;
	public float maxAvoidForce = 0.2f;

	public float maxEnemyAhead = 2f;
	public float enemyAheadRadius = 0.8f;

	public float slowRadius = 4f;
	public float stopRadius = 1f;

	public Vector2 velocity;

	[Space]
	public bool seeking = true;
	public bool collisionAvoidance = true;
	public bool queueing = true;
	public bool allowMove = true;

	private int obstacleLayer;
	private int enemyLayer;

	private void Start() {
		obstacleLayer = 1 << LayerMask.NameToLayer("Obstacle");
		enemyLayer = 1 << LayerMask.NameToLayer("Enemy");
	}

	private void Update() {
		Vector2 steering = Vector2.zero;

		if (seeking)
			steering += Seek(followTarget);

		// foreach (Transform obstacle in obstacles) {
		// 	steering += Flee(obstacle);
		// }

		if (collisionAvoidance)
			steering += AvoidObstacles();

		if (queueing)
			steering += Queue(steering);

		velocity = Vector2.ClampMagnitude(velocity + steering, maxVelocity);
		if (allowMove)
			transform.position += (Vector3)(velocity * Time.deltaTime);
	}

	// private Vector2 Seek(Transform target) {
	// 	Vector2 desiredVelocity = ((Vector2)target.position - (Vector2)transform.position).normalized * maxVelocity;
	// 	Debug.DrawRay(transform.position, desiredVelocity, Color.red);
	// 	Vector2 steering = desiredVelocity - velocity;
	// 	Debug.DrawRay(transform.position + (Vector3)velocity, steering, Color.blue);
	// 	return Vector2.ClampMagnitude(steering, maxForce);
	// }

	private Vector2 Seek(Transform target) {
		Vector2 direction = (Vector2)target.position - (Vector2)transform.position;
		float distance = direction.magnitude;

		Vector2 desiredVelocity = Vector2.zero;

		if (distance >= stopRadius) {
			desiredVelocity = direction.normalized * maxVelocity;

			if (distance < slowRadius)
				desiredVelocity *= distance / slowRadius;

			Debug.DrawRay(transform.position, desiredVelocity, Color.red);
		}

		Vector2 steering = desiredVelocity - velocity;
		Debug.DrawRay(transform.position + (Vector3)velocity, steering, Color.blue);

		return Vector2.ClampMagnitude(steering, maxForce);
	}

	private Vector2 Flee(Transform target) {
		Vector2 desiredVelocity = ((Vector2)transform.position - (Vector2)target.position).normalized * maxVelocity;
		Vector2 steering = desiredVelocity - velocity;
		return Vector2.ClampMagnitude(steering, maxForce * 0.5f);
	}

	private Vector2 AvoidObstacles() {
		Vector2 self = transform.position;

		RaycastHit2D hit = Physics2D.Raycast(self, velocity, visionDistance, obstacleLayer | enemyLayer);
		if (hit.collider == null) return Vector2.zero;

		Debug.DrawLine(transform.position, hit.point, Color.yellow);

		Vector2 obstacle = hit.collider.bounds.center;
		Vector2 selfToObstacle = obstacle - self;

		float p = Vector2.Dot(selfToObstacle, velocity.normalized);

		Vector2 avoidance = (self + velocity.normalized * p) - obstacle;

		//print($"p: {p}, avoidance: {avoidance}");
		//Vector2 avoidance = visionRay - (Vector2)hit.collider.bounds.center;


		Debug.DrawRay(hit.collider.bounds.center, avoidance, Color.magenta);

		return avoidance.normalized * maxAvoidForce;
	}


	// private Vector2? GetNeighborAhead() {
	// 	Vector2 aheadPoint = (Vector2)transform.position + velocity.normalized * maxEnemyAhead;
	// 	aheadCenter = aheadPoint;
	//
	// 	Collider2D[] hits = Physics2D.OverlapCircleAll(aheadPoint, enemyAheadRadius, enemyLayer);
	//
	// 	float minSqrDistance = float.PositiveInfinity;
	// 	Collider2D closestCollider = null;
	//
	// 	foreach (Collider2D hit in hits) {
	// 		if (hit == myCollider) continue;
	//
	// 		float sqrDist = (aheadPoint - (Vector2)hit.bounds.center).sqrMagnitude;
	// 		if (sqrDist < minSqrDistance) {
	// 			minSqrDistance = sqrDist;
	// 			closestCollider = hit;
	// 		}
	// 	}
	//
	// 	return closestCollider != null ? (Vector2?)closestCollider.bounds.center : null;
	// }

	private Vector2? FindClosestNeighbor(Vector2 position) {
		Collider2D[] hits = Physics2D.OverlapCircleAll(position, enemyAheadRadius, enemyLayer);

		float minSqrDistance = float.PositiveInfinity;
		Collider2D closestCollider = null;

		foreach (Collider2D hit in hits) {
			if (hit == navigationCollider) continue;

			float sqrDist = (position - (Vector2)hit.bounds.center).sqrMagnitude;
			if (sqrDist < minSqrDistance) {
				minSqrDistance = sqrDist;
				closestCollider = hit;
			}
		}

		return closestCollider != null ? (Vector2?)closestCollider.bounds.center : null;
	}

	private Vector2 Queue(Vector2 steering) {
		Vector2 aheadPoint = (Vector2)transform.position + velocity.normalized * maxEnemyAhead;
		aheadCenter = aheadPoint;

		Vector2? closestNeighbor = FindClosestNeighbor(transform.position);
		if (closestNeighbor.HasValue && ((Vector2)transform.position - closestNeighbor.Value).sqrMagnitude <= enemyAheadRadius * enemyAheadRadius)
			velocity *= 0.3f;

		// Vector2? closestNeighborAhead = FindClosestNeighbor(aheadPoint);
		// if (closestNeighborAhead.HasValue) {
		// 	Vector2 brake = -steering * 0.8f;
		// 	return brake - velocity;
		// }

		return Vector2.zero;
	}


	private Vector2 aheadCenter;

	private void OnDrawGizmos() {
		Gizmos.DrawRay(transform.position, velocity);
		// Gizmos.DrawWireSphere(aheadCenter, enemyAheadRadius);
	}
}