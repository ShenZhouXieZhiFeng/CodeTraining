using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TurnTheGameOn.RacingGameTemplate;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;

[System.Serializable]
public class ManagerReference{
	[Header("Game Object Reference")]
	public GameObject managerCamera;
	public GameObject nextLevelButton;
	public GameObject cameraSwitchButton;
	public GameObject pauseButton;
	public GameObject miniMap;
	public GameObject miniMapBorder;
	public RG_WaypointCircuit waypointCircut;
	[Header("UI Text Reference")]
	public Text[] positionText;
	public RectTransform[] positionObject;
	public Vector2[] displayPositions;
	public Text playerPositionText;
	public Text playerLapText;
	public Text timerText;
	public Text completeTime;
	public Text finalStanding;
	public Text rewardText;
}

[System.Serializable]
public class RacerInfo{
	public string racerName;
	public GameObject racer;
	public int position = 1;
	public int lap = 1;
	public bool finishedRace;
	public int finalStanding;
	public float finishTime;
	public Transform currentWaypoint;
	public int nextWP;
	public float checkpointDistance;
	public float positionScore;
	public float distanceScore;
	public float totalScore;
	public RG_AIHelper aIHelper_Default;
	#if EVP_SUPPORT
	public RGT_EVPAIHelper aIHelper_EVP;
	#endif
	public Rigidbody rbody;
}

[ExecuteInEditMode][RequireComponent (typeof (RG_WaypointCircuit))]
public class RG_SceneManager : MonoBehaviour {
	public enum Switch { Off, On }

	public bool canCalculatePosition;
	public RacerInfo[] racerInfo;
	public GameObject[] spawnPoints;
	public ManagerReference managerReference;
	[Tooltip("Configure mode should only be turned on when you're ready to update your waypoint settings.")]
	public Switch configureMode;
	[Tooltip("Set the number of waypoints for this scene.")]
	public int TotalWaypoints;
	public Transform[] Waypoints;
	private RG_Waypoint[] waypointScripts;
	Behaviour playerController;
	public float gameTime;
	private string timeString;

	private bool gameStarted;
	private bool startedRace;
	public bool countUp;
	private bool countDown = true;
	private bool playedRaceTimerAudio;
	int[] toSort;
	float[] toSort2;
	public bool showWaypointMesh;

	public UnityEvent onRaceFinished;

	public static RG_SceneManager instance;

	void Awake(){
		if (Application.isPlaying) {
			instance = this;
			RGT_PlayerPrefs.raceData.raceNumber = RGT_PlayerPrefs.GetRaceNumber ();
			Array.Resize (ref racerInfo, RGT_PlayerPrefs.raceData.numberOfRacers[RGT_PlayerPrefs.raceData.raceNumber]);
			Array.Resize (ref toSort, RGT_PlayerPrefs.raceData.numberOfRacers[RGT_PlayerPrefs.raceData.raceNumber]);
			Array.Resize (ref toSort2, RGT_PlayerPrefs.raceData.numberOfRacers[RGT_PlayerPrefs.raceData.raceNumber]);
			showWaypointMesh = RGT_PlayerPrefs.raceData.showWaypoints[RGT_PlayerPrefs.raceData.raceNumber];
		}
	}

	void Start () {
		if (Application.isPlaying)	Invoke ("StartGame", 0.3f);
		Time.timeScale = 1.0f;
	}
	void StartGame(){
		for (int i = 0; i < RGT_PlayerPrefs.raceData.numberOfRacers[RGT_PlayerPrefs.raceData.raceNumber]; i++) {
			managerReference.positionText [i].gameObject.SetActive (true);
			racerInfo [i].position = i + 1;
		}
		racerInfo [0].racerName = RGT_PlayerPrefs.playableVehicles.playerName;																//get player name
		RGT_PlayerPrefs.raceData.raceLaps[RGT_PlayerPrefs.raceData.raceNumber] = RGT_PlayerPrefs.GetRaceLaps(RGT_PlayerPrefs.raceData.raceNumber, RGT_PlayerPrefs.raceData.raceLaps[RGT_PlayerPrefs.raceData.raceNumber]); //get race laps
		RGT_PlayerPrefs.raceData.vehicleNumber = RGT_PlayerPrefs.GetVehicleNumber();														//get player vehicle
		RGT_PlayerPrefs.raceData.currency = RGT_PlayerPrefs.GetCurrency();																	//get currunt currency
		RGT_PlayerPrefs.raceData.raceRewards[0] = RGT_PlayerPrefs.GetRaceReward (1, RGT_PlayerPrefs.raceData.raceRewards[0]);				//get first place reward
		RGT_PlayerPrefs.raceData.raceRewards[1] = RGT_PlayerPrefs.GetRaceReward (2, RGT_PlayerPrefs.raceData.raceRewards[1]);				//get second place reward
		RGT_PlayerPrefs.raceData.raceRewards[2] = RGT_PlayerPrefs.GetRaceReward (3, RGT_PlayerPrefs.raceData.raceRewards[2]);				//get third place reward
		RGT_PlayerPrefs.raceData.bestRaceTime = RGT_PlayerPrefs.GetBestRaceTime (RGT_PlayerPrefs.raceData.raceNumber, 9999.99f);			//get best race time
		RGT_PlayerPrefs.raceData.bestLapTime = RGT_PlayerPrefs.GetBestLapTime (RGT_PlayerPrefs.raceData.raceNumber, 9999.99f);				//get best lap time
		SpawnVehicles (RGT_PlayerPrefs.raceData.vehicleNumber);
		gameStarted = true;
		canCalculatePosition = true;
	}

	void Update () {
		#if UNITY_EDITOR
		if(!Application.isPlaying){
			CalculateWaypoints ();
		}
		#endif
		if (gameStarted) {			
			if (!startedRace)	StartRace ();
			GameTime ();
			GenTimeSpanFromSeconds( (double) gameTime );
			if (canCalculatePosition)	CalculateRacerPositions ();
			UIPositionDisplay ();
		}
	}

	void GameTime(){
		if(countUp)	gameTime += 1 * Time.deltaTime;
		if(countDown){
			gameTime -= 1 * Time.deltaTime;
			if(gameTime <= 2 && !playedRaceTimerAudio){
				playedRaceTimerAudio = true;
				GameObject tempObject = Instantiate(Resources.Load ("Audio Clip - Race Timer")) as GameObject;
				tempObject.name = "Audio Clip - Race Timer";
				tempObject = null;
			}
		}
	}

	void StartRace(){
		if (gameTime <= 0) {	
			startedRace = true;
			countDown = false;
			gameTime = 0;
			countUp = true;
			racerInfo [0].rbody.constraints = RigidbodyConstraints.None;
			for (int i = 1; i < racerInfo.Length; i++) {
				#if EVP_SUPPORT
				racerInfo [i].aIHelper_EVP.enabled = true;
				#else
				racerInfo [i].aIHelper_Default.enabled = true;
				#endif
				racerInfo [i].rbody.constraints = RigidbodyConstraints.None;
			}
			playerController.enabled = true;
		}
	}

	/// <summary>
	/// Called when a target waypoint is reached, handles waypoint targeting, lap and race completion status for each racer
	/// </summary>
	/// <param name="racerNumber">Racer number.</param>
	/// <param name="waypointNumber">Waypoint number.</param>
	public void ChangeTarget(int racerNumber, int waypointNumber){
		if (Application.isPlaying && waypointNumber == racerInfo[racerNumber].nextWP + 1) {
			#if EVP_SUPPORT

			#else
			racerInfo[racerNumber].aIHelper_Default.ResetTimer();
			#endif
			float check = racerInfo[racerNumber].nextWP;
			if (check < TotalWaypoints) {
				racerInfo[racerNumber].nextWP += 1;
				racerInfo[racerNumber].positionScore += 2;
				if(racerNumber == 0 && showWaypointMesh)
					racerInfo[racerNumber].currentWaypoint.gameObject.GetComponent<MeshRenderer>().enabled = false;
				if (racerInfo [racerNumber].nextWP != TotalWaypoints) {
					racerInfo [racerNumber].currentWaypoint = Waypoints [racerInfo [racerNumber].nextWP];
				} else {
					racerInfo[racerNumber].nextWP = 0;
					if (RGT_PlayerPrefs.raceData.raceLaps[RGT_PlayerPrefs.raceData.raceNumber] > racerInfo [racerNumber].lap) {
						racerInfo[racerNumber].nextWP = 0;
						racerInfo [racerNumber].currentWaypoint = Waypoints [racerInfo [racerNumber].nextWP];
						racerInfo [racerNumber].lap += 1;
						racerInfo[racerNumber].positionScore += 100;
					}else if(!racerInfo[racerNumber].finishedRace){
						racerInfo[racerNumber].finishedRace = true;
						racerInfo[racerNumber].finalStanding = racerInfo[racerNumber].position;
						if(racerNumber == 0) FinishedRace ();
					}
				}
				if(racerNumber == 0 && showWaypointMesh)
					racerInfo[racerNumber].currentWaypoint.gameObject.GetComponent<MeshRenderer>().enabled = true;
			}
		}
	}

	void CalculateRacerPositions() {
		canCalculatePosition = false;
		for (int i = 0; i < racerInfo.Length; i ++) {
			if(!racerInfo[i].finishedRace){
				if(racerInfo[i].lap >= RGT_PlayerPrefs.raceData.raceLaps[RGT_PlayerPrefs.raceData.raceNumber] + 1){
					racerInfo[i].finishedRace = true;
					racerInfo[i].finishTime = gameTime;
					racerInfo[i].finalStanding = racerInfo[i].position;
					Debug.Log("Racer " + i + " finished the race.");
				}
			}
		}
		float distance;
		for (int i = 0; i < racerInfo.Length; i ++) {
			racerInfo [i].checkpointDistance = Vector3.Distance (racerInfo [i].racer.transform.position, racerInfo [i].currentWaypoint.position);
			distance = (0.00001f * racerInfo [i].checkpointDistance);
			racerInfo [i].distanceScore = - (distance);
			racerInfo [i].totalScore = racerInfo [i].positionScore + racerInfo [i].distanceScore;
			for(int i2 = 0; i2 < RGT_PlayerPrefs.raceData.numberOfRacers[RGT_PlayerPrefs.raceData.raceNumber]; i2++){
				toSort[i2] = racerInfo [i2].position;
			}
			foreach (int sort in toSort.OrderBy(sorted=>sorted)) {
				if (racerInfo [i].position == sort) {
					managerReference.positionText [sort - 1].text = sort + "   " + racerInfo [i].racerName;
				}
			}
		}
		for(int i2 = 0; i2 < RGT_PlayerPrefs.raceData.numberOfRacers[RGT_PlayerPrefs.raceData.raceNumber]; i2++){
			toSort2[i2] = racerInfo [i2].totalScore;
		}
		for (int i = 0; i < racerInfo.Length; i ++) {
			var sort2 = toSort2.OrderByDescending(sorted=>sorted).ToArray();
			float scoreCheck = racerInfo[i].totalScore;
			for (int i2 = 0; i2 < toSort2.Length; i2 ++) {
				if (scoreCheck == sort2 [i2]) {
					racerInfo [i].position = i2 + 1;
				}
			}
		}
		canCalculatePosition = true;
	}

	/// <summary>
	/// Handles UI position data
	/// </summary>
	void UIPositionDisplay(){
		managerReference.playerPositionText.text = racerInfo [0].position.ToString() + "/" + RGT_PlayerPrefs.raceData.numberOfRacers[RGT_PlayerPrefs.raceData.raceNumber].ToString();
		managerReference.playerLapText.text = "Lap     " + racerInfo[0].lap.ToString() + " / " + RGT_PlayerPrefs.raceData.raceLaps[RGT_PlayerPrefs.raceData.raceNumber].ToString();
		if(RGT_PlayerPrefs.raceData.numberOfRacers[RGT_PlayerPrefs.raceData.raceNumber] > 8){
			if (racerInfo [0].position > 8) {
				for (int i = 6; i < RGT_PlayerPrefs.raceData.numberOfRacers[RGT_PlayerPrefs.raceData.raceNumber] + 1; i++) {
					if (racerInfo [0].position != i) {
						managerReference.positionObject [i - 1].gameObject.SetActive (false);

					} else {
						Vector3 tPos;
						tPos = managerReference.positionObject [i - 1].localPosition;
						tPos.y = 243.78f;
						managerReference.positionObject [i - 1].localPosition = tPos;
						managerReference.positionObject [i - 1].gameObject.SetActive (true);

						tPos = managerReference.positionObject [i - 2].localPosition;
						tPos.y = 275.78f;
						managerReference.positionObject [i - 2].localPosition = tPos;
						managerReference.positionObject [i - 2].gameObject.SetActive (true);

						tPos = managerReference.positionObject [i - 3].localPosition;
						tPos.y = 307.78f;
						managerReference.positionObject [i - 3].localPosition = tPos;
						managerReference.positionObject [i - 3].gameObject.SetActive (true);
					}
				}
			} else {
				for(int i = 0; i < RGT_PlayerPrefs.raceData.numberOfRacers[RGT_PlayerPrefs.raceData.raceNumber]; i++){
					if (i <= 7) {
						Vector3 tPos;
						if (i == 6) {
							tPos = managerReference.positionObject [i].localPosition;
							tPos.y = 275.78f;
							managerReference.positionObject [i].localPosition = tPos;
						}
						if (i == 7) {							
							tPos = managerReference.positionObject [i].localPosition;
							tPos.y = 243.78f;
							managerReference.positionObject [i].localPosition = tPos;
						}
						managerReference.positionObject [i].gameObject.SetActive (true);
					} else {
						managerReference.positionObject [i].gameObject.SetActive (false);
					}
				}
			}
		}
	}

	public void GenTimeSpanFromSeconds( double seconds ){
		TimeSpan interval = TimeSpan.FromSeconds( seconds );			
		string timeInterval = string.Format("{0:D2}:{1:D2}",  interval.Minutes, interval.Seconds);
		timeString = timeInterval + "." + interval.Milliseconds.ToString ("D3");
		managerReference.timerText.text = timeString;
	}

	/// <summary>
	/// Handles race completion logic when the player completes the race
	/// </summary>
	void FinishedRace(){		
		managerReference.miniMapBorder.SetActive(false);
		managerReference.miniMap.SetActive(false);
		#if EVP_SUPPORT
		racerInfo[0].aIHelper_EVP.enabled = true;
		racerInfo [0].aIHelper_EVP.enabled = true;
		#else
		racerInfo[0].aIHelper_Default.enabled = true;
		racerInfo [0].aIHelper_Default.enabled = true;
		#endif

		playerController.enabled = false;
		int temp = RGT_PlayerPrefs.raceData.raceNumber + 1;
		managerReference.finalStanding.text = "Final Standing: " + racerInfo[0].position.ToString();
		racerInfo[0].finishTime = gameTime;
		TimeSpan interval = TimeSpan.FromSeconds( racerInfo[0].finishTime );			
		string timeInterval = string.Format("{0:D2}:{1:D2}",  interval.Minutes, interval.Seconds);
		timeString = timeInterval + "." + interval.Ticks.ToString ().Substring (1,3);
		managerReference.completeTime.text = "Finish Time: " + timeString;
		//set new best race time
		if(racerInfo[0].finishTime < RGT_PlayerPrefs.raceData.bestRaceTime)	RGT_PlayerPrefs.SetBestRaceTime(RGT_PlayerPrefs.raceData.raceNumber, racerInfo[0].finishTime);				


		switch (racerInfo [0].finalStanding) {
		case 1:
			//check if race is completed and unlimited rewards is enabled
			if (RGT_PlayerPrefs.GetRaceStatus (RGT_PlayerPrefs.raceData.raceNames [RGT_PlayerPrefs.raceData.raceNumber]) == "COMPLETE" && !RGT_PlayerPrefs.raceData.unlimitedRewards [RGT_PlayerPrefs.raceData.raceNumber]) {
				managerReference.rewardText.text = "Reward: $0";
			} else {
				//set new currency
				RGT_PlayerPrefs.SetCurrency (RGT_PlayerPrefs.raceData.currency + RGT_PlayerPrefs.raceData.raceRewards [0]);
				managerReference.rewardText.text = "Reward: $" + RGT_PlayerPrefs.raceData.raceRewards [0].ToString ();
			}
			//check if auto unlock next race is on and this is not the last race
			if (RGT_PlayerPrefs.raceData.autoUnlockNextRace == RaceData.Switch.On && RGT_PlayerPrefs.raceData.raceNumber != RGT_PlayerPrefs.raceData.raceNames.Length - 1) {
				//set new race number
				RGT_PlayerPrefs.SetRaceNumber (temp);
				managerReference.nextLevelButton.SetActive (true);
				//unlock the next race
				RGT_PlayerPrefs.SetRaceLock (RGT_PlayerPrefs.raceData.raceNames [RGT_PlayerPrefs.raceData.raceNumber + 1], "UNLOCKED");
			} else {
				managerReference.nextLevelButton.SetActive (false);
			}
			//set race status to complete
			RGT_PlayerPrefs.SetRaceStatus (RGT_PlayerPrefs.raceData.raceNames [RGT_PlayerPrefs.raceData.raceNumber], "COMPLETE");
			break;
		case 2:
			//check if race is completed and unlimited rewards is enabled
			if (RGT_PlayerPrefs.GetRaceStatus (RGT_PlayerPrefs.raceData.raceNames [RGT_PlayerPrefs.raceData.raceNumber]) == "COMPLETE" && !RGT_PlayerPrefs.raceData.unlimitedRewards [RGT_PlayerPrefs.raceData.raceNumber]) {
				managerReference.rewardText.text = "Reward: $0";
			} else {
				//set new currency
				RGT_PlayerPrefs.SetCurrency (RGT_PlayerPrefs.raceData.currency + RGT_PlayerPrefs.raceData.raceRewards [1]);
				managerReference.rewardText.text = "Reward: $" + RGT_PlayerPrefs.raceData.raceRewards [1].ToString ();
			}
			//check if auto unlock next race is on and this is not the last race
			if (RGT_PlayerPrefs.raceData.autoUnlockNextRace == RaceData.Switch.On && RGT_PlayerPrefs.raceData.raceNumber != RGT_PlayerPrefs.raceData.raceNames.Length - 1) {
				//set new race number
				RGT_PlayerPrefs.SetRaceNumber (temp);
				managerReference.nextLevelButton.SetActive (true);
				//unlock the next race
				RGT_PlayerPrefs.SetRaceLock (RGT_PlayerPrefs.raceData.raceNames [RGT_PlayerPrefs.raceData.raceNumber + 1], "UNLOCKED");
			} else {
				managerReference.nextLevelButton.SetActive (false);
			}
			break;
		case 3:
			//check if race is completed and unlimited rewards is enabled
			if (RGT_PlayerPrefs.GetRaceStatus (RGT_PlayerPrefs.raceData.raceNames [RGT_PlayerPrefs.raceData.raceNumber]) == "COMPLETE" && !RGT_PlayerPrefs.raceData.unlimitedRewards [RGT_PlayerPrefs.raceData.raceNumber]) {
				managerReference.rewardText.text = "Reward: $0";
			} else {
				//set new currency
				RGT_PlayerPrefs.SetCurrency (RGT_PlayerPrefs.raceData.currency + RGT_PlayerPrefs.raceData.raceRewards [2]);
				managerReference.rewardText.text = "Reward: $" + RGT_PlayerPrefs.raceData.raceRewards [2].ToString ();
			}
			//check if auto unlock next race is on and this is not the last race
			if (RGT_PlayerPrefs.raceData.autoUnlockNextRace == RaceData.Switch.On && RGT_PlayerPrefs.raceData.raceNumber != RGT_PlayerPrefs.raceData.raceNames.Length - 1) {
				//set new race number
				RGT_PlayerPrefs.SetRaceNumber (temp);
				managerReference.nextLevelButton.SetActive (true);
				//unlock the next race
				RGT_PlayerPrefs.SetRaceLock (RGT_PlayerPrefs.raceData.raceNames [RGT_PlayerPrefs.raceData.raceNumber + 1], "UNLOCKED");
			} else {
				managerReference.nextLevelButton.SetActive (false);
			}
			break;
		}
		onRaceFinished.Invoke ();
	}

	public void ResetLostPlayer(int playerNumber){
		int temp = racerInfo [playerNumber].nextWP - 1;
		if(temp < 0)	temp = Waypoints.Length - 1;
		racerInfo [playerNumber].racer.transform.position = Waypoints [temp].position;
	}

	#region Waypoint Creation Methods
	public void CalculateWaypoints(){
		if (configureMode == Switch.On) {
			GameObject newWaypoint;
			string newWaypointName;
			System.Array.Resize (ref Waypoints, TotalWaypoints);
			System.Array.Resize (ref waypointScripts, TotalWaypoints);
			for (var i = 0; i < TotalWaypoints; i++) {
				newWaypointName = "Waypoint " + (i + 1);
				if (Waypoints [i] == null) {
					foreach (Transform child in transform) {
						if (child.name == newWaypointName) {		Waypoints [i] = child;			}
					}
					if (Waypoints [i] == null) {
						newWaypoint = Instantiate (Resources.Load ("Waypoint")) as GameObject;
						newWaypoint.name = newWaypointName;
						newWaypoint.transform.parent = gameObject.transform;
						Waypoints [i] = newWaypoint.transform;
						waypointScripts [i] = newWaypoint.GetComponent<RG_Waypoint>();
						waypointScripts [i].waypointNumber = i + 1;
						Debug.Log ("Waypoint Controller created a new Waypoint: " + newWaypointName);
					}
				}				
			}
			newWaypoint = null;
			newWaypointName = null;
			CleanUpWaypoints ();
			managerReference.waypointCircut.AssignWaypoints();
		}
	}

	public void CleanUpWaypoints(){
		if (configureMode == Switch.On) {
			if (transform.childCount > Waypoints.Length) {
				foreach (Transform oldChild in transform) {
					if (oldChild.GetComponent<RG_Waypoint> ().waypointNumber > Waypoints.Length) {
						DestroyImmediate (oldChild.gameObject);
					}
				}
			}
		}
	}
	#endregion

	#region UI Button Methods
	/// <summary>
	/// Loads the next race scene
	/// </summary>
	public void LoadNextLevelButton(){
		int temp = RGT_PlayerPrefs.raceData.raceNumber + 1;
		SceneManager.LoadScene(temp.ToString() + RGT_PlayerPrefs.raceData.raceNames[RGT_PlayerPrefs.raceData.raceNumber + 1]);
	}
	/// <summary>
	/// Loads the garage scene
	/// </summary>
	public void LoadGarageButton(){
		SceneManager.LoadScene ("Garage");
	}
	/// <summary>
	/// Reloads the current scene
	/// </summary>
	public void RestartButton(){		SceneManager.LoadScene(SceneManager.GetActiveScene().name);					}
	#endregion

	public void SpawnVehicles(int vehicle){
		managerReference.managerCamera.SetActive (false);
		GameObject newVehicle;
		gameTime = RGT_PlayerPrefs.raceData.readyTime[RGT_PlayerPrefs.raceData.raceNumber];
		countDown = true;
		spawnPoints[0].SetActive (false);
		if (RGT_PlayerPrefs.inputData.useMobileController)	newVehicle = Instantiate (RGT_PlayerPrefs.inputData.mobileController, new Vector3 (0, 0, 0), new Quaternion (0, 0, 0, 0)) as GameObject;
		newVehicle = Instantiate(RGT_PlayerPrefs.playableVehicles.vehicles[RGT_PlayerPrefs.raceData.vehicleNumber], spawnPoints[0].transform.position, spawnPoints[0].transform.rotation) as GameObject;
		newVehicle.transform.rotation = spawnPoints[0].transform.rotation;
		racerInfo [0].racer = newVehicle;
		racerInfo[0].racer.name = racerInfo[0].racerName;
		racerInfo [0].lap = 1;
		#if EVP_SUPPORT
		playerController = newVehicle.GetComponent<EVP.VehicleStandardInput>();
		racerInfo[0].aIHelper_EVP = racerInfo[0].racer.GetComponent<RGT_EVPAIHelper>();
		#else
		playerController = newVehicle.GetComponent<RG_CarUserControl>();
		racerInfo[0].aIHelper_Default = racerInfo[0].racer.GetComponent<RG_AIHelper>();
		#endif

		racerInfo[0].rbody = racerInfo[0].racer.GetComponent<Rigidbody>();
		racerInfo [0].currentWaypoint = Waypoints [0];
		for(int i = 0; i < managerReference.positionObject.Length; i++){
			managerReference.positionObject [i].gameObject.SetActive (false);
		}
		for(int i = 0; i < RGT_PlayerPrefs.raceData.numberOfRacers[RGT_PlayerPrefs.raceData.raceNumber]; i++){
			managerReference.positionObject [i].gameObject.SetActive (true);
		}
		for(int i = 1; i < RGT_PlayerPrefs.raceData.numberOfRacers[RGT_PlayerPrefs.raceData.raceNumber]; i ++){
			spawnPoints[i].SetActive (false);
			newVehicle = Instantiate(RGT_PlayerPrefs.opponentVehicles.vehicles[i - 1], spawnPoints[i].transform.position, spawnPoints[i].transform.rotation) as GameObject;
			newVehicle.transform.rotation = spawnPoints[i].transform.rotation;
			racerInfo[i].racer = newVehicle;
			racerInfo[i].racerName = RGT_PlayerPrefs.opponentVehicles.opponentNames[i - 1];
			racerInfo[i].racer.name = racerInfo[i].racerName;
			racerInfo [0].lap = 1;
			if(i > 0){
				#if EVP_SUPPORT
				racerInfo[i].aIHelper_EVP = racerInfo[i].racer.GetComponent<RGT_EVPAIHelper>();
				racerInfo[i].aIHelper_EVP.opponentNumber = i;
				#else
				racerInfo[i].aIHelper_Default = racerInfo[i].racer.GetComponent<RG_AIHelper>();
				racerInfo[i].aIHelper_Default.opponentNumber = i;
				#endif

				racerInfo[i].rbody = racerInfo[i].racer.GetComponent<Rigidbody>();

				racerInfo [i].positionScore = 0;
				racerInfo [i].currentWaypoint = Waypoints [0];
			}
		}
		newVehicle = null;
		for (int i = 0; i < spawnPoints.Length; i++) {
			spawnPoints [i].SetActive (false);
		}
	}

}