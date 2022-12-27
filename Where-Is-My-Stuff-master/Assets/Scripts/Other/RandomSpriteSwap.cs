using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class RandomSpriteSwap : MonoBehaviour {
	[MinMaxRange(0, 60)]
	public Vector2 swapInterval;
	public Sprite[] sprites;

	private SpriteRenderer spriteRenderer;

	private IEnumerator Start() {
		spriteRenderer = GetComponent<SpriteRenderer>();
		spriteRenderer.sprite = sprites[Random.Range(0, sprites.Length)];

		while (true) {
			float delay = Random.Range(swapInterval.x, swapInterval.y);
			yield return new WaitForSeconds(delay);

			spriteRenderer.sprite = GetRandomSprite();
		}
	}

	private Sprite GetRandomSprite() {
		if (sprites.Length == 0) return null;
		if (sprites.Length == 1) return sprites[0];

		int index = Random.Range(0, sprites.Length - 1);
		Sprite sprite = sprites[index];
		Util.Swap(ref sprites[index], ref sprites[sprites.Length - 1]);

		return sprite;
	}
}