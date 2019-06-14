#if UNITY_EDITOR

using UnityEditor;
using UnityEngine;
using System.Collections;

namespace TurnTheGameOn.RacingGameTemplate{
	
	[InitializeOnLoad]
	public class Editor_LaunchWindow : EditorWindow{
		
		public static Editor_LaunchWindow editorWindow;
		protected static bool launchedWindow = false;
		protected static int windowTextureWidth = 512;
		protected Texture2D windowTexture;
		protected GUISkin editorSkin;


		static Editor_LaunchWindow (){ 
			EditorApplication.update += CheckShowWelcome;
		}

		[MenuItem ("Window/Racing Game Template/Launch Window")]
		public static void ShowWelcomeScreen(){
			float windowWidth = windowTextureWidth + 8;
			float windowHeight = (windowTextureWidth * 0.75f) + 164;
			Rect rect = new Rect((Screen.currentResolution.width - windowWidth) / 2.0f,
				(Screen.currentResolution.height - windowHeight) / 2.0f,
				windowWidth , windowHeight);
			editorWindow = (Editor_LaunchWindow) EditorWindow.GetWindowWithRect(typeof(Editor_LaunchWindow), rect, true, "Racing Game Template");
			editorWindow.minSize = new Vector2 (windowWidth, windowHeight);
			editorWindow.maxSize = new Vector2 (windowWidth, windowHeight);
			editorWindow.position = rect;
			editorWindow.Show();
			launchedWindow = true;
		}
			
		protected static void CheckShowWelcome(){
			EditorApplication.update -= CheckShowWelcome;
			if (Time.realtimeSinceStartup > 3) launchedWindow = true;
			if (!launchedWindow && RGT_PlayerPrefs.debugData.launchWindow) {
				if (!EditorApplication.isPlayingOrWillChangePlaymode) {
					ShowWelcomeScreen ();
				}
			}
		}

		void OnGUI (){
			GUISkin defaultSkin = GUI.skin;
			if (editorSkin == null) editorSkin = Resources.Load("EditorSkin") as GUISkin;
			GUI.skin = editorSkin;

			if (windowTexture == null) windowTexture = Resources.Load <Texture2D> ("RGT_LaunchWindowTexture");
			GUILayout.Box (windowTexture, GUILayout.Width(windowTextureWidth), GUILayout.Height(windowTextureWidth * 0.75f));


			GUILayout.FlexibleSpace ();

			GUIStyle buttonStyle = new GUIStyle ("button");
			buttonStyle.fontSize = 20;
			buttonStyle.fontStyle = FontStyle.BoldAndItalic;
			buttonStyle.normal.textColor = editorSkin.button.active.textColor;
			buttonStyle.hover.textColor = editorSkin.button.active.textColor;
			buttonStyle.active.textColor = editorSkin.customStyles [2].active.textColor;



			if (GUILayout.Button ("Open RGT Project Editor", buttonStyle, GUILayout.Height(80))) EditorApplication.ExecuteMenuItem ("Window/Racing Game Template/RGT Settings");
			GUILayout.FlexibleSpace ();
			GUILayout.BeginHorizontal ();
			if (GUILayout.Button ("Documentation")) Application.OpenURL (RGT_PlayerPrefs.debugData.documentationURL);
			if (GUILayout.Button ("Tutorials")) Application.OpenURL (RGT_PlayerPrefs.debugData.tutorialURL);
			if (GUILayout.Button ("Support Request")) Application.OpenURL (RGT_PlayerPrefs.debugData.supportURL);
			GUILayout.EndHorizontal ();
			GUILayout.FlexibleSpace ();

			GUI.skin = defaultSkin;


			GUILayout.BeginHorizontal ();
			defaultSkin.toggle.alignment = TextAnchor.UpperCenter;
			bool showOnStartUp = GUILayout.Toggle (RGT_PlayerPrefs.debugData.launchWindow, "Launch this window when Unity is opened?");
			if (RGT_PlayerPrefs.debugData.launchWindow != showOnStartUp){
				RGT_PlayerPrefs.debugData.launchWindow = showOnStartUp;
				EditorUtility.SetDirty (RGT_PlayerPrefs.debugData);
			}
			GUILayout.FlexibleSpace ();

			GUILayout.Label ("Version " + RGT_PlayerPrefs.debugData.projectVersion + "  ", editorSkin.customStyles [2]);
			GUILayout.EndHorizontal ();
			GUILayout.FlexibleSpace ();
			Repaint ();
		}
	}

}
#endif