using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ControlInfoUpdater : MonoBehaviour {

	// Settings
	[Header("Settings")]

	// References to Children
	[Header("Names")]
	[SerializeField]
	Text GameName;
	[SerializeField]
	Text PlayerOneText;
	[SerializeField]
	Text PlayerTwoText;

	[Header("Player 1 Text")]
	[SerializeField]
	Text PlayerOneJoyUp;
	[SerializeField]
	Text PlayerOneJoyLeft;
	[SerializeField]
	Text PlayerOneJoyDown;
	[SerializeField]
	Text PlayerOneJoyRight;
	[SerializeField]
	Text PlayerOneTopLeft;
	[SerializeField]
	Text PlayerOneTopMiddle;
	[SerializeField]
	Text PlayerOneTopRight;
	[SerializeField]
	Text PlayerOneBottomLeft;
	[SerializeField]
	Text PlayerOneBottomMiddle;
	[SerializeField]
	Text PlayerOneBottomRight;
	[SerializeField]
	Text PlayerOneStart;

	[Header("Player 2 Text")]
	[SerializeField]
	Text PlayerTwoJoyUp;
	[SerializeField]
	Text PlayerTwoJoyLeft;
	[SerializeField]
	Text PlayerTwoJoyDown;
	[SerializeField]
	Text PlayerTwoJoyRight;
	[SerializeField]
	Text PlayerTwoTopLeft;
	[SerializeField]
	Text PlayerTwoTopMiddle;
	[SerializeField]
	Text PlayerTwoTopRight;
	[SerializeField]
	Text PlayerTwoBottomLeft;
	[SerializeField]
	Text PlayerTwoBottomMiddle;
	[SerializeField]
	Text PlayerTwoBottomRight;
	[SerializeField]
	Text PlayerTwoStart;

	// Variables
	System.Guid currentDataHandle = System.Guid.Empty;

	void LoadControl(Text controlText, string newText) {
		if (newText == "")
			controlText.transform.parent.gameObject.SetActive(false);
		else {
			controlText.transform.parent.gameObject.SetActive(true);
			controlText.text = newText;
		}
	}

	public void LoadControls(GameInfoData newData) {
		// Don't reload controls (slow)
		if (newData._GameGuid == currentDataHandle)
			return;
		currentDataHandle = newData._GameGuid;

		// Change every text value
		GameName.text = newData.GameName;
		LoadControl(PlayerOneText,newData.PlayerOneControlLabel);
		LoadControl(PlayerTwoText,newData.PlayerTwoControlLabel);

		// Player 1
		LoadControl(PlayerOneJoyUp,newData.KeyW);
		LoadControl(PlayerOneJoyLeft,newData.KeyA);
		LoadControl(PlayerOneJoyDown,newData.KeyS);
		LoadControl(PlayerOneJoyRight,newData.KeyD);
		LoadControl(PlayerOneTopLeft,newData.Key1);
		LoadControl(PlayerOneTopMiddle,newData.Key2);
		LoadControl(PlayerOneTopRight,newData.Key3);
		LoadControl(PlayerOneBottomLeft,newData.KeyZ);
		LoadControl(PlayerOneBottomMiddle,newData.KeyX);
		LoadControl(PlayerOneBottomRight,newData.KeyC);
		LoadControl(PlayerOneStart,newData.Key5);

		// Player Two
		LoadControl(PlayerTwoJoyUp,newData.KeyI);	
		LoadControl(PlayerTwoJoyLeft,newData.KeyJ);
		LoadControl(PlayerTwoJoyDown,newData.KeyK);
		LoadControl(PlayerTwoJoyRight,newData.KeyL);
		LoadControl(PlayerTwoTopLeft,newData.Key7);
		LoadControl(PlayerTwoTopMiddle,newData.Key8);
		LoadControl(PlayerTwoTopRight,newData.Key9);
		LoadControl(PlayerTwoBottomLeft,newData.KeyB);
		LoadControl(PlayerTwoBottomMiddle,newData.KeyN);
		LoadControl(PlayerTwoBottomRight,newData.KeyM);
		LoadControl(PlayerTwoStart,newData.Key6);
	}
}
