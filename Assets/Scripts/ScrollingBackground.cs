using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollingBackground : MonoBehaviour {
	[SerializeField]
	float scrollSpeed = 0.5f;
	CanvasRenderer thisRenderer;

	// Use this for initialization
	void Start () {
		thisRenderer = GetComponent<CanvasRenderer>();
	}
	
	// Update is called once per frame
	void Update () {
		// Check if the material exists because it never finds the material on the first frame or so...
		if (thisRenderer.GetMaterial() != null) {
			Vector2 offset = new Vector2(Mathf.Cos(Time.time * scrollSpeed),Mathf.Sin(Time.time * scrollSpeed));
			thisRenderer.GetMaterial().SetTextureOffset("_MainTex", offset);
		}
	}
}
