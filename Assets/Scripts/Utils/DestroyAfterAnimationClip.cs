using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAfterAnimationClip : MonoBehaviour {
	public AnimationClip animationClip;

	private void Start() {
		Destroy(gameObject, animationClip.length);
	}
}