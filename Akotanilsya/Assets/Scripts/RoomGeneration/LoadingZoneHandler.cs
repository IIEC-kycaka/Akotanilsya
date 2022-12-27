using System;
using UnityEngine;

public class LoadingZoneHandler : MonoBehaviour {
	public RoomGenerator roomGenerator;

	public Collider2D leftLoadingZone;
	public Collider2D rightLoadingZone;
	public Collider2D topLoadingZone;
	public Collider2D bottomLoadingZone;

	private Direction enterDirection;

	private void Start() {
		roomGenerator = GameObject.FindWithTag("Room Generator").GetComponent<RoomGenerator>();
	}

	private void OnTriggerEnter2D(Collider2D other) {
		if (!other.CompareTag("Player")) return;
		if (other.isTrigger) return;

		// We're assuming that the player will never touch two collider simultaneously
		if (other.IsTouching(leftLoadingZone))
			roomGenerator.LoadRoom(Direction.Left);
		else if (other.IsTouching(rightLoadingZone))
			roomGenerator.LoadRoom(Direction.Right);
		else if (other.IsTouching(topLoadingZone))
			roomGenerator.LoadRoom(Direction.Up);
		else if (other.IsTouching(bottomLoadingZone))
			roomGenerator.LoadRoom(Direction.Down);
	}

	public void ActivateLoadingZones(Direction direction) {
		enterDirection = direction;

		leftLoadingZone.gameObject.SetActive(true);
		rightLoadingZone.gameObject.SetActive(true);
		topLoadingZone.gameObject.SetActive(true);
		bottomLoadingZone.gameObject.SetActive(true);

		switch (direction) {
			case Direction.Up:
				bottomLoadingZone.gameObject.SetActive(false);
				break;
			case Direction.Right:
				leftLoadingZone.gameObject.SetActive(false);
				break;
			case Direction.Down:
				topLoadingZone.gameObject.SetActive(false);
				break;
			case Direction.Left:
				rightLoadingZone.gameObject.SetActive(false);
				break;
			default:
				throw new ArgumentOutOfRangeException();
		}
	}
}
