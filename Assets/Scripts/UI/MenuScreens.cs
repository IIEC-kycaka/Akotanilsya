using DG.Tweening;
using UnityEngine;

public class MenuScreens : MonoBehaviour {
	public RectTransform mainScreen;
	public RectTransform creditsScreen;
	public RectTransform scoreScreen;
	private readonly float duration = 0.5f;

	private RectTransform currentScreen;

	private void Start() {
		currentScreen = mainScreen;
	}

	public void ShowMainScreen() {
		DOTween.Sequence()
			.Append(currentScreen.DOMoveX(-currentScreen.rect.width, duration))
			.Append(mainScreen.DOMoveX(0, duration)).SetEase(Ease.InOutSine)
			.Play();
	}

	public void ShowScreen(RectTransform screen) {
		DOTween.Sequence()
			.Append(mainScreen.DOMoveX(-mainScreen.rect.width, duration))
			.Append(screen.DOMoveX(0, duration)).SetEase(Ease.InOutSine)
			.Play();

		currentScreen = screen;
	}
}
