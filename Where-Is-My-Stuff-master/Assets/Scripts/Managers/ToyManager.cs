using System;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

[DefaultExecutionOrder(-10)]
public class ToyManager : SingletonBehaviour<ToyManager> {
	public Toy toyPrefab;

	[NonSerialized]
	public ToyData[] toys;

	private void Start() {
		toys = Resources.LoadAll<ToyData>("Toys")
			.OrderBy(x => x.name) // In case 'LoadAll' does not have a deterministic order.
			.ToArray();
	}

	public Toy CreateRandom() {
		float sumOfWeights = toys.Sum(x => x.randomWeight);
		float rand = Random.Range(0, sumOfWeights);
		float current = sumOfWeights;

		ToyData toyData = toys[0];

		for (int i = toys.Length - 1; i >= 0; i--) {
			current -= toys[i].randomWeight;
			if (current <= rand) {
				toyData = toys[i];
				break;
			}
		}

		return CreateToy(toyData);
	}

	private Toy CreateToy(ToyData toyData) {
		Toy toy = Instantiate(toyPrefab);
		toy.data = toyData;
		return toy;
	}
}