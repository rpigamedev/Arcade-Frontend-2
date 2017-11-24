using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Diagnostics;
using UnityEngine;

public class GameLauncher : MonoBehaviour {

    public static GameLauncher instance
	{
		get {
			if (_instance != null)
				return _instance;
			else return null;
		}
	}

	private static GameLauncher _instance;

    // Settings
    [Header("Settings")]
    [SerializeField]
    bool launchDirectly = false;

    // References
    [Header("References")]
    [SerializeField]
    DisplayManager currentDisplayManager;
    [SerializeField]
    ControlInfoUpdater currentControlInfo;

	// Use this for initialization
	void Awake () {
        if (_instance == null)
            _instance = this;
        
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void requestLaunch(GameInfoData launchInfo) {
        if (currentDisplayManager.currentState != InfoPanelState.ControlsPanel) {
            currentControlInfo.LoadControls(launchInfo);
            currentDisplayManager.SetInfoPanelState(InfoPanelState.ControlsPanel);
        }
        else {
            if (launchDirectly) {
                launchGameDirect(launchInfo, "");
            }
            else {
                launchGameShim(launchInfo, "");
            }
            currentDisplayManager.SetInfoPanelState(InfoPanelState.GamePanel);
        }
    }

    public void cancelLaunch() {
        currentDisplayManager.SetInfoPanelState(InfoPanelState.GamePanel);
    }

	// Function to launch the game directly (no shim, no force quitting)
    void launchGameDirect (GameInfoData launchInfo, string Args) {
        Process launchedGame = new Process();

        // Use to create no window when running from shell
        launchedGame.StartInfo.UseShellExecute = true;
        launchedGame.StartInfo.CreateNoWindow = true;
        launchedGame.StartInfo.WindowStyle = ProcessWindowStyle.Maximized;

        // Set game launch parameters
        launchedGame.StartInfo.WorkingDirectory = Path.GetFullPath(Path.Combine(launchInfo._GameFolderPath,launchInfo.ExeFolder));
        launchedGame.StartInfo.FileName = launchInfo.ExeName;
        launchedGame.StartInfo.Arguments = Args;

		// Start the game
        Application.runInBackground = true;
        launchedGame.Start();

        //UI will halt until game is finished
        launchedGame.WaitForExit();

		// Restore the resolution from before
		Screen.SetResolution (1920, 1200, true);
        UnityEngine.Debug.Log(Screen.width.ToString() + "x" + Screen.height.ToString());

		// Check for new high scores or etc. extra data.
		DataManager.instance.ReloadExtraData(launchInfo._GameGuid);
        DisplayManager.instance.SetInfo(launchInfo._GameGuid, true);
    }

    // Function to launch the game using the AutoHotKey shim (less direct, but allows force quitting)
    void launchGameShim (GameInfoData launchInfo, string Args) {
        int CurrentWidth = Screen.width;
        int CurrentHeight = Screen.height;

        Process launchedGame = new Process();

        // Use to create no window when running from shell
        launchedGame.StartInfo.UseShellExecute = true;
        launchedGame.StartInfo.CreateNoWindow = false;
        launchedGame.StartInfo.WindowStyle = ProcessWindowStyle.Minimized;

		// Set shim launch parameters. Assuming Shim is in frontend build folder. 
		launchedGame.StartInfo.WorkingDirectory = Path.GetFullPath(Path.Combine(Application.dataPath.Replace('/','\\'), @"..\"));
        launchedGame.StartInfo.FileName = "CabinetUIShim.exe";

        // Collect arguments
        // First working directory, then EXE name, then the game's args.
        // Example: "C:\CabinetUI\Data\Games\Super Toast" "SuperToast.exe" -hardmode -decaf
		launchedGame.StartInfo.Arguments = "\"" + Path.GetFullPath(Path.Combine(launchInfo._GameFolderPath,launchInfo.ExeFolder)) + "\" ";
        launchedGame.StartInfo.Arguments += "\"" + launchInfo.ExeName +"\" ";
        launchedGame.StartInfo.Arguments += Args;

        // Start the game
        Application.runInBackground = true;
        launchedGame.Start();

        // UI will halt until shim script is finished
        launchedGame.WaitForExit();

		// Restore the resolution from before.
		Screen.SetResolution (CurrentWidth, CurrentHeight, true);

		// Check for new high scores or etc. extra data.
		DataManager.instance.ReloadExtraData(launchInfo._GameGuid);
        DisplayManager.instance.SetInfo(launchInfo._GameGuid, true);
    }

}
