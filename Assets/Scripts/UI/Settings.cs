using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class Settings : MonoBehaviour {
	public AudioMixer mixer;

	public Slider musicSlider;
	public Slider sfxSlider;

	private void Start() {
		if (!PlayerPrefs.HasKey("musicVolume"))
			PlayerPrefs.SetFloat("musicVolume", 0.5f);

		if (!PlayerPrefs.HasKey("sfxVolume"))
			PlayerPrefs.SetFloat("sfxVolume", 0.5f);

		SetVolume();
	}

	private void SetVolume() {
		float musicVolume = PlayerPrefs.GetFloat("musicVolume");
		float sfxVolume = PlayerPrefs.GetFloat("sfxVolume");

		mixer.SetFloat("musicVolume", musicVolume);
		mixer.SetFloat("sfxVolume", sfxVolume);

		musicSlider.value = musicVolume;
		sfxSlider.value = sfxVolume;
	}

	public void HandleVolumeChange() {
		float musicVolume = MathX.LinearToDecibels(musicSlider.value);
		float sfxVolume = MathX.LinearToDecibels(sfxSlider.value);

		mixer.SetFloat("musicVolume", musicVolume);
		mixer.SetFloat("sfxVolume", sfxVolume);

		PlayerPrefs.SetFloat("musicVolume", musicSlider.value);
		PlayerPrefs.SetFloat("sfxVolume", sfxSlider.value);
	}
}
