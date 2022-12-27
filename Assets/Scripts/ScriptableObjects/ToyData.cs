using UnityEngine;

[CreateAssetMenu(fileName = "Toy", menuName = "Game/Toy")]
public class ToyData : ScriptableObject {
	public Sprite sprite;
	public int points = 10;
	public float randomWeight = 1f;
}