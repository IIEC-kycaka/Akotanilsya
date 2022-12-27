using System;
using UnityEngine;

public class Room : MonoBehaviour {
	public LoadingZoneHandler loadingZoneHandler;
	public EnemySpawnHandler enemySpawnHandler;

	public void Initialize(Direction direction) {
		loadingZoneHandler.ActivateLoadingZones(direction);
		enemySpawnHandler.InitializeEnemySpawning();
	}
}
