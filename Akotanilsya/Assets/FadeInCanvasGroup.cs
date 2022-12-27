using DG.Tweening;
using UnityEngine;

public class FadeInCanvasGroup : MonoBehaviour {
	public float duration;

	private CanvasGroup canvasGroup;

	private void Awake() {
		canvasGroup = GetComponent<CanvasGroup>();
		canvasGroup.alpha = 0f;
	}

	void Start() {
		canvasGroup.DOFade(1f, duration).SetEase(Ease.OutQuad);
	}
}