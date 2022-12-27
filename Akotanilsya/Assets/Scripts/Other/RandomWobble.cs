using System;
using UnityEngine;

public class RandomWobble : MonoBehaviour {
	public float frequency = 1f;
	public float strength = 1f;

	private Vector3 basePosition;

	private void Start() {
		basePosition = transform.localPosition;
	}

	private void Update() {
		Vector3 delta = new Vector3(
			GetNoise(0, Time.time * frequency) * strength,
			GetNoise(7, Time.time * frequency) * strength, 0
		);

		transform.localPosition = basePosition + delta;
	}

	private static float GetNoise(float x, float y) {
		float value = Mathf.PerlinNoise(x, y) * 2f - 1f;
		return Mathf.Clamp(value, -1f, 1f);
	}
}