using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MenuNavigator : MonoBehaviour {
	// Components & References
	[Header("References")]
	[SerializeField]
	ScrollRect scrollRect;
	[SerializeField]
	Transform scrollRectContent;

	// Tweakable Settings
	[Header("Settings")]
	[SerializeField]
	float iconTransitionScale = 1.0f;

	// Variables
	float timeSinceLastSelection = 0;
	GameObject lastValidSelection;

	// Instance
	public static MenuNavigator instance
	{
		get {
			if (_instance != null)
				return _instance;
			else return null;
		}
	}

	private static MenuNavigator _instance;

	int currentListIndex {
		get {
			if (EventSystem.current.currentSelectedGameObject != null)
				if (EventSystem.current.currentSelectedGameObject.transform.parent == scrollRectContent)
					return EventSystem.current.currentSelectedGameObject.transform.GetSiblingIndex();
			return -1;
		}
	}

	// Use this for initialization
	void Start () {
		if (_instance == null) _instance = this;
		EventSystem.current.firstSelectedGameObject = scrollRectContent.GetChild(0).gameObject;
		lastValidSelection = EventSystem.current.firstSelectedGameObject;
	}

	public bool SelectNextItem() {
		if (EventSystem.current.currentSelectedGameObject != null) {
			int thisIndex = EventSystem.current.currentSelectedGameObject.transform.GetSiblingIndex();
			int nextIndex = thisIndex + 1 >= scrollRectContent.childCount ? 0 : thisIndex + 1;
			EventSystem.current.SetSelectedGameObject(scrollRectContent.GetChild(nextIndex).gameObject);
			lastValidSelection = EventSystem.current.currentSelectedGameObject;
			timeSinceLastSelection = Time.time;
			return true;
		}
		else return false;
	}

	public bool SelectPreviousItem() {
		if (EventSystem.current.currentSelectedGameObject != null) {
			int thisIndex = EventSystem.current.currentSelectedGameObject.transform.GetSiblingIndex();
			int nextIndex = thisIndex - 1 < 0 ? scrollRectContent.childCount - 1 : thisIndex - 1;
			EventSystem.current.SetSelectedGameObject(scrollRectContent.GetChild(nextIndex).gameObject);
			lastValidSelection = EventSystem.current.currentSelectedGameObject;
			timeSinceLastSelection = Time.time;
			return true;
		}
		else return false;
	}

	public bool SelectByIndex(int index) {
		if (index < 0 || index >= scrollRectContent.childCount) return false;
		EventSystem.current.SetSelectedGameObject(scrollRectContent.GetChild(index).gameObject);
		lastValidSelection = EventSystem.current.currentSelectedGameObject;
		timeSinceLastSelection = Time.time;
		return true;
	}

	// Update is called once per frame
	void Update () {
		int currentIndex = currentListIndex;
		if (currentIndex >= 0) {
			float newPos = (float)currentIndex / (scrollRectContent.childCount - 1);
			scrollRect.horizontalNormalizedPosition = Mathf.Lerp(scrollRect.horizontalNormalizedPosition, newPos, (Time.time - timeSinceLastSelection) * iconTransitionScale);
		}
	}
}
