using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

using GameInfoDict = System.Collections.Generic.Dictionary<System.Guid, GameInfoData>;
using SpriteDict = System.Collections.Generic.Dictionary<System.Guid, UnityEngine.Sprite>;

public class DataManager : MonoBehaviour {
	[Header("References")]
	[SerializeField]
	GameObject gameIconPrefab;
	[SerializeField]
	Transform gameListContent;
	GameInfoDict gameInfoDataDict;
	SpriteDict iconDict;
	SpriteDict screenShotDict;

	public static DataManager instance
	{
		get {
			if (_instance != null)
				return _instance;
			else return null;
		}
	}

	private static DataManager _instance;

	public Sprite LoadSprite(string FilePath, float PixelsPerUnit = 100.0f) {
		// Load a PNG or JPG image from disk to a Texture2D, assign this texture to a new sprite and return its reference
		Sprite NewSprite = new Sprite();
		Texture2D SpriteTexture = LoadTexture(FilePath);
		NewSprite = Sprite.Create(SpriteTexture, new Rect(0, 0, SpriteTexture.width, SpriteTexture.height),new Vector2(0,0), PixelsPerUnit);

		return NewSprite;
	}
	public Texture2D LoadTexture(string FilePath) {
		// Load a PNG or JPG file from disk to a Texture2D
		// Returns null if load fails
		Texture2D Tex2D;
		byte[] FileData;
		if (File.Exists(FilePath)) {
			FileData = File.ReadAllBytes(FilePath);
			Tex2D = new Texture2D(2, 2);		// Create new "empty" texture
			if (Tex2D.LoadImage(FileData))	// Load the imagedata into the texture (size is set automatically)
			return Tex2D;					// If data = readable -> return texture
		}
		return null;						// Return null if load failed
	}

	void Start () {
		if (_instance == null) _instance = this;
		// Initialize data structures
		gameInfoDataDict = new GameInfoDict();
		iconDict = new SpriteDict();
		screenShotDict = new SpriteDict();
		InitializeIcons();
	}

	void InitializeIcons() {
		string PathToGames;
		string[] gameFolders;
		#if (UNITY_EDITOR_OSX)
			PathToGames = Path.GetFullPath("Games");
		#else
			PathToGames = Path.GetFullPath(Path.Combine(Application.dataPath.Replace('/','\\'), @"..\Games"));
		#endif
		gameFolders = Directory.GetDirectories (PathToGames);
		foreach (string folderPath in gameFolders) {
			string cabinetDataPath = Path.Combine(folderPath, "CabinetData");
			string metaFilePath = Path.Combine(cabinetDataPath, "metadata.json");;
			if (File.Exists(metaFilePath)) {
				GameObject newGameIcon = Instantiate (gameIconPrefab, gameListContent);
				GameObject newGameIconBadge = newGameIcon.transform.GetChild(2).gameObject;
				GameObject updatedGameIconBadge = newGameIcon.transform.GetChild(3).gameObject;
				MenuEventTrigger iconTrigger = newGameIcon.GetComponent<MenuEventTrigger>();
				System.Guid newGuid = System.Guid.NewGuid();
				iconTrigger.SetGuid(newGuid);
				GameInfoData newMetadata = new GameInfoData(metaFilePath);
				newMetadata._GameFolderPath = folderPath;
				newMetadata._GameGuid = newGuid;
				newGameIcon.name = newMetadata.GameName;
				if (newMetadata.IsUpdated) {
					updatedGameIconBadge.SetActive(true);
					newGameIcon.transform.SetAsFirstSibling();
				}
				else if (newMetadata.IsNew) {
					newGameIconBadge.SetActive(true);
					newGameIcon.transform.SetAsFirstSibling();
				}
				// Load extra data
				if (File.Exists(Path.Combine(cabinetDataPath,newMetadata.ExtraDataName))){//cabinetDataPath + "\\" + newMetadata.ExtraDataName)) {
					newMetadata._ExtraData = System.IO.File.ReadAllText (Path.Combine(cabinetDataPath,newMetadata.ExtraDataName));
				}
				// Add new metadata to the metadata dictionary.
				gameInfoDataDict[newGuid] = newMetadata;
				// Load new game icon
				if (File.Exists(Path.Combine(cabinetDataPath,newMetadata.IconName))) {
					Sprite newIcon = LoadSprite(Path.Combine(cabinetDataPath,newMetadata.IconName));
					iconDict[newGuid] = newIcon;
					iconTrigger.SetIcon(newIcon);
				}
				else Debug.LogWarning("Failed to load icon at " + (Path.Combine(cabinetDataPath,newMetadata.IconName)));
				// Load new screenshot
				if (File.Exists(Path.Combine(cabinetDataPath,newMetadata.ScreenshotName))) {
					Sprite newScreenshot = LoadSprite(Path.Combine(cabinetDataPath,newMetadata.ScreenshotName));
					screenShotDict[newGuid] = newScreenshot;
				}
				else Debug.LogWarning("Failed to load screenshot at " + (Path.Combine(cabinetDataPath,newMetadata.ScreenshotName)));
			}
			else Debug.LogWarning("Failed to find metadata file at " + metaFilePath);
		}
		// Finally, throw the about icon last in the list.
		GameObject aboutIcon = GameObject.FindGameObjectWithTag("AboutIcon");
		if (aboutIcon != null)
			aboutIcon.transform.SetAsLastSibling();
		UnityEngine.EventSystems.EventSystem.current.SendMessage("SelectByIndex", 0, SendMessageOptions.DontRequireReceiver);
	}
	
	public GameInfoData GetInfo(System.Guid searchGuid) {
		GameInfoData returnInfo = gameInfoDataDict[searchGuid];
		if (returnInfo == null)
			Debug.LogWarning("Failed to retrieve game info in \"GetInfo\".");
		return returnInfo;
	}

	public Sprite GetScreenshot(System.Guid searchGuid) {
		Sprite returnScreenshot = screenShotDict[searchGuid];
		if (returnScreenshot == null)
			Debug.LogWarning("Failed to retrieve screenshot in \"GetScreenshot\".");
		return returnScreenshot;
	}

	public bool ReloadExtraData (System.Guid searchGuid) {
		GameInfoData gameMetadata = gameInfoDataDict[searchGuid];
		if (gameMetadata == null) {
			Debug.LogError("Failed to retrieve game info in \"ReloadExtraData\".");
			return false;
		}
		if (gameMetadata.HasExtraData) {
			string ExtraDataLocation = gameMetadata._GameFolderPath + "\\CabinetData\\" + gameMetadata.ExtraDataName;
			if (File.Exists(ExtraDataLocation)) {
				gameMetadata._ExtraData = System.IO.File.ReadAllText (ExtraDataLocation);
				gameInfoDataDict[searchGuid] = gameMetadata;
				return true;
			}
			else {
				 Debug.LogWarning("A request to reload extra data has failed because the file doesn't exist.");
				 return false;
			}
		}
		return false;
	}
}
