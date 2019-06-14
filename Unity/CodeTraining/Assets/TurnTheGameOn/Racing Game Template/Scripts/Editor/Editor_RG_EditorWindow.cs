using UnityEngine;
using UnityEditor;
using TurnTheGameOn.RacingGameTemplate;


public class Editor_RG_EditorWindow : EditorWindow {
	public enum Switch { Off, On }

	bool audioSettings;
	bool inputSettings;
	bool raceSettings;
	bool opponentVehicleSettings;
	bool playableVehicleSettings;
	bool playerPrefsSettings;
	bool debugSettings;
	bool showOpponentNames;
	bool showOpponentPrefabs;
	Vector2 scrollPosition = Vector2.zero;
	public AudioClip soundClip;
	Switch configureRaceSize;
	Switch configureCarSize;
	int raceView;
	int carView;
	int opponentCarView;
	int numberOfRaces;
	int numberOfCars;

	bool showAccelerationLevelBonus;
	bool showTopSpeedLevelBonus;
	bool showTireTractionLevelBonus;
	bool showSteerSensitivityLevelBonus;
	bool showBrakePowerLevelBonus;

	GUISkin defaultSkin;

	[MenuItem ("Window/Racing Game Template/RGT Settings")]
	static void Init () {
		Editor_RG_EditorWindow window = (Editor_RG_EditorWindow)EditorWindow.GetWindow (typeof (Editor_RG_EditorWindow));
		Texture icon = Resources.Load("RGTIcon") as Texture;
		GUIContent titleContent = new GUIContent ("RGT Settings", icon);
		window.titleContent = titleContent;
		window.minSize = new Vector2(300f,500f);
		window.Show();
	}

	void DisableAllTabs(){
		audioSettings = false;
		inputSettings = false;
		raceSettings = false;
		opponentVehicleSettings = false;
		playableVehicleSettings = false;
		playerPrefsSettings = false;
		debugSettings = false;
	}

	void OnGUI () {
		if(defaultSkin == null)
			defaultSkin = GUI.skin;
		GUISkin editorSkin = Resources.Load("EditorSkin") as GUISkin;
		GUI.skin = editorSkin;
		EditorGUILayout.BeginVertical("Box");
		EditorGUILayout.LabelField ("Racing Game Template", editorSkin.customStyles [1]);
		EditorGUILayout.LabelField ("Project Editor", editorSkin.customStyles [1]);
		EditorGUILayout.LabelField (RGT_PlayerPrefs.debugData.projectVersion, editorSkin.customStyles [2]);
		EditorGUILayout.BeginHorizontal();
		editorSkin.button.active.textColor = Color.yellow;
		if (audioSettings) {
			editorSkin.button.normal.textColor = Color.yellow;
			editorSkin.button.hover.textColor = Color.yellow;
		}
		else {
			editorSkin.button.normal.textColor = Color.white;
			editorSkin.button.hover.textColor = Color.white;
		}
		if (GUILayout.Button ("Audio", GUILayout.MaxWidth(Screen.width * 0.33f), GUILayout.MaxHeight(35) )) {
			GUIUtility.hotControl = 0;
			GUIUtility.keyboardControl = 0;
			DisableAllTabs ();
			audioSettings = true;
		}
		if (inputSettings) {
			editorSkin.button.normal.textColor = Color.yellow;
			editorSkin.button.hover.textColor = Color.yellow;
		}
		else {
			editorSkin.button.normal.textColor = Color.white;
			editorSkin.button.hover.textColor = Color.white;
		}
		if (GUILayout.Button ("Input", GUILayout.MaxWidth(Screen.width * 0.33f), GUILayout.MaxHeight(35) )) {
			GUIUtility.hotControl = 0;
			GUIUtility.keyboardControl = 0;
			DisableAllTabs ();
			inputSettings = true;
		}
		if (raceSettings) {
			editorSkin.button.normal.textColor = Color.yellow;
			editorSkin.button.hover.textColor = Color.yellow;
		}
		else {
			editorSkin.button.normal.textColor = Color.white;
			editorSkin.button.hover.textColor = Color.white;
		}
		if (GUILayout.Button ("Races", GUILayout.MaxWidth(Screen.width * 0.33f), GUILayout.MaxHeight(35) )) {
			GUIUtility.hotControl = 0;
			GUIUtility.keyboardControl = 0;
			DisableAllTabs ();
			raceSettings = true;
		}
		EditorGUILayout.EndHorizontal ();


		EditorGUILayout.BeginHorizontal ();
		if (playableVehicleSettings) {
			editorSkin.button.normal.textColor = Color.yellow;
			editorSkin.button.hover.textColor = Color.yellow;
		}
		else {
			editorSkin.button.normal.textColor = Color.white;
			editorSkin.button.hover.textColor = Color.white;
		}
		if (GUILayout.Button ("Player Cars", GUILayout.MaxWidth(Screen.width * 0.5f), GUILayout.MaxHeight(40) )) {
			GUIUtility.hotControl = 0;
			GUIUtility.keyboardControl = 0;
			DisableAllTabs ();
			playableVehicleSettings = true;
		}

		if (opponentVehicleSettings) {
			editorSkin.button.normal.textColor = Color.yellow;
			editorSkin.button.hover.textColor = Color.yellow;
		}
		else {
			editorSkin.button.normal.textColor = Color.white;
			editorSkin.button.hover.textColor = Color.white;
		}
		if (GUILayout.Button ("AI Cars", GUILayout.MaxWidth(Screen.width * 0.5f), GUILayout.MaxHeight(40) )) {
			GUIUtility.hotControl = 0;
			GUIUtility.keyboardControl = 0;
			DisableAllTabs ();
			opponentVehicleSettings = true;
		}
		EditorGUILayout.EndHorizontal ();



		EditorGUILayout.BeginHorizontal ();
		if (playerPrefsSettings) {
			editorSkin.button.normal.textColor = Color.yellow;
			editorSkin.button.hover.textColor = Color.yellow;
		}
		else {
			editorSkin.button.normal.textColor = Color.white;
			editorSkin.button.hover.textColor = Color.white;
		}
		if (GUILayout.Button ("PlayerPrefs", GUILayout.MaxWidth(Screen.width * 0.5f), GUILayout.MaxHeight(35) )) {
			GUIUtility.hotControl = 0;
			GUIUtility.keyboardControl = 0;
			DisableAllTabs ();
			playerPrefsSettings = true;
		}

		if (debugSettings) {
			editorSkin.button.normal.textColor = Color.yellow;
			editorSkin.button.hover.textColor = Color.yellow;
		}
		else {
			editorSkin.button.normal.textColor = Color.white;
			editorSkin.button.hover.textColor = Color.white;
		}
		if (GUILayout.Button ("Debug Settings", GUILayout.MaxWidth(Screen.width * 0.5f), GUILayout.MaxHeight(35) )) {
			GUIUtility.hotControl = 0;
			GUIUtility.keyboardControl = 0;
			DisableAllTabs ();
			debugSettings = true;
		}
		EditorGUILayout.EndHorizontal ();


		editorSkin.button.normal.textColor = Color.white;
		editorSkin.button.hover.textColor = Color.white;


		editorSkin.button.normal.textColor = Color.grey;


		EditorGUILayout.LabelField ("Unity Project Settings:");

		EditorGUILayout.BeginHorizontal ();
		if (GUILayout.Button ("Build Settings", GUILayout.MaxWidth (Screen.width * 0.5f), GUILayout.MaxHeight (20))) {
			GUIUtility.hotControl = 0;
			GUIUtility.keyboardControl = 0;
			EditorWindow.GetWindow (System.Type.GetType("UnityEditor.BuildPlayerWindow,UnityEditor"));
		}
		if (GUILayout.Button ("Services", GUILayout.MaxWidth (Screen.width * 0.5f), GUILayout.MaxHeight (20))) {
			GUIUtility.hotControl = 0;
			GUIUtility.keyboardControl = 0;
			EditorApplication.ExecuteMenuItem ("Window/Services");
		}
		if (GUILayout.Button ("Lighting", GUILayout.MaxWidth (Screen.width * 0.5f), GUILayout.MaxHeight (20))) {
			GUIUtility.hotControl = 0;
			GUIUtility.keyboardControl = 0;
			EditorApplication.ExecuteMenuItem ("Window/Lighting");
		}
		EditorGUILayout.EndHorizontal ();

		EditorGUILayout.BeginHorizontal ();
		if (GUILayout.Button ("Physics", GUILayout.MaxWidth (Screen.width * 0.2f), GUILayout.MaxHeight (20))) {
			GUIUtility.hotControl = 0;
			GUIUtility.keyboardControl = 0;
			EditorApplication.ExecuteMenuItem ("Edit/Project Settings/Physics");
		}
		if (GUILayout.Button ("Quality", GUILayout.MaxWidth (Screen.width * 0.2f), GUILayout.MaxHeight (20))) {
			GUIUtility.hotControl = 0;
			GUIUtility.keyboardControl = 0;
			EditorApplication.ExecuteMenuItem ("Edit/Project Settings/Quality");
		}
		if (GUILayout.Button ("Graphics", GUILayout.MaxWidth (Screen.width * 0.2f), GUILayout.MaxHeight (20))) {
			GUIUtility.hotControl = 0;
			GUIUtility.keyboardControl = 0;
			EditorApplication.ExecuteMenuItem ("Edit/Project Settings/Graphics");
		}
		if (GUILayout.Button ("Tags and Layers", GUILayout.MaxWidth (Screen.width * 0.35f), GUILayout.MaxHeight (20))) {
			GUIUtility.hotControl = 0;
			GUIUtility.keyboardControl = 0;
			EditorApplication.ExecuteMenuItem ("Edit/Project Settings/Tags and Layers");
		}
		EditorGUILayout.EndHorizontal ();

		EditorGUILayout.BeginHorizontal ();
		if (GUILayout.Button ("Editor", GUILayout.MaxWidth (Screen.width * 0.5f), GUILayout.MaxHeight (20))) {
			GUIUtility.hotControl = 0;
			GUIUtility.keyboardControl = 0;
			EditorApplication.ExecuteMenuItem ("Edit/Project Settings/Editor");
		}
		if (GUILayout.Button ("Player", GUILayout.MaxWidth (Screen.width * 0.5f), GUILayout.MaxHeight (20))) {
			GUIUtility.hotControl = 0;
			GUIUtility.keyboardControl = 0;
			EditorApplication.ExecuteMenuItem ("Edit/Project Settings/Player");
		}
		if (GUILayout.Button ("Script Execution Order", GUILayout.MaxWidth (Screen.width * 0.5f), GUILayout.MaxHeight (20))) {
			GUIUtility.hotControl = 0;
			GUIUtility.keyboardControl = 0;
			EditorApplication.ExecuteMenuItem ("Edit/Project Settings/Script Execution Order");
		}
		EditorGUILayout.EndHorizontal ();
		EditorGUILayout.BeginHorizontal ();


		if (GUILayout.Button ("Audio", GUILayout.MaxWidth (Screen.width * 0.5f), GUILayout.MaxHeight (20))) {
			GUIUtility.hotControl = 0;
			GUIUtility.keyboardControl = 0;
			EditorApplication.ExecuteMenuItem ("Edit/Project Settings/Audio");
		}
		if (GUILayout.Button ("Time", GUILayout.MaxWidth (Screen.width * 0.5f), GUILayout.MaxHeight (20))) {
			GUIUtility.hotControl = 0;
			GUIUtility.keyboardControl = 0;
			EditorApplication.ExecuteMenuItem ("Edit/Project Settings/Time");
		}
		if (GUILayout.Button ("Network", GUILayout.MaxWidth (Screen.width * 0.5f), GUILayout.MaxHeight (20))) {
			GUIUtility.hotControl = 0;
			GUIUtility.keyboardControl = 0;
			EditorApplication.ExecuteMenuItem ("Edit/Project Settings/Network");
		}
		EditorGUILayout.EndHorizontal ();
		editorSkin.button.normal.textColor = Color.white;
		EditorGUILayout.EndVertical();
		scrollPosition = EditorGUILayout.BeginScrollView (scrollPosition, false, false);
/// Audio Settings
		if(audioSettings){
			EditorGUILayout.BeginVertical("Box");
			EditorGUILayout.LabelField ("Audio Settings", editorSkin.customStyles [0]);
			EditorGUILayout.EndVertical();
			RGT_PlayerPrefs.audioData.musicSelection = (AudioData.MusicSelection) EditorGUILayout.EnumPopup ("Music Selection", RGT_PlayerPrefs.audioData.musicSelection);
			RGT_PlayerPrefs.audioData.numberOfTracks = EditorGUILayout.IntSlider ("Number of Tracks", RGT_PlayerPrefs.audioData.numberOfTracks, 0, 50);
			if(RGT_PlayerPrefs.audioData.numberOfTracks != RGT_PlayerPrefs.audioData.music.Length){
				System.Array.Resize (ref RGT_PlayerPrefs.audioData.music, RGT_PlayerPrefs.audioData.numberOfTracks);
			}
			EditorGUILayout.BeginVertical("Box");
			EditorGUILayout.LabelField ("Music Tracks", editorSkin.customStyles [0]);
			EditorGUILayout.EndVertical();
			for(int i = 0; i < RGT_PlayerPrefs.audioData.music.Length; i++){
				RGT_PlayerPrefs.audioData.music[i] = (AudioClip) EditorGUILayout.ObjectField ("Track " + i, RGT_PlayerPrefs.audioData.music[i], typeof(AudioClip), false);
			}
			EditorGUILayout.BeginVertical("Box");
			EditorGUILayout.LabelField ("UI Interaction Sounds", editorSkin.customStyles [0]);
			EditorGUILayout.EndVertical();
			RGT_PlayerPrefs.audioData.menuBack = (AudioClip) EditorGUILayout.ObjectField ("Back", RGT_PlayerPrefs.audioData.menuBack, typeof(AudioClip), false);
			RGT_PlayerPrefs.audioData.menuSelect = (AudioClip) EditorGUILayout.ObjectField ("Confirm", RGT_PlayerPrefs.audioData.menuSelect, typeof(AudioClip), false);
			EditorUtility.SetDirty (RGT_PlayerPrefs.audioData);
		}
/// Input Settings
		if(inputSettings){
			EditorGUILayout.BeginVertical("Box");
			EditorGUILayout.LabelField ("Input Manager Axes Settings", editorSkin.customStyles [0]);
			EditorGUILayout.EndVertical();
			SerializedObject serializedObject = new SerializedObject (AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/InputManager.asset")[0]);

			SerializedProperty axisProperty = serializedObject.FindProperty ("m_Axes");
			EditorGUI.BeginChangeCheck ();
			EditorGUILayout.PropertyField (axisProperty, true);
			if (EditorGUI.EndChangeCheck ())
				serializedObject.ApplyModifiedProperties ();
			EditorGUILayout.BeginVertical("Box");
			EditorGUILayout.LabelField ("Keyboard Input Settings", editorSkin.customStyles [0]);
			EditorGUILayout.EndVertical();

			RGT_PlayerPrefs.inputData.pauseKey = (KeyCode)EditorGUILayout.EnumPopup ("Pause", RGT_PlayerPrefs.inputData.pauseKey);
			RGT_PlayerPrefs.inputData.cameraSwitchKey = (KeyCode)EditorGUILayout.EnumPopup ("Camera Switch", RGT_PlayerPrefs.inputData.cameraSwitchKey);
			RGT_PlayerPrefs.inputData.nitroKey = (KeyCode)EditorGUILayout.EnumPopup ("Nitro", RGT_PlayerPrefs.inputData.nitroKey);
			RGT_PlayerPrefs.inputData.shiftUp = (KeyCode)EditorGUILayout.EnumPopup ("Shift Up", RGT_PlayerPrefs.inputData.shiftUp);
			RGT_PlayerPrefs.inputData.shiftDown = (KeyCode)EditorGUILayout.EnumPopup ("Shift Down", RGT_PlayerPrefs.inputData.shiftDown);
			RGT_PlayerPrefs.inputData.lookBack = (KeyCode)EditorGUILayout.EnumPopup ("Look Back", RGT_PlayerPrefs.inputData.lookBack);

			EditorGUILayout.BeginVertical("Box");
			EditorGUILayout.LabelField ("Joystick Input Settings", editorSkin.customStyles [0]);
			EditorGUILayout.EndVertical();

			RGT_PlayerPrefs.inputData._pauseJoystick = (InputData.Joystick)EditorGUILayout.EnumPopup ("Pause", RGT_PlayerPrefs.inputData._pauseJoystick);
			switch(RGT_PlayerPrefs.inputData._pauseJoystick){
			case InputData.Joystick.None:
				RGT_PlayerPrefs.inputData.pauseJoystick = KeyCode.None;
				break;
			case InputData.Joystick.JoystickButton0:
				RGT_PlayerPrefs.inputData.pauseJoystick = KeyCode.JoystickButton0; 
				break;
			case InputData.Joystick.JoystickButton1:
				RGT_PlayerPrefs.inputData.pauseJoystick = KeyCode.JoystickButton1; 
				break;
			case InputData.Joystick.JoystickButton2:
				RGT_PlayerPrefs.inputData.pauseJoystick = KeyCode.JoystickButton2; 
				break;
			case InputData.Joystick.JoystickButton3:
				RGT_PlayerPrefs.inputData.pauseJoystick = KeyCode.JoystickButton3; 
				break;
			case InputData.Joystick.JoystickButton4:
				RGT_PlayerPrefs.inputData.pauseJoystick = KeyCode.JoystickButton4; 
				break;
			case InputData.Joystick.JoystickButton5:
				RGT_PlayerPrefs.inputData.pauseJoystick = KeyCode.JoystickButton5; 
				break;
			case InputData.Joystick.JoystickButton6:
				RGT_PlayerPrefs.inputData.pauseJoystick = KeyCode.JoystickButton6; 
				break;
			case InputData.Joystick.JoystickButton7:
				RGT_PlayerPrefs.inputData.pauseJoystick = KeyCode.JoystickButton7; 
				break;
			case InputData.Joystick.JoystickButton8:
				RGT_PlayerPrefs.inputData.pauseJoystick = KeyCode.JoystickButton8; 
				break;
			case InputData.Joystick.JoystickButton9:
				RGT_PlayerPrefs.inputData.pauseJoystick = KeyCode.JoystickButton9; 
				break;
			case InputData.Joystick.JoystickButton10:
				RGT_PlayerPrefs.inputData.pauseJoystick = KeyCode.JoystickButton10; 
				break;
			case InputData.Joystick.JoystickButton11:
				RGT_PlayerPrefs.inputData.pauseJoystick = KeyCode.JoystickButton11; 
				break;
			case InputData.Joystick.JoystickButton12:
				RGT_PlayerPrefs.inputData.pauseJoystick = KeyCode.JoystickButton12; 
				break;
			case InputData.Joystick.JoystickButton13:
				RGT_PlayerPrefs.inputData.pauseJoystick = KeyCode.JoystickButton13; 
				break;
			case InputData.Joystick.JoystickButton14:
				RGT_PlayerPrefs.inputData.pauseJoystick = KeyCode.JoystickButton14; 
				break;
			case InputData.Joystick.JoystickButton15:
				RGT_PlayerPrefs.inputData.pauseJoystick = KeyCode.JoystickButton15; 
				break;
			case InputData.Joystick.JoystickButton16:
				RGT_PlayerPrefs.inputData.pauseJoystick = KeyCode.JoystickButton16; 
				break;
			case InputData.Joystick.JoystickButton17:
				RGT_PlayerPrefs.inputData.pauseJoystick = KeyCode.JoystickButton17; 
				break;
			case InputData.Joystick.JoystickButton18:
				RGT_PlayerPrefs.inputData.pauseJoystick = KeyCode.JoystickButton18; 
				break;
			case InputData.Joystick.JoystickButton19:
				RGT_PlayerPrefs.inputData.pauseJoystick = KeyCode.JoystickButton19;
				break;
			}
			RGT_PlayerPrefs.inputData._cameraSwitchJoystick = (InputData.Joystick)EditorGUILayout.EnumPopup ("Camera Switch", RGT_PlayerPrefs.inputData._cameraSwitchJoystick);
			switch(RGT_PlayerPrefs.inputData._cameraSwitchJoystick){
			case InputData.Joystick.None:
				RGT_PlayerPrefs.inputData.cameraSwitchJoystick = KeyCode.None;
				break;
			case InputData.Joystick.JoystickButton0:
				RGT_PlayerPrefs.inputData.cameraSwitchJoystick = KeyCode.JoystickButton0; 
				break;
			case InputData.Joystick.JoystickButton1:
				RGT_PlayerPrefs.inputData.cameraSwitchJoystick = KeyCode.JoystickButton1; 
				break;
			case InputData.Joystick.JoystickButton2:
				RGT_PlayerPrefs.inputData.cameraSwitchJoystick = KeyCode.JoystickButton2; 
				break;
			case InputData.Joystick.JoystickButton3:
				RGT_PlayerPrefs.inputData.cameraSwitchJoystick = KeyCode.JoystickButton3; 
				break;
			case InputData.Joystick.JoystickButton4:
				RGT_PlayerPrefs.inputData.cameraSwitchJoystick = KeyCode.JoystickButton4; 
				break;
			case InputData.Joystick.JoystickButton5:
				RGT_PlayerPrefs.inputData.cameraSwitchJoystick = KeyCode.JoystickButton5; 
				break;
			case InputData.Joystick.JoystickButton6:
				RGT_PlayerPrefs.inputData.cameraSwitchJoystick = KeyCode.JoystickButton6; 
				break;
			case InputData.Joystick.JoystickButton7:
				RGT_PlayerPrefs.inputData.cameraSwitchJoystick = KeyCode.JoystickButton7; 
				break;
			case InputData.Joystick.JoystickButton8:
				RGT_PlayerPrefs.inputData.cameraSwitchJoystick = KeyCode.JoystickButton8; 
				break;
			case InputData.Joystick.JoystickButton9:
				RGT_PlayerPrefs.inputData.cameraSwitchJoystick = KeyCode.JoystickButton9; 
				break;
			case InputData.Joystick.JoystickButton10:
				RGT_PlayerPrefs.inputData.cameraSwitchJoystick = KeyCode.JoystickButton10; 
				break;
			case InputData.Joystick.JoystickButton11:
				RGT_PlayerPrefs.inputData.cameraSwitchJoystick = KeyCode.JoystickButton11; 
				break;
			case InputData.Joystick.JoystickButton12:
				RGT_PlayerPrefs.inputData.cameraSwitchJoystick = KeyCode.JoystickButton12; 
				break;
			case InputData.Joystick.JoystickButton13:
				RGT_PlayerPrefs.inputData.cameraSwitchJoystick = KeyCode.JoystickButton13; 
				break;
			case InputData.Joystick.JoystickButton14:
				RGT_PlayerPrefs.inputData.cameraSwitchJoystick = KeyCode.JoystickButton14; 
				break;
			case InputData.Joystick.JoystickButton15:
				RGT_PlayerPrefs.inputData.cameraSwitchJoystick = KeyCode.JoystickButton15; 
				break;
			case InputData.Joystick.JoystickButton16:
				RGT_PlayerPrefs.inputData.cameraSwitchJoystick = KeyCode.JoystickButton16; 
				break;
			case InputData.Joystick.JoystickButton17:
				RGT_PlayerPrefs.inputData.cameraSwitchJoystick = KeyCode.JoystickButton17; 
				break;
			case InputData.Joystick.JoystickButton18:
				RGT_PlayerPrefs.inputData.cameraSwitchJoystick = KeyCode.JoystickButton18; 
				break;
			case InputData.Joystick.JoystickButton19:
				RGT_PlayerPrefs.inputData.cameraSwitchJoystick = KeyCode.JoystickButton19;
				break;
			}
			RGT_PlayerPrefs.inputData._nitroJoystick = (InputData.Joystick)EditorGUILayout.EnumPopup ("Nitro", RGT_PlayerPrefs.inputData._nitroJoystick);
			switch(RGT_PlayerPrefs.inputData._nitroJoystick){
			case InputData.Joystick.None:
				RGT_PlayerPrefs.inputData.nitroJoystick = KeyCode.None;
				break;
			case InputData.Joystick.JoystickButton0:
				RGT_PlayerPrefs.inputData.nitroJoystick = KeyCode.JoystickButton0; 
				break;
			case InputData.Joystick.JoystickButton1:
				RGT_PlayerPrefs.inputData.nitroJoystick = KeyCode.JoystickButton1; 
				break;
			case InputData.Joystick.JoystickButton2:
				RGT_PlayerPrefs.inputData.nitroJoystick = KeyCode.JoystickButton2; 
				break;
			case InputData.Joystick.JoystickButton3:
				RGT_PlayerPrefs.inputData.nitroJoystick = KeyCode.JoystickButton3; 
				break;
			case InputData.Joystick.JoystickButton4:
				RGT_PlayerPrefs.inputData.nitroJoystick = KeyCode.JoystickButton4; 
				break;
			case InputData.Joystick.JoystickButton5:
				RGT_PlayerPrefs.inputData.nitroJoystick = KeyCode.JoystickButton5; 
				break;
			case InputData.Joystick.JoystickButton6:
				RGT_PlayerPrefs.inputData.nitroJoystick = KeyCode.JoystickButton6; 
				break;
			case InputData.Joystick.JoystickButton7:
				RGT_PlayerPrefs.inputData.nitroJoystick = KeyCode.JoystickButton7; 
				break;
			case InputData.Joystick.JoystickButton8:
				RGT_PlayerPrefs.inputData.nitroJoystick = KeyCode.JoystickButton8; 
				break;
			case InputData.Joystick.JoystickButton9:
				RGT_PlayerPrefs.inputData.nitroJoystick = KeyCode.JoystickButton9; 
				break;
			case InputData.Joystick.JoystickButton10:
				RGT_PlayerPrefs.inputData.nitroJoystick = KeyCode.JoystickButton10; 
				break;
			case InputData.Joystick.JoystickButton11:
				RGT_PlayerPrefs.inputData.nitroJoystick = KeyCode.JoystickButton11; 
				break;
			case InputData.Joystick.JoystickButton12:
				RGT_PlayerPrefs.inputData.nitroJoystick = KeyCode.JoystickButton12; 
				break;
			case InputData.Joystick.JoystickButton13:
				RGT_PlayerPrefs.inputData.nitroJoystick = KeyCode.JoystickButton13; 
				break;
			case InputData.Joystick.JoystickButton14:
				RGT_PlayerPrefs.inputData.nitroJoystick = KeyCode.JoystickButton14; 
				break;
			case InputData.Joystick.JoystickButton15:
				RGT_PlayerPrefs.inputData.nitroJoystick = KeyCode.JoystickButton15; 
				break;
			case InputData.Joystick.JoystickButton16:
				RGT_PlayerPrefs.inputData.nitroJoystick = KeyCode.JoystickButton16; 
				break;
			case InputData.Joystick.JoystickButton17:
				RGT_PlayerPrefs.inputData.nitroJoystick = KeyCode.JoystickButton17; 
				break;
			case InputData.Joystick.JoystickButton18:
				RGT_PlayerPrefs.inputData.nitroJoystick = KeyCode.JoystickButton18; 
				break;
			case InputData.Joystick.JoystickButton19:
				RGT_PlayerPrefs.inputData.nitroJoystick = KeyCode.JoystickButton19;
				break;
			}
			RGT_PlayerPrefs.inputData._shiftUpJoystick = (InputData.Joystick)EditorGUILayout.EnumPopup ("Shift Up", RGT_PlayerPrefs.inputData._shiftUpJoystick);
			switch(RGT_PlayerPrefs.inputData._shiftUpJoystick){
			case InputData.Joystick.None:
				RGT_PlayerPrefs.inputData.shiftUpJoystick = KeyCode.None;
				break;
			case InputData.Joystick.JoystickButton0:
				RGT_PlayerPrefs.inputData.shiftUpJoystick = KeyCode.JoystickButton0; 
				break;
			case InputData.Joystick.JoystickButton1:
				RGT_PlayerPrefs.inputData.shiftUpJoystick = KeyCode.JoystickButton1; 
				break;
			case InputData.Joystick.JoystickButton2:
				RGT_PlayerPrefs.inputData.shiftUpJoystick = KeyCode.JoystickButton2; 
				break;
			case InputData.Joystick.JoystickButton3:
				RGT_PlayerPrefs.inputData.shiftUpJoystick = KeyCode.JoystickButton3; 
				break;
			case InputData.Joystick.JoystickButton4:
				RGT_PlayerPrefs.inputData.shiftUpJoystick = KeyCode.JoystickButton4; 
				break;
			case InputData.Joystick.JoystickButton5:
				RGT_PlayerPrefs.inputData.shiftUpJoystick = KeyCode.JoystickButton5; 
				break;
			case InputData.Joystick.JoystickButton6:
				RGT_PlayerPrefs.inputData.shiftUpJoystick = KeyCode.JoystickButton6; 
				break;
			case InputData.Joystick.JoystickButton7:
				RGT_PlayerPrefs.inputData.shiftUpJoystick = KeyCode.JoystickButton7; 
				break;
			case InputData.Joystick.JoystickButton8:
				RGT_PlayerPrefs.inputData.shiftUpJoystick = KeyCode.JoystickButton8; 
				break;
			case InputData.Joystick.JoystickButton9:
				RGT_PlayerPrefs.inputData.shiftUpJoystick = KeyCode.JoystickButton9; 
				break;
			case InputData.Joystick.JoystickButton10:
				RGT_PlayerPrefs.inputData.shiftUpJoystick = KeyCode.JoystickButton10; 
				break;
			case InputData.Joystick.JoystickButton11:
				RGT_PlayerPrefs.inputData.shiftUpJoystick = KeyCode.JoystickButton11; 
				break;
			case InputData.Joystick.JoystickButton12:
				RGT_PlayerPrefs.inputData.shiftUpJoystick = KeyCode.JoystickButton12; 
				break;
			case InputData.Joystick.JoystickButton13:
				RGT_PlayerPrefs.inputData.shiftUpJoystick = KeyCode.JoystickButton13; 
				break;
			case InputData.Joystick.JoystickButton14:
				RGT_PlayerPrefs.inputData.shiftUpJoystick = KeyCode.JoystickButton14; 
				break;
			case InputData.Joystick.JoystickButton15:
				RGT_PlayerPrefs.inputData.shiftUpJoystick = KeyCode.JoystickButton15; 
				break;
			case InputData.Joystick.JoystickButton16:
				RGT_PlayerPrefs.inputData.shiftUpJoystick = KeyCode.JoystickButton16; 
				break;
			case InputData.Joystick.JoystickButton17:
				RGT_PlayerPrefs.inputData.shiftUpJoystick = KeyCode.JoystickButton17; 
				break;
			case InputData.Joystick.JoystickButton18:
				RGT_PlayerPrefs.inputData.shiftUpJoystick = KeyCode.JoystickButton18; 
				break;
			case InputData.Joystick.JoystickButton19:
				RGT_PlayerPrefs.inputData.shiftUpJoystick = KeyCode.JoystickButton19;
				break;
			}
			RGT_PlayerPrefs.inputData._shiftDownJoystick = (InputData.Joystick)EditorGUILayout.EnumPopup ("Shift Down", RGT_PlayerPrefs.inputData._shiftDownJoystick);
			switch(RGT_PlayerPrefs.inputData._shiftDownJoystick){
			case InputData.Joystick.None:
				RGT_PlayerPrefs.inputData.shiftDownJoystick = KeyCode.None;
				break;
			case InputData.Joystick.JoystickButton0:
				RGT_PlayerPrefs.inputData.shiftDownJoystick = KeyCode.JoystickButton0; 
				break;
			case InputData.Joystick.JoystickButton1:
				RGT_PlayerPrefs.inputData.shiftDownJoystick = KeyCode.JoystickButton1; 
				break;
			case InputData.Joystick.JoystickButton2:
				RGT_PlayerPrefs.inputData.shiftDownJoystick = KeyCode.JoystickButton2; 
				break;
			case InputData.Joystick.JoystickButton3:
				RGT_PlayerPrefs.inputData.shiftDownJoystick = KeyCode.JoystickButton3; 
				break;
			case InputData.Joystick.JoystickButton4:
				RGT_PlayerPrefs.inputData.shiftDownJoystick = KeyCode.JoystickButton4; 
				break;
			case InputData.Joystick.JoystickButton5:
				RGT_PlayerPrefs.inputData.shiftDownJoystick = KeyCode.JoystickButton5; 
				break;
			case InputData.Joystick.JoystickButton6:
				RGT_PlayerPrefs.inputData.shiftDownJoystick = KeyCode.JoystickButton6; 
				break;
			case InputData.Joystick.JoystickButton7:
				RGT_PlayerPrefs.inputData.shiftDownJoystick = KeyCode.JoystickButton7; 
				break;
			case InputData.Joystick.JoystickButton8:
				RGT_PlayerPrefs.inputData.shiftDownJoystick = KeyCode.JoystickButton8; 
				break;
			case InputData.Joystick.JoystickButton9:
				RGT_PlayerPrefs.inputData.shiftDownJoystick = KeyCode.JoystickButton9; 
				break;
			case InputData.Joystick.JoystickButton10:
				RGT_PlayerPrefs.inputData.shiftDownJoystick = KeyCode.JoystickButton10; 
				break;
			case InputData.Joystick.JoystickButton11:
				RGT_PlayerPrefs.inputData.shiftDownJoystick = KeyCode.JoystickButton11; 
				break;
			case InputData.Joystick.JoystickButton12:
				RGT_PlayerPrefs.inputData.shiftDownJoystick = KeyCode.JoystickButton12; 
				break;
			case InputData.Joystick.JoystickButton13:
				RGT_PlayerPrefs.inputData.shiftDownJoystick = KeyCode.JoystickButton13; 
				break;
			case InputData.Joystick.JoystickButton14:
				RGT_PlayerPrefs.inputData.shiftDownJoystick = KeyCode.JoystickButton14; 
				break;
			case InputData.Joystick.JoystickButton15:
				RGT_PlayerPrefs.inputData.shiftDownJoystick = KeyCode.JoystickButton15; 
				break;
			case InputData.Joystick.JoystickButton16:
				RGT_PlayerPrefs.inputData.shiftDownJoystick = KeyCode.JoystickButton16; 
				break;
			case InputData.Joystick.JoystickButton17:
				RGT_PlayerPrefs.inputData.shiftDownJoystick = KeyCode.JoystickButton17; 
				break;
			case InputData.Joystick.JoystickButton18:
				RGT_PlayerPrefs.inputData.shiftDownJoystick = KeyCode.JoystickButton18; 
				break;
			case InputData.Joystick.JoystickButton19:
				RGT_PlayerPrefs.inputData.shiftDownJoystick = KeyCode.JoystickButton19;
				break;
			}
			RGT_PlayerPrefs.inputData._lookBackJoystick = (InputData.Joystick)EditorGUILayout.EnumPopup ("Look Back", RGT_PlayerPrefs.inputData._lookBackJoystick);
			switch(RGT_PlayerPrefs.inputData._lookBackJoystick){
			case InputData.Joystick.None:
				RGT_PlayerPrefs.inputData.lookBackJoystick = KeyCode.None;
				break;
			case InputData.Joystick.JoystickButton0:
				RGT_PlayerPrefs.inputData.lookBackJoystick = KeyCode.JoystickButton0; 
				break;
			case InputData.Joystick.JoystickButton1:
				RGT_PlayerPrefs.inputData.lookBackJoystick = KeyCode.JoystickButton1; 
				break;
			case InputData.Joystick.JoystickButton2:
				RGT_PlayerPrefs.inputData.lookBackJoystick = KeyCode.JoystickButton2; 
				break;
			case InputData.Joystick.JoystickButton3:
				RGT_PlayerPrefs.inputData.lookBackJoystick = KeyCode.JoystickButton3; 
				break;
			case InputData.Joystick.JoystickButton4:
				RGT_PlayerPrefs.inputData.lookBackJoystick = KeyCode.JoystickButton4; 
				break;
			case InputData.Joystick.JoystickButton5:
				RGT_PlayerPrefs.inputData.lookBackJoystick = KeyCode.JoystickButton5; 
				break;
			case InputData.Joystick.JoystickButton6:
				RGT_PlayerPrefs.inputData.lookBackJoystick = KeyCode.JoystickButton6; 
				break;
			case InputData.Joystick.JoystickButton7:
				RGT_PlayerPrefs.inputData.lookBackJoystick = KeyCode.JoystickButton7; 
				break;
			case InputData.Joystick.JoystickButton8:
				RGT_PlayerPrefs.inputData.lookBackJoystick = KeyCode.JoystickButton8; 
				break;
			case InputData.Joystick.JoystickButton9:
				RGT_PlayerPrefs.inputData.lookBackJoystick = KeyCode.JoystickButton9; 
				break;
			case InputData.Joystick.JoystickButton10:
				RGT_PlayerPrefs.inputData.lookBackJoystick = KeyCode.JoystickButton10; 
				break;
			case InputData.Joystick.JoystickButton11:
				RGT_PlayerPrefs.inputData.lookBackJoystick = KeyCode.JoystickButton11; 
				break;
			case InputData.Joystick.JoystickButton12:
				RGT_PlayerPrefs.inputData.lookBackJoystick = KeyCode.JoystickButton12; 
				break;
			case InputData.Joystick.JoystickButton13:
				RGT_PlayerPrefs.inputData.lookBackJoystick = KeyCode.JoystickButton13; 
				break;
			case InputData.Joystick.JoystickButton14:
				RGT_PlayerPrefs.inputData.lookBackJoystick = KeyCode.JoystickButton14; 
				break;
			case InputData.Joystick.JoystickButton15:
				RGT_PlayerPrefs.inputData.lookBackJoystick = KeyCode.JoystickButton15; 
				break;
			case InputData.Joystick.JoystickButton16:
				RGT_PlayerPrefs.inputData.lookBackJoystick = KeyCode.JoystickButton16; 
				break;
			case InputData.Joystick.JoystickButton17:
				RGT_PlayerPrefs.inputData.lookBackJoystick = KeyCode.JoystickButton17; 
				break;
			case InputData.Joystick.JoystickButton18:
				RGT_PlayerPrefs.inputData.lookBackJoystick = KeyCode.JoystickButton18; 
				break;
			case InputData.Joystick.JoystickButton19:
				RGT_PlayerPrefs.inputData.lookBackJoystick = KeyCode.JoystickButton19;
				break;
			}

			EditorGUILayout.BeginVertical("Box");
			EditorGUILayout.LabelField ("Mobile Settings", editorSkin.customStyles [0]);
			EditorGUILayout.EndVertical();
			RGT_PlayerPrefs.inputData.useMobileController = EditorGUILayout.Toggle("Use Mobile Controller", RGT_PlayerPrefs.inputData.useMobileController);
			RGT_PlayerPrefs.inputData.mobileController = (GameObject) EditorGUILayout.ObjectField("Prefab", RGT_PlayerPrefs.inputData.mobileController, typeof (GameObject), false );

			EditorUtility.SetDirty (RGT_PlayerPrefs.inputData);
		}
/// Race Settings
		if(raceSettings){
			EditorGUILayout.BeginVertical("Box");
			EditorGUILayout.LabelField ("Race Settings", editorSkin.customStyles [0]);
			EditorGUILayout.EndVertical();

			configureRaceSize = (Switch) EditorGUILayout.EnumPopup ("Configure Race Size", configureRaceSize);
			if (configureRaceSize == Switch.On) {
				EditorGUILayout.HelpBox ("When you reduce this number the values of the affected arrays are deleted. Only reduce this number if you want fewer races.", MessageType.Warning);
				EditorGUILayout.BeginHorizontal ();
				numberOfRaces = EditorGUILayout.IntField ("Number Of Races", numberOfRaces);
				if (GUILayout.Button ("Update")) {
					GUIUtility.hotControl = 0;
					GUIUtility.keyboardControl = 0;
					RGT_PlayerPrefs.raceData.numberOfRaces = numberOfRaces;
					raceView = 0;
				}
				EditorGUILayout.EndHorizontal ();
				System.Array.Resize (ref RGT_PlayerPrefs.raceData.raceNames, RGT_PlayerPrefs.raceData.numberOfRaces);
				System.Array.Resize (ref RGT_PlayerPrefs.raceData.raceImages, RGT_PlayerPrefs.raceData.numberOfRaces);
				System.Array.Resize (ref RGT_PlayerPrefs.raceData.gameType, RGT_PlayerPrefs.raceData.numberOfRaces);
				System.Array.Resize (ref RGT_PlayerPrefs.raceData.numberOfRacers, RGT_PlayerPrefs.raceData.numberOfRaces);
				System.Array.Resize (ref RGT_PlayerPrefs.raceData.raceLaps, RGT_PlayerPrefs.raceData.numberOfRaces);
				System.Array.Resize (ref RGT_PlayerPrefs.raceData.lapLimit, RGT_PlayerPrefs.raceData.numberOfRaces);
				System.Array.Resize (ref RGT_PlayerPrefs.raceData.racerLimit, RGT_PlayerPrefs.raceData.numberOfRaces);
				System.Array.Resize (ref RGT_PlayerPrefs.raceData.raceLocked, RGT_PlayerPrefs.raceData.numberOfRaces);
				System.Array.Resize (ref RGT_PlayerPrefs.raceData.unlockAmount, RGT_PlayerPrefs.raceData.numberOfRaces);
				System.Array.Resize (ref RGT_PlayerPrefs.raceData.firstPrize, RGT_PlayerPrefs.raceData.numberOfRaces);
				System.Array.Resize (ref RGT_PlayerPrefs.raceData.secondPrize, RGT_PlayerPrefs.raceData.numberOfRaces);
				System.Array.Resize (ref RGT_PlayerPrefs.raceData.thirdPrize, RGT_PlayerPrefs.raceData.numberOfRaces);
				System.Array.Resize (ref RGT_PlayerPrefs.raceData.readyTime, RGT_PlayerPrefs.raceData.numberOfRaces);
				System.Array.Resize (ref RGT_PlayerPrefs.raceData.extraLapRewardMultiplier, RGT_PlayerPrefs.raceData.numberOfRaces);
				System.Array.Resize (ref RGT_PlayerPrefs.raceData.extraRacerRewardMultiplier, RGT_PlayerPrefs.raceData.numberOfRaces);
				System.Array.Resize (ref RGT_PlayerPrefs.raceData.showWaypoints, RGT_PlayerPrefs.raceData.numberOfRaces);
				System.Array.Resize (ref RGT_PlayerPrefs.raceData.showWaypointArrow, RGT_PlayerPrefs.raceData.numberOfRaces);

				System.Array.Resize (ref RGT_PlayerPrefs.raceData.unlimitedRewards, RGT_PlayerPrefs.raceData.numberOfRaces);
			} else if(RGT_PlayerPrefs.raceData.numberOfRaces != numberOfRaces){
				numberOfRaces = RGT_PlayerPrefs.raceData.numberOfRaces;
			}
			EditorGUILayout.BeginHorizontal();
			if (GUILayout.Button ("<", GUILayout.MaxWidth(Screen.width * 0.33f), GUILayout.MaxHeight(35) )) {
				GUIUtility.hotControl = 0;
				GUIUtility.keyboardControl = 0;
				if (raceView > 0) {
					raceView -= 1;
				} else {
					raceView = RGT_PlayerPrefs.raceData.numberOfRaces - 1;
				}
			}
			GUILayout.Label("Race\n" + raceView.ToString(), GUILayout.MaxWidth(Screen.width * 0.33f), GUILayout.MaxHeight(35) );
			if (GUILayout.Button (">", GUILayout.MaxWidth(Screen.width * 0.33f), GUILayout.MaxHeight(35) )) {
				GUIUtility.hotControl = 0;
				GUIUtility.keyboardControl = 0;
				if (raceView < RGT_PlayerPrefs.raceData.numberOfRaces - 1) {
					raceView += 1;
				} else {
					raceView = 0;
				}
			}
			EditorGUILayout.EndHorizontal();
			EditorGUILayout.BeginVertical();
			RGT_PlayerPrefs.raceData.raceNames[raceView] = EditorGUILayout.TextField("Race Name", RGT_PlayerPrefs.raceData.raceNames[raceView]);
			RGT_PlayerPrefs.raceData.raceImages[raceView] = (Sprite) EditorGUILayout.ObjectField("Race Image", RGT_PlayerPrefs.raceData.raceImages[raceView], typeof (Sprite), false, GUILayout.MaxHeight(16) );
			RGT_PlayerPrefs.raceData.gameType[raceView] = (RaceData.GameType) EditorGUILayout.EnumPopup ("Game Type", RGT_PlayerPrefs.raceData.gameType[raceView]);

			RGT_PlayerPrefs.raceData.raceLocked[raceView] = EditorGUILayout.Toggle("Race Locked", RGT_PlayerPrefs.raceData.raceLocked[raceView]);
			if(RGT_PlayerPrefs.raceData.purchaseLevelUnlock == RaceData.Switch.On)
				RGT_PlayerPrefs.raceData.unlockAmount[raceView] = EditorGUILayout.IntField("Unlock Amount", RGT_PlayerPrefs.raceData.unlockAmount[raceView]);
			RGT_PlayerPrefs.raceData.unlimitedRewards[raceView] = EditorGUILayout.Toggle("Unlimited Rewards", RGT_PlayerPrefs.raceData.unlimitedRewards[raceView]);
			RGT_PlayerPrefs.raceData.firstPrize[raceView] = EditorGUILayout.IntField("First Prize", RGT_PlayerPrefs.raceData.firstPrize[raceView]);
			RGT_PlayerPrefs.raceData.secondPrize[raceView] = EditorGUILayout.IntField("Second Prize", RGT_PlayerPrefs.raceData.secondPrize[raceView]);
			RGT_PlayerPrefs.raceData.thirdPrize[raceView] = EditorGUILayout.IntField("Third Prize", RGT_PlayerPrefs.raceData.thirdPrize[raceView]);
			RGT_PlayerPrefs.raceData.extraLapRewardMultiplier[raceView] = EditorGUILayout.IntField("Extra Lap Multiplier", RGT_PlayerPrefs.raceData.extraLapRewardMultiplier[raceView]);
			RGT_PlayerPrefs.raceData.extraRacerRewardMultiplier[raceView] = EditorGUILayout.IntField("Extra Racer Multiplier", RGT_PlayerPrefs.raceData.extraRacerRewardMultiplier[raceView]);
			RGT_PlayerPrefs.raceData.numberOfRacers[raceView] = EditorGUILayout.IntSlider("Number of Racers", RGT_PlayerPrefs.raceData.numberOfRacers[raceView], 1, 64);
			RGT_PlayerPrefs.raceData.racerLimit[raceView] = EditorGUILayout.IntSlider ("Racer Limit", RGT_PlayerPrefs.raceData.racerLimit[raceView], 1, 64);
			RGT_PlayerPrefs.raceData.raceLaps[raceView] = EditorGUILayout.IntField("Number of Laps", RGT_PlayerPrefs.raceData.raceLaps[raceView]);
			RGT_PlayerPrefs.raceData.lapLimit[raceView] = EditorGUILayout.IntField("Lap Limit", RGT_PlayerPrefs.raceData.lapLimit[raceView]);
			RGT_PlayerPrefs.raceData.readyTime[raceView] = EditorGUILayout.FloatField("Ready Time", RGT_PlayerPrefs.raceData.readyTime[raceView]);
			RGT_PlayerPrefs.raceData.showWaypoints[raceView] = EditorGUILayout.Toggle("Show Waypoints", RGT_PlayerPrefs.raceData.showWaypoints[raceView]);
			RGT_PlayerPrefs.raceData.showWaypointArrow[raceView] = EditorGUILayout.Toggle("Show Waypoint Arrow", RGT_PlayerPrefs.raceData.showWaypointArrow[raceView]);
			EditorGUILayout.EndVertical();
			EditorGUILayout.BeginVertical("Box");
			EditorGUILayout.LabelField ("General Settings", editorSkin.customStyles [0]);
			EditorGUILayout.EndVertical();
			RGT_PlayerPrefs.raceData.autoUnlockNextRace = (RaceData.Switch) EditorGUILayout.EnumPopup ("Autounlock Next Race", RGT_PlayerPrefs.raceData.autoUnlockNextRace);
			RGT_PlayerPrefs.raceData.purchaseLevelUnlock = (RaceData.Switch) EditorGUILayout.EnumPopup ("Purchase Level Unlocks", RGT_PlayerPrefs.raceData.purchaseLevelUnlock);
			RGT_PlayerPrefs.raceData.lockButtonText = EditorGUILayout.TextField ("Locked Button Text", RGT_PlayerPrefs.raceData.lockButtonText);
			RGT_PlayerPrefs.raceData.wrongWayDelay = EditorGUILayout.FloatField("Wrong Way Delay", RGT_PlayerPrefs.raceData.wrongWayDelay);
			EditorUtility.SetDirty (RGT_PlayerPrefs.raceData);
		}
/// Opponent Vehicles Settings
		if(opponentVehicleSettings){
			EditorGUILayout.BeginVertical("Box");
			EditorGUILayout.LabelField ("AI Vehicle Settings", editorSkin.customStyles [0]);
			EditorGUILayout.EndVertical();

			EditorGUILayout.BeginHorizontal();
			if (GUILayout.Button ("<", GUILayout.MaxWidth(Screen.width * 0.33f), GUILayout.MaxHeight(35) )) {
				GUIUtility.hotControl = 0;
				GUIUtility.keyboardControl = 0;
				if (opponentCarView > 0) {
					opponentCarView -= 1;
				} else {
					opponentCarView = 62;
				}
			}
			GUILayout.Label("Opponent\n" + opponentCarView.ToString(), GUILayout.MaxWidth(Screen.width * 0.33f), GUILayout.MaxHeight(35) );
			if (GUILayout.Button (">", GUILayout.MaxWidth(Screen.width * 0.33f), GUILayout.MaxHeight(35) )) {
				GUIUtility.hotControl = 0;
				GUIUtility.keyboardControl = 0;
				if (opponentCarView < 62) {
					opponentCarView += 1;
				} else {
					opponentCarView = 0;
				}
			}
			EditorGUILayout.EndHorizontal();
			GUI.skin = defaultSkin;
			RGT_PlayerPrefs.opponentVehicles.opponentNames[opponentCarView] = EditorGUILayout.TextField("Name", RGT_PlayerPrefs.opponentVehicles.opponentNames[opponentCarView]);
			RGT_PlayerPrefs.opponentVehicles.vehicles[opponentCarView] = (GameObject) EditorGUILayout.ObjectField("Prefab", RGT_PlayerPrefs.opponentVehicles.vehicles[opponentCarView], typeof (GameObject), false );
			RGT_PlayerPrefs.opponentVehicles.opponentBodyMaterials[opponentCarView] = (Material) EditorGUILayout.ObjectField("Body Material", RGT_PlayerPrefs.opponentVehicles.opponentBodyMaterials[opponentCarView], typeof (Material), false );
			RGT_PlayerPrefs.opponentVehicles.opponentBodyColors[opponentCarView] = EditorGUILayout.ColorField("Body Color", RGT_PlayerPrefs.opponentVehicles.opponentBodyColors[opponentCarView]);
			if(RGT_PlayerPrefs.opponentVehicles.opponentBodyMaterials[opponentCarView] != null){
				RGT_PlayerPrefs.opponentVehicles.opponentBodyMaterials [opponentCarView].color = RGT_PlayerPrefs.opponentVehicles.opponentBodyColors [opponentCarView];
			}
			EditorUtility.SetDirty (RGT_PlayerPrefs.opponentVehicles);
		}
/// Player Vehicles Settings
		if(playableVehicleSettings){
			EditorGUILayout.BeginVertical("Box");
			EditorGUILayout.LabelField ("Player Vehicle Settings", editorSkin.customStyles [0]);
			EditorGUILayout.EndVertical();

			configureCarSize = (Switch) EditorGUILayout.EnumPopup ("Configure Car Size", configureCarSize);
			if (configureCarSize == Switch.On) {
				EditorGUILayout.HelpBox ("When you reduce this number the values of the affected arrays are deleted. Only reduce this number if you want fewer playable vehicles.", MessageType.Warning);
				EditorGUILayout.BeginHorizontal ();
				numberOfCars = EditorGUILayout.IntField ("Number Of Cars", numberOfCars);
				if (GUILayout.Button ("Update")) {
					GUIUtility.hotControl = 0;
					GUIUtility.keyboardControl = 0;
					RGT_PlayerPrefs.playableVehicles.numberOfCars = numberOfCars;
					carView = 0;
				}
				EditorGUILayout.EndHorizontal ();
				System.Array.Resize (ref RGT_PlayerPrefs.playableVehicles.vehicles, RGT_PlayerPrefs.playableVehicles.numberOfCars);
				System.Array.Resize (ref RGT_PlayerPrefs.playableVehicles.vehicleNames, RGT_PlayerPrefs.playableVehicles.numberOfCars);
				System.Array.Resize (ref RGT_PlayerPrefs.playableVehicles.price, RGT_PlayerPrefs.playableVehicles.numberOfCars);
				System.Array.Resize (ref RGT_PlayerPrefs.playableVehicles.carMaterial, RGT_PlayerPrefs.playableVehicles.numberOfCars);
				System.Array.Resize (ref RGT_PlayerPrefs.playableVehicles.brakeMaterial, RGT_PlayerPrefs.playableVehicles.numberOfCars);
				System.Array.Resize (ref RGT_PlayerPrefs.playableVehicles.glassMaterial, RGT_PlayerPrefs.playableVehicles.numberOfCars);
				System.Array.Resize (ref RGT_PlayerPrefs.playableVehicles.rimMaterial, RGT_PlayerPrefs.playableVehicles.numberOfCars);
				System.Array.Resize (ref RGT_PlayerPrefs.playableVehicles.defaultBodyColors, RGT_PlayerPrefs.playableVehicles.numberOfCars);
				System.Array.Resize (ref RGT_PlayerPrefs.playableVehicles.defaultBrakeColors, RGT_PlayerPrefs.playableVehicles.numberOfCars);
				System.Array.Resize (ref RGT_PlayerPrefs.playableVehicles.defaultGlassColors, RGT_PlayerPrefs.playableVehicles.numberOfCars);
				System.Array.Resize (ref RGT_PlayerPrefs.playableVehicles.defaultRimColors, RGT_PlayerPrefs.playableVehicles.numberOfCars);
				System.Array.Resize (ref RGT_PlayerPrefs.playableVehicles.defaultNeonColors, RGT_PlayerPrefs.playableVehicles.numberOfCars);
				System.Array.Resize (ref RGT_PlayerPrefs.playableVehicles.carGlowLight, RGT_PlayerPrefs.playableVehicles.numberOfCars);
				System.Array.Resize (ref RGT_PlayerPrefs.playableVehicles.carUnlocked, RGT_PlayerPrefs.playableVehicles.numberOfCars);
				System.Array.Resize (ref RGT_PlayerPrefs.playableVehicles.speedometerType, RGT_PlayerPrefs.playableVehicles.numberOfCars);
				System.Array.Resize (ref RGT_PlayerPrefs.playableVehicles.vehicleUpgrades, RGT_PlayerPrefs.playableVehicles.numberOfCars);
				System.Array.Resize (ref RGT_PlayerPrefs.playableVehicles.topSpeedLevel, RGT_PlayerPrefs.playableVehicles.numberOfCars);
				System.Array.Resize (ref RGT_PlayerPrefs.playableVehicles.torqueLevel, RGT_PlayerPrefs.playableVehicles.numberOfCars);
				System.Array.Resize (ref RGT_PlayerPrefs.playableVehicles.brakeTorqueLevel, RGT_PlayerPrefs.playableVehicles.numberOfCars);
				System.Array.Resize (ref RGT_PlayerPrefs.playableVehicles.tireTractionLevel, RGT_PlayerPrefs.playableVehicles.numberOfCars);
				System.Array.Resize (ref RGT_PlayerPrefs.playableVehicles.steerSensitivityLevel, RGT_PlayerPrefs.playableVehicles.numberOfCars);

				System.Array.Resize (ref RGT_PlayerPrefs.playerPrefsData.redValues, RGT_PlayerPrefs.playableVehicles.numberOfCars);
				System.Array.Resize (ref RGT_PlayerPrefs.playerPrefsData.blueValues, RGT_PlayerPrefs.playableVehicles.numberOfCars);
				System.Array.Resize (ref RGT_PlayerPrefs.playerPrefsData.greenValues, RGT_PlayerPrefs.playableVehicles.numberOfCars);
				System.Array.Resize (ref RGT_PlayerPrefs.playerPrefsData.redGlowValues, RGT_PlayerPrefs.playableVehicles.numberOfCars);
				System.Array.Resize (ref RGT_PlayerPrefs.playerPrefsData.blueGlowValues, RGT_PlayerPrefs.playableVehicles.numberOfCars);
				System.Array.Resize (ref RGT_PlayerPrefs.playerPrefsData.greenGlowValues, RGT_PlayerPrefs.playableVehicles.numberOfCars);
				System.Array.Resize (ref RGT_PlayerPrefs.playerPrefsData.redGlassValues, RGT_PlayerPrefs.playableVehicles.numberOfCars);
				System.Array.Resize (ref RGT_PlayerPrefs.playerPrefsData.blueGlassValues, RGT_PlayerPrefs.playableVehicles.numberOfCars);
				System.Array.Resize (ref RGT_PlayerPrefs.playerPrefsData.greenGlassValues, RGT_PlayerPrefs.playableVehicles.numberOfCars);
				System.Array.Resize (ref RGT_PlayerPrefs.playerPrefsData.alphaGlassValues, RGT_PlayerPrefs.playableVehicles.numberOfCars);
				System.Array.Resize (ref RGT_PlayerPrefs.playerPrefsData.redBrakeValues, RGT_PlayerPrefs.playableVehicles.numberOfCars);
				System.Array.Resize (ref RGT_PlayerPrefs.playerPrefsData.blueBrakeValues, RGT_PlayerPrefs.playableVehicles.numberOfCars);
				System.Array.Resize (ref RGT_PlayerPrefs.playerPrefsData.greenBrakeValues, RGT_PlayerPrefs.playableVehicles.numberOfCars);
				System.Array.Resize (ref RGT_PlayerPrefs.playerPrefsData.redRimValues, RGT_PlayerPrefs.playableVehicles.numberOfCars);
				System.Array.Resize (ref RGT_PlayerPrefs.playerPrefsData.blueRimValues, RGT_PlayerPrefs.playableVehicles.numberOfCars);
				System.Array.Resize (ref RGT_PlayerPrefs.playerPrefsData.greenRimValues, RGT_PlayerPrefs.playableVehicles.numberOfCars);
			} else if(RGT_PlayerPrefs.playableVehicles.numberOfCars != numberOfCars){
				numberOfCars = RGT_PlayerPrefs.playableVehicles.numberOfCars;
			}
			EditorGUILayout.BeginHorizontal();
			if (GUILayout.Button ("<", GUILayout.MaxWidth(Screen.width * 0.33f), GUILayout.MaxHeight(35) )) {
				GUIUtility.hotControl = 0;
				GUIUtility.keyboardControl = 0;
				if (carView > 0) {
					carView -= 1;
				} else {
					carView = RGT_PlayerPrefs.playableVehicles.numberOfCars - 1;
				}
			}
			GUILayout.Label("Car\n" + carView.ToString(), GUILayout.MaxWidth(Screen.width * 0.33f), GUILayout.MaxHeight(35) );
			if (GUILayout.Button (">", GUILayout.MaxWidth(Screen.width * 0.33f), GUILayout.MaxHeight(35) )) {
				GUIUtility.hotControl = 0;
				GUIUtility.keyboardControl = 0;
				if (carView < RGT_PlayerPrefs.playableVehicles.numberOfCars - 1) {
					carView += 1;
				} else {
					carView = 0;
				}
			}
			EditorGUILayout.EndHorizontal();
			if (RGT_PlayerPrefs.playableVehicles.numberOfCars > 0) {
				RGT_PlayerPrefs.playableVehicles.vehicleNames [carView] = EditorGUILayout.TextField ("Name", RGT_PlayerPrefs.playableVehicles.vehicleNames [carView]);
				RGT_PlayerPrefs.playableVehicles.price [carView] = EditorGUILayout.IntField ("Price", RGT_PlayerPrefs.playableVehicles.price [carView]);
				RGT_PlayerPrefs.playableVehicles.carUnlocked [carView] = EditorGUILayout.Toggle ("Unlocked", RGT_PlayerPrefs.playableVehicles.carUnlocked [carView]);
				RGT_PlayerPrefs.playableVehicles.vehicles [carView] = (GameObject)EditorGUILayout.ObjectField ("Prefab", RGT_PlayerPrefs.playableVehicles.vehicles [carView], typeof(GameObject), false);
				RGT_PlayerPrefs.playableVehicles.carMaterial [carView] = (Material)EditorGUILayout.ObjectField ("Body Material", RGT_PlayerPrefs.playableVehicles.carMaterial [carView], typeof(Material), false);
				RGT_PlayerPrefs.playableVehicles.brakeMaterial [carView] = (Material)EditorGUILayout.ObjectField ("Brake Material", RGT_PlayerPrefs.playableVehicles.brakeMaterial [carView], typeof(Material), false);
				RGT_PlayerPrefs.playableVehicles.glassMaterial [carView] = (Material)EditorGUILayout.ObjectField ("Glass Material", RGT_PlayerPrefs.playableVehicles.glassMaterial [carView], typeof(Material), false);
				RGT_PlayerPrefs.playableVehicles.rimMaterial [carView] = (Material)EditorGUILayout.ObjectField ("Rim Material", RGT_PlayerPrefs.playableVehicles.rimMaterial [carView], typeof(Material), false);
				RGT_PlayerPrefs.playableVehicles.carGlowLight [carView] = (ParticleSystem)EditorGUILayout.ObjectField ("Neon Particle", RGT_PlayerPrefs.playableVehicles.carGlowLight [carView], typeof(ParticleSystem), false);
//new colors
				GUI.skin = defaultSkin;
				RGT_PlayerPrefs.playableVehicles.defaultBodyColors [carView] = EditorGUILayout.ColorField ("Default Body Color", RGT_PlayerPrefs.playableVehicles.defaultBodyColors [carView]);
				RGT_PlayerPrefs.playableVehicles.defaultBrakeColors [carView] = EditorGUILayout.ColorField ("Default Brake Color", RGT_PlayerPrefs.playableVehicles.defaultBrakeColors [carView]);
				RGT_PlayerPrefs.playableVehicles.defaultGlassColors [carView] = EditorGUILayout.ColorField ("Default Glass Color", RGT_PlayerPrefs.playableVehicles.defaultGlassColors [carView]);
				RGT_PlayerPrefs.playableVehicles.defaultRimColors [carView] = EditorGUILayout.ColorField ("Default Rim Color", RGT_PlayerPrefs.playableVehicles.defaultRimColors [carView]);
				RGT_PlayerPrefs.playableVehicles.defaultNeonColors [carView] = EditorGUILayout.ColorField ("Default Neon Color", RGT_PlayerPrefs.playableVehicles.defaultNeonColors [carView]);
				RGT_PlayerPrefs.playableVehicles.speedometerType [carView] = (RG_DistanceMetrics.SpeedType) EditorGUILayout.EnumPopup ("Speedometer Type", RGT_PlayerPrefs.playableVehicles.speedometerType [carView]);

				EditorGUILayout.BeginVertical ("box");
				showTopSpeedLevelBonus = EditorGUI.Foldout (EditorGUILayout.GetControlRect(), showTopSpeedLevelBonus, "  Top Speed Level Bonus", true);	
				EditorGUILayout.EndVertical();
				if (showTopSpeedLevelBonus) {
					for (int i = 0; i < RGT_PlayerPrefs.playableVehicles.vehicleUpgrades [carView].topSpeed.Length; i++) {
						RGT_PlayerPrefs.playableVehicles.vehicleUpgrades [carView].topSpeed [i] = EditorGUILayout.FloatField ("Level " + (i + 1).ToString (), RGT_PlayerPrefs.playableVehicles.vehicleUpgrades [carView].topSpeed [i]);
					}
				}
				EditorGUILayout.BeginVertical ("box");
				showAccelerationLevelBonus = EditorGUI.Foldout (EditorGUILayout.GetControlRect(), showAccelerationLevelBonus, "  Acceleration Level Bonus", true);						
				EditorGUILayout.EndVertical();
				if (showAccelerationLevelBonus) {
					for (int i = 0; i < RGT_PlayerPrefs.playableVehicles.vehicleUpgrades [carView].acceleration.Length; i++) {
						RGT_PlayerPrefs.playableVehicles.vehicleUpgrades [carView].acceleration [i] = EditorGUILayout.FloatField ("Level " + (i + 1).ToString (), RGT_PlayerPrefs.playableVehicles.vehicleUpgrades [carView].acceleration [i]);
					}
				}
				EditorGUILayout.BeginVertical ("box");
				showBrakePowerLevelBonus = EditorGUI.Foldout (EditorGUILayout.GetControlRect(), showBrakePowerLevelBonus, "  Brake Power Level Bonus", true);	
				EditorGUILayout.EndVertical();
				if (showBrakePowerLevelBonus) {
					for (int i = 0; i < RGT_PlayerPrefs.playableVehicles.vehicleUpgrades [carView].brakePower.Length; i++) {
						RGT_PlayerPrefs.playableVehicles.vehicleUpgrades [carView].brakePower [i] = EditorGUILayout.FloatField ("Level " + (i + 1).ToString (), RGT_PlayerPrefs.playableVehicles.vehicleUpgrades [carView].brakePower [i]);
					}
				}
				EditorGUILayout.BeginVertical ("box");
				showTireTractionLevelBonus = EditorGUI.Foldout (EditorGUILayout.GetControlRect(), showTireTractionLevelBonus, "  Tire Traction Level Bonus", true);	
				EditorGUILayout.EndVertical();
				if (showTireTractionLevelBonus) {
					for (int i = 0; i < RGT_PlayerPrefs.playableVehicles.vehicleUpgrades [carView].tireTraction.Length; i++) {
						RGT_PlayerPrefs.playableVehicles.vehicleUpgrades [carView].tireTraction [i] = EditorGUILayout.FloatField ("Level " + (i + 1).ToString (), RGT_PlayerPrefs.playableVehicles.vehicleUpgrades [carView].tireTraction [i]);
					}
				}
				EditorGUILayout.BeginVertical ("box");
				showSteerSensitivityLevelBonus = EditorGUI.Foldout (EditorGUILayout.GetControlRect(), showSteerSensitivityLevelBonus, "  Steer Sensitivity Level Bonus", true);	
				EditorGUILayout.EndVertical();
				if (showSteerSensitivityLevelBonus) {
					for (int i = 0; i < RGT_PlayerPrefs.playableVehicles.vehicleUpgrades [carView].steerSensitivity.Length; i++) {
						RGT_PlayerPrefs.playableVehicles.vehicleUpgrades [carView].steerSensitivity [i] = EditorGUILayout.FloatField ("Level " + (i + 1).ToString (), RGT_PlayerPrefs.playableVehicles.vehicleUpgrades [carView].steerSensitivity [i]);
					}
				}


				GUI.skin = editorSkin;

				EditorGUILayout.BeginVertical("Box");
				EditorGUILayout.LabelField ("Transmission", editorSkin.customStyles [0]);
				EditorGUILayout.EndVertical();
				RGT_PlayerPrefs.playableVehicles.manual = EditorGUILayout.Toggle ("Manual Shifting", RGT_PlayerPrefs.playableVehicles.manual);
				if (RGT_PlayerPrefs.playableVehicles.manual) {
					RGT_PlayerPrefs.SetTransmission ("Manual");
				} else {
					RGT_PlayerPrefs.SetTransmission ("Auto");
				}

				EditorGUILayout.BeginVertical("Box");
				EditorGUILayout.LabelField ("Render Textures & Graphics", editorSkin.customStyles [0]);
				EditorGUILayout.EndVertical();
				EditorGUILayout.LabelField ("Location:  Assets/TurnTheGameOn/Racing Game Template/Render Textures", EditorStyles.wordWrappedMiniLabel);
				EditorGUILayout.Space ();
				RGT_PlayerPrefs.playableVehicles.rearMirror = EditorGUILayout.Toggle ("Rear View Mirror", RGT_PlayerPrefs.playableVehicles.rearMirror);
				RGT_PlayerPrefs.playableVehicles.sideMirrors = EditorGUILayout.Toggle ("Side View Mirrors", RGT_PlayerPrefs.playableVehicles.sideMirrors);
				RGT_PlayerPrefs.playableVehicles.reflectionProbe = EditorGUILayout.Toggle ("Reflection Probe", RGT_PlayerPrefs.playableVehicles.reflectionProbe);

				EditorGUILayout.BeginVertical("Box");
				EditorGUILayout.LabelField ("Mini Map", editorSkin.customStyles [0]);
				EditorGUILayout.EndVertical();
				RGT_PlayerPrefs.playableVehicles.fixedMiniMapRotation = EditorGUILayout.Toggle ("Fixed Rotation", RGT_PlayerPrefs.playableVehicles.fixedMiniMapRotation);

				EditorGUILayout.BeginVertical ("Box");
				EditorGUILayout.LabelField ("Starting Currency", editorSkin.customStyles [0]);
				EditorGUILayout.EndVertical ();
				RGT_PlayerPrefs.playableVehicles.startingCurrency = EditorGUILayout.IntField ("Starting Currency", RGT_PlayerPrefs.playableVehicles.startingCurrency);
				EditorGUILayout.BeginVertical ("Box");
				EditorGUILayout.LabelField ("Upgrades & Customization", editorSkin.customStyles [0]);
				EditorGUILayout.EndVertical ();
				RGT_PlayerPrefs.playableVehicles.paintPrice = EditorGUILayout.IntField ("Body Paint", RGT_PlayerPrefs.playableVehicles.paintPrice);
				RGT_PlayerPrefs.playableVehicles.brakeColorPrice = EditorGUILayout.IntField ("Brake Color", RGT_PlayerPrefs.playableVehicles.brakeColorPrice);
				RGT_PlayerPrefs.playableVehicles.rimColorPrice = EditorGUILayout.IntField ("Rim Color", RGT_PlayerPrefs.playableVehicles.rimColorPrice);
				RGT_PlayerPrefs.playableVehicles.glassColorPrice = EditorGUILayout.IntField ("Glass Tint", RGT_PlayerPrefs.playableVehicles.glassColorPrice);
				RGT_PlayerPrefs.playableVehicles.glowPrice = EditorGUILayout.IntField ("Neon Light", RGT_PlayerPrefs.playableVehicles.glowPrice);
				RGT_PlayerPrefs.playableVehicles.upgradeSpeedPrice = EditorGUILayout.IntField ("Upgrade Speed", RGT_PlayerPrefs.playableVehicles.upgradeSpeedPrice);
				RGT_PlayerPrefs.playableVehicles.upgradeAccelerationPrice = EditorGUILayout.IntField ("Upgrade Acceleration", RGT_PlayerPrefs.playableVehicles.upgradeAccelerationPrice);
				RGT_PlayerPrefs.playableVehicles.upgradeBrakesPrice = EditorGUILayout.IntField ("Upgrade Brakes", RGT_PlayerPrefs.playableVehicles.upgradeBrakesPrice);
				RGT_PlayerPrefs.playableVehicles.upgradeTiresPrice = EditorGUILayout.IntField ("Upgrade Tires", RGT_PlayerPrefs.playableVehicles.upgradeTiresPrice);
				RGT_PlayerPrefs.playableVehicles.upgradeSteeringPrice = EditorGUILayout.IntField ("Upgrade Steering", RGT_PlayerPrefs.playableVehicles.upgradeSteeringPrice);
				EditorGUILayout.BeginVertical ("Box");
				EditorGUILayout.LabelField ("EVP Support", editorSkin.customStyles [0]);
				EditorGUILayout.EndVertical ();
				RGT_PlayerPrefs.playableVehicles.EVPSupport = EditorGUILayout.Toggle ("EVP Support", RGT_PlayerPrefs.playableVehicles.EVPSupport);
				if (RGT_PlayerPrefs.playableVehicles.EVPSupport) {
					PlayerSettings.SetScriptingDefineSymbolsForGroup (EditorUserBuildSettings.selectedBuildTargetGroup, "CROSS_PLATFORM_INPUT;EVP_SUPPORT");
				} else {
					PlayerSettings.SetScriptingDefineSymbolsForGroup (EditorUserBuildSettings.selectedBuildTargetGroup, "CROSS_PLATFORM_INPUT;");
				}
			}
			EditorUtility.SetDirty (RGT_PlayerPrefs.playableVehicles);
		}
/// PlayerPrefs Settings
		if(playerPrefsSettings){
			EditorGUILayout.BeginVertical("Box");
			EditorGUILayout.LabelField ("PlayerPrefs Settings", editorSkin.customStyles [0]);
			EditorGUILayout.EndVertical();
			if (GUILayout.Button ("Delete PlayerPrefs Data", GUILayout.Height(40))) {
				GUIUtility.hotControl = 0;
				GUIUtility.keyboardControl = 0;
				DeleteAllPlayerPrefsData ();
			}
		}
/// Project Settings
		if(debugSettings){
			EditorGUILayout.BeginVertical("Box");
			EditorGUILayout.LabelField ("Debug Settings", editorSkin.customStyles [0]);
			EditorGUILayout.EndVertical();
			RGT_PlayerPrefs.debugData.fpsCounter = EditorGUILayout.Toggle("FPS Counter", RGT_PlayerPrefs.debugData.fpsCounter);
			RGT_PlayerPrefs.debugData.launchWindow = EditorGUILayout.Toggle("Project Launch Window", RGT_PlayerPrefs.debugData.launchWindow);
			RGT_PlayerPrefs.debugData.projectVersion = EditorGUILayout.TextField ("Project Version", RGT_PlayerPrefs.debugData.projectVersion);
			RGT_PlayerPrefs.debugData.tutorialURL = EditorGUILayout.TextField ("Tutorial URL", RGT_PlayerPrefs.debugData.tutorialURL);
			RGT_PlayerPrefs.debugData.supportURL = EditorGUILayout.TextField ("Support URL", RGT_PlayerPrefs.debugData.supportURL);
			RGT_PlayerPrefs.debugData.documentationURL = EditorGUILayout.TextField ("Documentation URL", RGT_PlayerPrefs.debugData.documentationURL);
			EditorUtility.SetDirty (RGT_PlayerPrefs.debugData);
		}
		EditorGUILayout.EndScrollView ();
		Repaint ();
	}

	void DeleteAllPlayerPrefsData(){
		if (EditorUtility.DisplayDialog ("Racing Game Template", "Are you sure you want to delete all PlayerPrefs?", "Yes", "No")) {
			for(int i = 1; i < RGT_PlayerPrefs.playableVehicles.numberOfCars; i ++){
				RGT_PlayerPrefs.playableVehicles.carUnlocked [i] = false;
			}
			PlayerPrefs.DeleteAll ();
			Debug.Log ("Deleted PlayerPrefs Data");
		}
	}


}