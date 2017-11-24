using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIImageBlink : MonoBehaviour {

	// Settings
	[Header("Settings")]
	[SerializeField]
	float blinkRate = 1.0f;
	[SerializeField]
	float rockRate = 3.0f;
	[SerializeField]
	float rockAngle = 30.0f;
	// References
	CanvasRenderer thisRenderer;

	// Use this for initialization
	void Start () {
		thisRenderer = GetComponent<CanvasRenderer>();
	}
	
	// Update is called once per frame
	void Update () {
		thisRenderer.SetAlpha(0.75f + (Mathf.Sin(Time.time * blinkRate) / 4.0f));
		transform.localRotation = Quaternion.Euler(0.0f,0.0f,Mathf.Sin(Time.time * rockRate) * rockAngle);
	}
}
