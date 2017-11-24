using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using UnityEngine.EventSystems;

public enum InfoPanelState {
	GamePanel,
	AboutPanel,
	ControlsPanel
};

public class DisplayManager : MonoBehaviour {

	public static DisplayManager instance
	{
		get {
			if (_instance != null)
				return _instance;
			else return null;
		}
	}

	private static DisplayManager _instance;

	// Settings
	[Header("Settings")]
	[SerializeField]
	bool enableVideoPlayback = true;
	[SerializeField]
	float screenSaverDelay = 300.0f;
	[SerializeField]
	bool hideMouseCursor = false;

	// Variables
	public InfoPanelState currentState {
		get {
			return _currentState;
		}
	}
	InfoPanelState _currentState = InfoPanelState.GamePanel;
	GameInfoData lastGameInfo;


	// UI elements in Info Display Panel
	[Header("Object References")]
	[SerializeField]
	GameObject ScreenSaver;

	// Info descriptor icons
	[Header("Descriptor Icons")]
	[SerializeField]
	GameObject OnePlayerIcon;
	[SerializeField]
	GameObject TwoPlayerIcon;
	[SerializeField]
	GameObject ThreePlayerIcon;
	[SerializeField]
	GameObject FourPlayerIcon;
	[SerializeField]
	GameObject CabinetControlsIcon;
	[SerializeField]
	GameObject XboxControlsIcon;
	[SerializeField]
	GameObject UnityIcon;
	[SerializeField]
	GameObject UnrealIcon;
	[SerializeField]
	GameObject GameMakerIcon;
	[SerializeField]
	GameObject Love2DIcon;
	[SerializeField]
	GameObject GameJamIcon;
	[SerializeField]
	GameObject RGSIcon;
	[SerializeField]
	GameObject WIPIcon;

	// Info Panel State Groups
	[Header("Info Panel State Groups")]
	[SerializeField]
	CanvasGroup GameCanvasGroup;
	[SerializeField]
	CanvasGroup AboutCanvasGroup;
	[SerializeField]
	CanvasGroup ControlsCanvasGroup;

	// Component References
	[Header("Component References")]
	[SerializeField]
	Text GameNameText;
	[SerializeField]
	Text GameDescriptionText;
	[SerializeField]
	Text ExtraDataText;
	[SerializeField]
	Image GameScreenshotImage;
	[SerializeField]
	VideoPlayer GameVideoPreview;

	// Variables

	Vector3 lastMousePosition;
	float lastInputTime = 0.0f;

	

	// Use this for initialization
	void Start () {
		if (_instance == null) _instance = this;
		if (hideMouseCursor)
			Cursor.visible = false;
	}

	void Update () {
		if (Input.GetKey(KeyCode.Escape))
			Application.Quit();
		if (Input.anyKey || Input.mousePosition != lastMousePosition)
			lastInputTime = Time.time;
		lastMousePosition = Input.mousePosition;
		// Screensaver logic
		if (Time.time - lastInputTime > screenSaverDelay) {
			if (!ScreenSaver.active) {
				ScreenSaver.SetActive(true);
				GameVideoPreview.Pause();
			}
		}
		else {
			if (ScreenSaver.active) {
				ScreenSaver.SetActive(false);
				Input.ResetInputAxes();
				if (lastGameInfo.HasVideo && currentState == InfoPanelState.GamePanel)
					GameVideoPreview.Play();
			}
		}
	}
	public void SetInfoPanelState(InfoPanelState newState) {
		if (newState != InfoPanelState.GamePanel)
			GameVideoPreview.Pause();
		_currentState = newState;
		GameCanvasGroup.alpha = (newState == InfoPanelState.GamePanel ? 1.0f : 0.0f);
		AboutCanvasGroup.alpha = (newState == InfoPanelState.AboutPanel ? 1.0f : 0.0f);
		ControlsCanvasGroup.alpha = (newState == InfoPanelState.ControlsPanel ? 1.0f : 0.0f);
	}
	
	public void SetInfo(System.Guid gameInfoHandle, bool forceUpdate = false) {
		// Don't reload the game info if it's already being displayed.
		// Requires some extra checks to make sure prepared videos replay.
		if (lastGameInfo != null) {
			if (gameInfoHandle == lastGameInfo._GameGuid && !forceUpdate) {
				if (enableVideoPlayback && lastGameInfo.HasVideo && !GameVideoPreview.isPlaying) {
					GameVideoPreview.time = 0.0;
					GameVideoPreview.Play();
					GameScreenshotImage.enabled = false;
				}
				return;
			}
		}
		
		lastGameInfo = DataManager.instance.GetInfo(gameInfoHandle);
       	Sprite retrievedShot = DataManager.instance.GetScreenshot(gameInfoHandle);

		// Set some text
		GameNameText.text = lastGameInfo.GameName;
		GameDescriptionText.text = "<b>" + lastGameInfo.Author + "</b>\n\n" + lastGameInfo.Description;
		ExtraDataText.text = lastGameInfo._ExtraData;


		// Video or screenshot?
		string videoPath = System.IO.Path.Combine(System.IO.Path.Combine(lastGameInfo._GameFolderPath,"CabinetData"),lastGameInfo.VideoName);
		if (lastGameInfo.HasVideo && enableVideoPlayback && System.IO.File.Exists(videoPath)) {
			GameVideoPreview.url = videoPath;
			GameScreenshotImage.enabled = false;
		}
		else {
			GameVideoPreview.Pause();
			GameScreenshotImage.sprite = retrievedShot;
			GameScreenshotImage.enabled = true;
		}

		// Update controls
		
		// Set icon visibility.
		OnePlayerIcon.SetActive(lastGameInfo.MaxPlayers == 1 ? true : false);
		TwoPlayerIcon.SetActive(lastGameInfo.MaxPlayers == 2 ? true : false);
		ThreePlayerIcon.SetActive(lastGameInfo.MaxPlayers == 3 ? true : false);
		FourPlayerIcon.SetActive(lastGameInfo.MaxPlayers == 4 ? true : false);
		CabinetControlsIcon.SetActive(lastGameInfo.SupportsCabinetControls ? true : false);
		XboxControlsIcon.SetActive(lastGameInfo.SupportsXboxControls ? true : false);
		UnityIcon.SetActive(lastGameInfo.EngineUsed.StartsWith("Unity") ? true : false);
		UnrealIcon.SetActive(lastGameInfo.EngineUsed.StartsWith("Unreal") ? true : false);
		GameMakerIcon.SetActive(lastGameInfo.EngineUsed.StartsWith("Game Maker") ? true : false);
		Love2DIcon.SetActive(lastGameInfo.EngineUsed.StartsWith("LÖVE") ? true : false);
		GameJamIcon.SetActive(lastGameInfo.IsGameJam ? true : false);
		RGSIcon.SetActive(lastGameInfo.IsRGS ? true : false);
		WIPIcon.SetActive(lastGameInfo.IsWIP ? true : false);
	}
}
