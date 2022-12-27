using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class BobUpDown : MonoBehaviour {
	public float bobAmount = 0.1f;
	public float bobDuration = 0.5f;

	private Vector3 baseLocalPosition;

	private void Start() {
		baseLocalPosition = transform.localPosition;

		transform.localPosition = baseLocalPosition - new Vector3(0, bobAmount, 0);
		DOTween.Sequence()
			.Append(transform.DOLocalMoveY(baseLocalPosition.y + bobAmount, bobDuration).SetEase(Ease.InOutQuad))
			.Append(transform.DOLocalMoveY(baseLocalPosition.y - bobAmount, bobDuration).SetEase(Ease.InOutQuad))
			.SetLoops(-1)
			.Play();
	}
}