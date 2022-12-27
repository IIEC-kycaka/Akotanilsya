using System;
using UnityEngine;

public class BorderHandler : MonoBehaviour {
	public Collider2D leftBorder;
	public Collider2D rightBorder;
	public Collider2D topBorder;
	public Collider2D bottomBorder;

	private Collider2D entryBorder;

	public void HandleNewEnterDirection(Direction direction) {
		leftBorder.gameObject.SetActive(true);
		rightBorder.gameObject.SetActive(true);
		topBorder.gameObject.SetActive(true);
		bottomBorder.gameObject.SetActive(true);

		switch (direction) {
			case Direction.Up:
				entryBorder = bottomBorder;
				bottomBorder.gameObject.SetActive(false);
				break;
			case Direction.Right:
				entryBorder = leftBorder;
				leftBorder.gameObject.SetActive(false);
				break;
			case Direction.Down:
				entryBorder = topBorder;
				topBorder.gameObject.SetActive(false);
				break;
			case Direction.Left:
				entryBorder = rightBorder;
				rightBorder.gameObject.SetActive(false);
				break;
			default:
				throw new ArgumentOutOfRangeException();
		}
	}

	public void OpenExits() {
		leftBorder.gameObject.SetActive(false);
		rightBorder.gameObject.SetActive(false);
		topBorder.gameObject.SetActive(false);
		bottomBorder.gameObject.SetActive(false);

		entryBorder.gameObject.SetActive(true);
	}
}
