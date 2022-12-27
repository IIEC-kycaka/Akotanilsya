using System;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : SingletonBehaviour<GameManager> {
	public UnityEvent<int> onPointAdded;
	public int roomsEntered;

	[NonSerialized] public int[] toyCounts;

	public int TotalScore => toyCounts.Sum(x => x);

	private void Start() {
		toyCounts = new int[ToyManager.Instance.toys.Length];
	}

	public void AddScore(ToyData toyType) {
		int index = Array.IndexOf(ToyManager.Instance.toys, toyType);
		if (index == -1) {
			Debug.LogError($"Failed to add score: Toy not found {toyType}");
			return;
		}

		toyCounts[index] += 1;
		onPointAdded.Invoke(index);
	}
}
