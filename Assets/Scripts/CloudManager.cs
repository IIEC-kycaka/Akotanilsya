using System;
using DG.Tweening;
using UnityEngine;

public class CloudManager : SingletonBehaviour<CloudManager> {
	public GameObject[] borders;
	public float fadeDuration = 1f;
	public float fadeDisplacement = 3f;

	private void Start() {
		for (int i = 0; i < 4; i++) {
			borders[i].transform.position += (Vector3)(((Direction)i).ToVector() * fadeDisplacement);
			borders[i].SetActive(false);
		}
	}

	public void SetClosedBorder(Direction side) {
		for (int i = 0; i < 4; i++) {
			if (i == (int)side)
				CloseBorder((Direction)i);
			else
				OpenBorder((Direction)i);
		}
	}

	private void OpenBorder(Direction side) {
		GameObject border = borders[(int)side];
		if (!border.activeSelf) return;

		Transform borderTransform = border.transform;
		borderTransform.DOLocalMove(borderTransform.localPosition + (Vector3)(side.ToVector() * fadeDisplacement), fadeDuration);
		border.GetComponent<SpriteRenderer>()
			.DOFade(0, fadeDuration)
			.OnComplete(() => border.SetActive(false));
	}

	private void CloseBorder(Direction side) {
		GameObject border = borders[(int)side];
		if (border.activeSelf) return;

		border.SetActive(true);

		Transform borderTransform = border.transform;

		borderTransform.DOLocalMove(borderTransform.localPosition - (Vector3)(side.ToVector() * fadeDisplacement), fadeDuration);
		border.GetComponent<SpriteRenderer>().DOFade(1, fadeDuration);
	}

	[ContextMenu("Top in")]
	private void TopIn() {
		CloseBorder(Direction.Up);
	}

	[ContextMenu("Top out")]
	private void TopOut() {
		OpenBorder(Direction.Up);
	}
}