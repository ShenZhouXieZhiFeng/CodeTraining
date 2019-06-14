using System;
using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using TurnTheGameOn.RacingGameTemplate;

public class RG_NetPlayer : NetworkBehaviour {

	[SerializeField] Behaviour[] disableComponents;
	[SerializeField] Behaviour[] disableLocalComponents;
	[SerializeField] GameObject[] playerModelLayer;
	[SerializeField] public Renderer objectRef;
	[SyncVar] public int slot;
	[SyncVar] public Color carBodyColor;
	[SyncVar] public Color carGlassColor;
	[SyncVar] public Color carBrakeColor;
	[SyncVar] public Color carRimColor;
	[SyncVar] public Color carNeonColor;
	[SyncVar] public string lobbyPlayer;
	[SyncVar] public int vehicleNumber;
	public ParticleSystem neonGlow;
	public GameObject playerCamera;
	RG_NetworkManagerHUD netManagerHUD;
	public RG_NetworkLobbyPlayer[] lobbyPlayers;
	public GameObject waypointArrow;
	public GameObject waypointArrowCamera;
	private string gameMode;

	void Start () {
		gameMode = RGT_PlayerPrefs.GetGameMode ();
		lobbyPlayers = GameObject.FindObjectsOfType<RG_NetworkLobbyPlayer>();
		if (gameMode == "SINGLE PLAYER") {
			waypointArrow.SetActive (true);
			waypointArrowCamera.SetActive (true);
			vehicleNumber = RGT_PlayerPrefs.playableVehicles.currentVehicleNumber;
			carNeonColor.a = 0.1f;

			var main = neonGlow.main;
			main.startColor = carNeonColor;

			//neonGlow.startColor = carNeonColor;


			neonGlow.gameObject.SetActive (true);
			GameObject lobbyManager = GameObject.Find ("LobbyManager");
			Destroy (lobbyManager);
			enabled = false;
		} else {
			#if EVP_Support

			#else
			GetComponent<RG_CarUserControl>().enabled = true;
			#endif
			waypointArrow.SetActive (false);
			waypointArrowCamera.SetActive (false);
			if (isLocalPlayer) {			
				for (int i = 0; i < disableLocalComponents.Length; i++) {
					disableLocalComponents [i].enabled = false;
				}
				for (int i = 0; i < lobbyPlayers.Length; i++) {



					if (lobbyPlayers [i].isLocalPlayer) {
						CmdAssignParent (i);

						vehicleNumber = RGT_PlayerPrefs.playableVehicles.currentVehicleNumber;

						carBodyColor = RGT_PlayerPrefs.playableVehicles.carMaterial [vehicleNumber].color;
						CmdAssignBodyColor (carBodyColor);

						carGlassColor = RGT_PlayerPrefs.playableVehicles.glassMaterial [vehicleNumber].color;
						CmdAssignGlassColor (carGlassColor);

						carBrakeColor = RGT_PlayerPrefs.playableVehicles.brakeMaterial [vehicleNumber].color;
						CmdAssignBrakeColor (carBrakeColor);

						carRimColor = RGT_PlayerPrefs.playableVehicles.rimMaterial [vehicleNumber].color;
						CmdAssignRimColor (carRimColor);

						carNeonColor.a = 0.1f;
						carNeonColor.r = RGT_PlayerPrefs.playerPrefsData.redGlowValues [vehicleNumber];
						carNeonColor.b = RGT_PlayerPrefs.playerPrefsData.blueGlowValues [vehicleNumber];
						carNeonColor.g = RGT_PlayerPrefs.playerPrefsData.greenGlowValues [vehicleNumber];
						CmdAssignNeonColor (carNeonColor);
					}


				}

			} else {
				for (int i = 0; i < playerModelLayer.Length; i++) {
					playerModelLayer [i].layer = LayerMask.NameToLayer ("Default");
				}
				for (int i = 0; i < disableComponents.Length; i++) {
					disableComponents [i].enabled = false;
				}
			}
		}
	}

	void Update(){
		if (lobbyPlayer != "") {
			transform.name = "Player Car";
			transform.name = "Player Car " + slot.ToString ();
			transform.SetParent (GameObject.Find (lobbyPlayer).transform);
			playerCamera.transform.SetParent (GameObject.Find (lobbyPlayer).transform);
			#if EVP_SUPPORT

			#else
			playerCamera.GetComponent<RG_CarCamera> ().lobbyPlayer = GameObject.Find (lobbyPlayer).transform;
			#endif
			BroadcastMessage ("UpdateMaterialColors");

			var main = neonGlow.main;
			main.startColor = carNeonColor;

			//neonGlow.startColor = carNeonColor;



			neonGlow.gameObject.SetActive (true);
			GetComponent<Rigidbody> ().constraints = RigidbodyConstraints.None;
			enabled = false;
		}
	}

	[Command]
	void CmdAssignParent(int i){
		slot = lobbyPlayers [i].slot;
		lobbyPlayer = lobbyPlayers [i].transform.name;
	}

	[Command]
	void CmdAssignBodyColor(Color c){
		carBodyColor = c;
	}
	[Command]
	void CmdAssignGlassColor(Color c){
		carGlassColor = c;
	}
	[Command]
	void CmdAssignBrakeColor(Color c){
		carBrakeColor = c;
	}
	[Command]
	void CmdAssignRimColor(Color c){
		carRimColor = c;
	}
	[Command]
	void CmdAssignNeonColor(Color c){
		carNeonColor = c;
	}

}

