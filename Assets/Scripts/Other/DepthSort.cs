using System;
using UnityEngine;

public class DepthSort : MonoBehaviour {
	private const int SortingValues = 1 << 16;
	private const int OffsetValues = 256;

	public Transform positionSource;
	public bool isStatic = false;
	[Range(-128, 127)]
	public int orderOffset;

	private SpriteRenderer spriteRenderer;
	private SnappingCamera snappingCamera;

	protected virtual void Start() {
		spriteRenderer = GetComponent<SpriteRenderer>();
		snappingCamera = Camera.main.GetComponent<SnappingCamera>();

		if (isStatic) {
			enabled = false;
			UpdateSortingOrder();
		}
	}

	private void LateUpdate() {
		UpdateSortingOrder();
	}

	protected int CalculateSortingOrder() {
		Vector2 gridPosition = snappingCamera.WorldToGrid((positionSource != null ? positionSource : transform).position);
		float t = 1 - MathX.Fract(gridPosition.y);
		int order = Mathf.FloorToInt(t * (SortingValues - OffsetValues) - (SortingValues - OffsetValues) / 2) + orderOffset;
		return order;
	}

	protected virtual void UpdateSortingOrder() {
		spriteRenderer.sortingOrder = CalculateSortingOrder();
	}
}