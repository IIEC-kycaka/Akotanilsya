using System;
using UnityEngine;

public enum Direction {
	Up = 0,
	Right = 1,
	Down = 2,
	Left = 3,
}

static class DirectionExtensions {
	public static Vector2 ToVector(this Direction direction) {
		switch (direction) {
			case Direction.Up:
				return new Vector2(0, 1);
			case Direction.Right:
				return new Vector2(1, 0);
			case Direction.Down:
				return new Vector2(0, -1);
			case Direction.Left:
				return new Vector2(-1, 0);
			default:
				throw new ArgumentOutOfRangeException();
		}
	}

	public static Direction Inverted(this Direction direction) {
		switch (direction) {
			case Direction.Up:    return Direction.Down;
			case Direction.Right: return Direction.Left;
			case Direction.Down:  return Direction.Up;
			case Direction.Left:  return Direction.Right;
			default:              throw new ArgumentOutOfRangeException();
		}
	}
}