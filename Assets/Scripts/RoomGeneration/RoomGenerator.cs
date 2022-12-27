using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class RoomGenerator : MonoBehaviour {
	public Room startRoom;
	public Room[] roomPrefabs;

	private SnappingCamera snappingCamera;

	private Room currentRoom;
	private Room nextRoom;
	private Vector2 nextGridCell;
	private Direction nextRoomDirection;

	private Transform playerTransform;

	private void Start() {
		snappingCamera = Camera.main.GetComponent<SnappingCamera>();
		currentRoom = startRoom;
		playerTransform = GameObject.FindWithTag("Player").transform;
	}

	// Update keep track player grid position
	// Collider on the whole(middle of) room TODO: WORK ON THIS
	// Triggers act as both enter and loading zones

	// private void Update() {
	// 	Vector2 playerGridCell = MathX.Floor(snappingCamera.WorldToGrid(playerTransform.position));
	// 	Vector2 currentRoomGridCell = MathX.Floor(snappingCamera.WorldToGrid(currentRoom.transform.position));
	//
	// 	if (playerGridCell != currentRoomGridCell)
	// 		HandleEnteringNewRoom();
	// }

	public void HandleEnteringNewRoom() {
		if (nextRoom == null) return;

		Destroy(currentRoom.gameObject, 1f);

		currentRoom = nextRoom;
		currentRoom.Initialize(nextRoomDirection);
		nextRoom = null;

		CloudManager.Instance.SetClosedBorder(nextRoomDirection.Inverted());
		GameManager.Instance.roomsEntered++;
	}

	private Room GetRandomRoom() {
		int index = Random.Range(0, roomPrefabs.Length - 1);

		Room room = roomPrefabs[index];
		Util.Swap(ref roomPrefabs[index], ref roomPrefabs[roomPrefabs.Length - 1]);
		return room;
	}

	public void LoadRoom(Direction direction) {
		if (nextRoom != null) {
			MoveNextRoom(direction);
			return;
		}

		Vector2 nextGridPosition = snappingCamera.WorldToGrid(currentRoom.transform.position) + direction.ToVector();
		nextGridCell = MathX.Floor(nextGridPosition);

		nextRoom = Instantiate(GetRandomRoom(), snappingCamera.GridToWorld(nextGridPosition), Quaternion.identity);
		nextRoomDirection = direction;
	}

	private void MoveNextRoom(Direction direction) {
		Vector2 nextGridPosition = snappingCamera.WorldToGrid(currentRoom.transform.position) + direction.ToVector();

		nextRoom.transform.position = snappingCamera.GridToWorld(nextGridPosition);
		nextRoomDirection = direction;
	}
}
