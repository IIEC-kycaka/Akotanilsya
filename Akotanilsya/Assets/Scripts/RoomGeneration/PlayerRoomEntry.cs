using System;
using UnityEngine;

public class PlayerRoomEntry : MonoBehaviour {
	public LoadingZoneHandler loadingZoneHandler;
	private RoomGenerator roomGenerator;

	private void Start() {
		roomGenerator = GameObject.FindWithTag("Room Generator").GetComponent<RoomGenerator>();
	}

	private void OnTriggerEnter2D(Collider2D other) {
		if (other.CompareTag("Player") && !other.isTrigger) {
			roomGenerator.HandleEnteringNewRoom();
			loadingZoneHandler.gameObject.SetActive(true);
			transform.gameObject.SetActive(false);
		}
	}
}
