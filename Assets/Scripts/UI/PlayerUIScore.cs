using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[DefaultExecutionOrder(10)]
public class PlayerUIScore : MonoBehaviour {
	public GameObject UIScorePrefab;

	private TextMeshProUGUI[] texts;
	private Image[] images;

	private void Start() {
		Setup();

		GameManager.Instance.onPointAdded.AddListener(HandlePointAdd);
	}

	private void Setup() {
		ToyData[] toys = ToyManager.Instance.toys;
		texts = new TextMeshProUGUI[toys.Length];
		images = new Image[toys.Length];

		for (int i = 0; i < toys.Length; i++) {
			GameObject go = Instantiate(UIScorePrefab, transform);

			images[i] = go.GetComponentInChildren<Image>();
			images[i].sprite = toys[i].sprite;
			texts[i] = go.GetComponentsInChildren<TextMeshProUGUI>()[1];
			texts[i].text = GameManager.Instance.toyCounts[i].ToString();
		}
	}

	private void HandlePointAdd(int index) {
		for (int i = 0; i < texts.Length; i++)
			texts[i].text = GameManager.Instance.toyCounts[i].ToString();

		images[index].transform.DOPunchScale(new Vector3(0.2f, 0.2f, 0), 0.5f);
	}
}
