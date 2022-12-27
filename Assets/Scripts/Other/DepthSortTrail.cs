using UnityEngine;

public class DepthSortTrail : DepthSort {
	private TrailRenderer trailRenderer;

	protected override void Start() {
		trailRenderer = GetComponent<TrailRenderer>();
		base.Start();
	}

	protected override void UpdateSortingOrder() {
		trailRenderer.sortingOrder = CalculateSortingOrder();
	}
}