using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScreensaverBounce : MonoBehaviour {
	RectTransform thisTransform;
	AudioSource thisAudio;
	Vector3 currentDirection;



	// Use this for initialization
	void Start () {
		thisTransform = GetComponent<RectTransform>();
		thisAudio = GetComponent<AudioSource>();
		currentDirection = new Vector3(1.0f,1.0f,0.0f);
	}
	
	// Update is called once per frame
	void Update () {
		thisTransform.localPosition += currentDirection;
		Vector3 prevCurrentDirection = currentDirection;
		if (thisTransform.localPosition.x + thisTransform.sizeDelta.x / 2.0f > Screen.width / 2.0f) {
			currentDirection.x = -1.0f;
		}
		else if (thisTransform.localPosition.x - thisTransform.sizeDelta.x / 2.0f < -Screen.width / 2.0f) {
			currentDirection.x = 1.0f;
		}
		if (thisTransform.localPosition.y + thisTransform.sizeDelta.y / 2.0f > Screen.height / 2.0f) {
			currentDirection.y = -1.0f;
		}
		else if (thisTransform.localPosition.y - thisTransform.sizeDelta.y / 2.0f < -Screen.height / 2.0f) {
			currentDirection.y = 1.0f;
		}
		if (prevCurrentDirection != currentDirection) {
			thisAudio.Play();
		}
	}
}
