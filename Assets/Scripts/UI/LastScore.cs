using TMPro;
using UnityEngine;

public class LastScore : MonoBehaviour {
	public TextMeshProUGUI text;

	private void Start() {
		text.text = PlayerPrefs.GetInt("lastscore").ToString();
	}
}
