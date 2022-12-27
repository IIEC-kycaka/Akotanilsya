using System;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

public class Toy : MonoBehaviour {
	public ToyData data;
	public float maxDropDisplacement = 0.8f;
	public float dropBounceHeight = 1f;
	public float dropBounceDuration = 0.5f;

	private Collider2D pickupTrigger;

	private void Start() {
		GetComponent<SpriteRenderer>().sprite = data.sprite;
		pickupTrigger = GetComponent<Collider2D>();
	}

	private void OnTriggerEnter2D(Collider2D other) {
		if (!other.isTrigger && other.attachedRigidbody.CompareTag("Player")) {
			GameManager.Instance.AddScore(data);
			transform.DOKill();
			Destroy(gameObject);
		}
	}

	public void Drop(Transform root) {
		transform.parent = root.parent;
		pickupTrigger.enabled = true;

		float dx = Random.Range(-maxDropDisplacement, maxDropDisplacement);
		Vector3 myPosition = transform.position;
		DOTween.Sequence()
			.Append(transform.DOMoveY(myPosition.y + dropBounceHeight, dropBounceDuration * 0.5f).SetEase(Ease.OutQuad))
			.Append(transform.DOMoveY(myPosition.y, dropBounceDuration * 0.5f).SetEase(Ease.InQuad))
			.Insert(0, transform.DOMoveX(myPosition.x + dx, dropBounceDuration).SetEase(Ease.InQuad))
			.SetLink(gameObject)
			.Play();
	}
}