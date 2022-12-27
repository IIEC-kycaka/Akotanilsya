using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicPlayer : SingletonBehaviour<MusicPlayer> {
	public AudioClip menuMusic;
	public AudioClip gameMusic;
	public SceneReference gameScene;

	private AudioSource currentAudioSource;
	private AudioSource secondAudioSource;
	private float baseVolume;
	private int currentSceneIndex;

	protected override void Awake() {
		base.Awake();

		DontDestroyOnLoad(this);

		currentSceneIndex = SceneManager.GetActiveScene().buildIndex;

		currentAudioSource = GetComponent<AudioSource>();
		currentAudioSource.clip = currentSceneIndex == gameScene.BuildIndex ? gameMusic : menuMusic;
		currentAudioSource.Play();

		baseVolume = currentAudioSource.volume;

		secondAudioSource = gameObject.AddComponent<AudioSource>();
		secondAudioSource.loop = currentAudioSource.loop;
		secondAudioSource.outputAudioMixerGroup = currentAudioSource.outputAudioMixerGroup;
	}

	private void OnEnable() {
		SceneManager.activeSceneChanged += OnSceneChanged;
	}

	private void OnDisable() {
		SceneManager.activeSceneChanged -= OnSceneChanged;
	}

	private void OnSceneChanged(Scene _, Scene to) {
		if ((currentSceneIndex == gameScene.BuildIndex) != (to.buildIndex == gameScene.BuildIndex))
			CrossFadeToClip(to.buildIndex == gameScene.BuildIndex ? gameMusic : menuMusic);

		currentSceneIndex = to.buildIndex;
	}

	// private void FadeToClip(AudioClip newClip) {
	// 	DOTween.Sequence()
	// 		.Append(currentSource.DOFade(0, 0.5f))
	// 		.AppendCallback(() => {
	// 			float time = currentSource.time;
	// 			currentSource.clip = newClip;
	// 			currentSource.time = time;
	// 			currentSource.Play();
	// 		})
	// 		.Append(currentSource.DOFade(1, 0.5f))
	// 		.Play();
	// }

	private void CrossFadeToClip(AudioClip newClip) {
		secondAudioSource.volume = 0;
		secondAudioSource.clip = newClip;
		secondAudioSource.timeSamples = currentAudioSource.timeSamples;
		secondAudioSource.Play();

		DOTween.Sequence()
			.Append(currentAudioSource.DOFade(0, 1f))
			.Join(secondAudioSource.DOFade(baseVolume, 1f))
			.AppendCallback(() => {
				currentAudioSource.Stop();
				Util.Swap(ref currentAudioSource, ref secondAudioSource);
			})
			.Play();
	}
}