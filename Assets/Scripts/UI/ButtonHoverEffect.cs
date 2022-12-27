using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonHoverEffect : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler,
	IPointerDownHandler, IPointerUpHandler {
	public float scaleAmount = 1.5f;
	public float duration = 0.5f;

	private bool isHovering;

	public void OnPointerDown(PointerEventData eventData) {
		transform.DOKill();
		transform.DOScale(1f, duration / 2f)
			.SetUpdate(UpdateType.Normal, true);
	}

	public void OnPointerEnter(PointerEventData eventData) {
		isHovering = true;
		transform.DOKill();
		transform.DOScale(scaleAmount, duration)
			.SetUpdate(UpdateType.Normal, true);
	}

	public void OnPointerExit(PointerEventData eventData) {
		isHovering = false;
		transform.DOKill();
		transform.DOScale(1f, duration)
			.SetUpdate(UpdateType.Normal, true);
	}

	public void OnPointerUp(PointerEventData eventData) {
		transform.DOKill();
		transform.DOScale(isHovering ? scaleAmount : 1f, duration / 2f)
			.SetUpdate(UpdateType.Normal, true);
	}
}
