using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using TurnTheGameOn.RacingGameTemplate;

public class RG_OpenWorldManager : MonoBehaviour {

	public GameObject pauseWindow;
	public GameObject spawnPoint;
	private bool gamePaused;
	public GameObject pauseButton;
	public GameObject cameraSwitchButton;
	public GameObject disableSpawnPoint;
	private string gameMode;
	public GameObject pauseText;
	public GameObject restartButton;
	public GameObject[] spawnPointMesh;
	GameObject newVehicle;

	void Start () {
		gameMode = RGT_PlayerPrefs.GetGameMode ();
		if (gameMode == "SINGLE PLAYER") {
			Time.timeScale = 1.0f;
			RGT_PlayerPrefs.playableVehicles.currentVehicleNumber = RGT_PlayerPrefs.GetVehicleNumber ();
			SpawnVehicles (RGT_PlayerPrefs.playableVehicles.currentVehicleNumber);
			Invoke ("EnableUserControl", 0.25f);
		} else {

			pauseText.SetActive (false);
			restartButton.SetActive (false);
		}
		for(int i = 0; i < spawnPointMesh.Length;i++){
			spawnPointMesh [i].SetActive (false);
		}
		if (RGT_PlayerPrefs.inputData.useMobileController)
		{
			GameObject mobileController = Instantiate (RGT_PlayerPrefs.inputData.mobileController, new Vector3 (0, 0, 0), new Quaternion (0, 0, 0, 0)) as GameObject;
		}		
		GameObject garageObjects;
		garageObjects = GameObject.Find ("Garage Objects");
		if (garageObjects != null)
			garageObjects.SetActive (false);
	}

	void EnableUserControl(){
		#if EVP_SUPPORT
		newVehicle.GetComponent<EVP.VehicleStandardInput>().enabled = true;
		#else
		newVehicle.GetComponent<RG_CarUserControl>().enabled = true;
		#endif
		newVehicle.GetComponent<Rigidbody> ().constraints = RigidbodyConstraints.None;
	}

	void Update(){
		if (Input.GetKeyDown (RGT_PlayerPrefs.inputData.pauseKey) || Input.GetKeyDown (RGT_PlayerPrefs.inputData.pauseJoystick)) {
			PauseButton ();
		}
	}

	public void LoadGarageButton(){
		if (gameMode == "MULTIPLAYER") {
			RG_NetworkManagerHUD networkManagerHUD = GameObject.Find ("LobbyManager").GetComponent<RG_NetworkManagerHUD> ();
			networkManagerHUD.Button_BackLobby ();



			//		if (lobbyHUDReference.startGameButton.activeInHierarchy) {
			//			networkLobbyManager.matchMaker.DestroyMatch ((NetworkID)networkLobbyManager.matchInfo.networkId, OnDestroyMatch); 
			//			networkLobbyManager.StopHost();
			//			networkLobbyManager.StopMatchMaker();
			//			Debug.Log ("Destroy Match");
			//		} else {
			//			networkLobbyManager.StopClient();
			//			networkLobbyManager.StopMatchMaker();
			//			Debug.Log ("Leave Match");
			//		}
		}else{
			SceneManager.LoadScene("Garage");    
		}
		//DestroyImmediate (GameObject.Find ("LobbyManager"));

	}

	public void RestartButton(){
		SceneManager.LoadScene(SceneManager.GetActiveScene().name);
	}

	public void PauseButton(){
		if (gameMode == "SINGLE PLAYER") {
			if (gamePaused) {					
				pauseWindow.SetActive (false);
				gamePaused = false;
				Time.timeScale = 1.0f;
				AudioSource[] allAudioSources = FindObjectsOfType (typeof(AudioSource)) as AudioSource[];
				for (int i = 0; i < allAudioSources.Length; i++) {
					if(allAudioSources[i].enabled)      allAudioSources [i].Play ();
				}			
			} else if (!gamePaused) {			
				gamePaused = true;
				Time.timeScale = 0.0f;
				AudioSource[] allAudioSources = FindObjectsOfType (typeof(AudioSource)) as AudioSource[];
				for (int i = 0; i < allAudioSources.Length; i++) {
					allAudioSources [i].Pause ();
				}
				pauseWindow.SetActive (true);				
			}
		} else {
			if (gamePaused) {					
				pauseWindow.SetActive (false);
				gamePaused = false;		
			} else if (!gamePaused) {			
				gamePaused = true;
				pauseWindow.SetActive (true);				
			}
		}
	}

	public void SpawnVehicles(int vehicle){
		spawnPoint.SetActive (false);
		newVehicle = Instantiate(RGT_PlayerPrefs.playableVehicles.vehicles[RGT_PlayerPrefs.playableVehicles.currentVehicleNumber], spawnPoint.transform.position, spawnPoint.transform.rotation) as GameObject; //,GlobalControl.worldUpDir)) as GameObject;
		newVehicle.transform.rotation = spawnPoint.transform.rotation;
		newVehicle.name = "Player";
		if(gameMode == "MULTIPLAYER")
			newVehicle.GetComponent<Rigidbody> ().constraints = RigidbodyConstraints.None;
		disableSpawnPoint.SetActive (false);
		//miniMapCamera.SetActive (true);
	}


}