using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class RandomAudio : MonoBehaviour {
	public bool useAudioSource = true;
	public AudioClip[] sounds;

	private AudioSource audioSource;

	private void Start() {
		audioSource = GetComponent<AudioSource>();
	}

	public void Play() {
		audioSource.clip = sounds[Random.Range(0, sounds.Length)];
		audioSource.Play();
	}

	public void DetachAndDestroyWhenDone() {
		if (!audioSource.isPlaying) return;

		transform.parent = null;
		Destroy(gameObject, audioSource.clip.length - audioSource.time);
	}
}