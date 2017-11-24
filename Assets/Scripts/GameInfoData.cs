using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameInfoData {
	// Following two are set by other classes, not loaded.
	public string _GameFolderPath;
	public System.Guid _GameGuid;
	public string _ExtraData = "";
	// Paths & Names
	public string ExeFolder = "";
	public string ExeName = "";
	public string IconName = "icon.png";
	public string ScreenshotName = "screenshot.png";
	public string VideoName = "video.mp4";

	public bool HasVideo = false;
	public string ExtraDataName = "extradata.txt";
	public bool HasExtraData = false;

	// All metadata entries and their defaults.
	public string GameName = "No Title";
	public string Author = "No author.";
	public string Description = "No description.";
	public int MaxPlayers = 1;
	public bool SupportsCabinetControls = true;
	public bool SupportsXboxControls = false;
	public bool IsNew = false;
	public bool IsUpdated = false;
	public bool IsGameJam = false;
	public bool IsRGS = false;
	public bool IsWIP = false;
	public string EngineUsed = "";
	
	// Controls
	public string PlayerOneControlLabel = "Player 1";
	public string PlayerTwoControlLabel = "Player 2";

	// Player 1 Controls
	//   Movement
	public string KeyW = "";
	public string KeyA = "";
	public string KeyS = "";
	public string KeyD = "";

	//   Buttons (top row, then bottom row, then the front button)
	public string Key1 = "";
	public string Key2 = "";
	public string Key3 = "";
	public string KeyZ = "";
	public string KeyX = "";
	public string KeyC = "";
	public string Key5 = "";
	
	// Player 2 Controls
	//   Movement
	public string KeyI = "";
	public string KeyJ = "";
	public string KeyK = "";
	public string KeyL = "";

	//   Buttons (top row, then bottom row, then the front button)
	public string Key7 = "";
	public string Key8 = "";
	public string Key9 = "";
	public string KeyB = "";
	public string KeyN = "";
	public string KeyM = "";
	public string Key6 = "";

	public GameInfoData(string jsonFilePath) {
		LoadFromJson(jsonFilePath);
	}

	public void LoadFromJson(string jsonFilePath) {
		string thisJson = System.IO.File.ReadAllText (jsonFilePath);
		JsonUtility.FromJsonOverwrite (thisJson, this);
		_GameFolderPath = jsonFilePath;
	}
}
