using UnityEngine;
using System.Collections;
using TurnTheGameOn.RacingGameTemplate;

[ExecuteInEditMode]
public class RG_Waypoint : MonoBehaviour {

	private RG_SceneManager sceneManager;
	public int waypointNumber;
	[Range(0.01f,1f)] public float AISpeedFactor = 1.0f;

	void Start () {
		sceneManager = this.transform.parent.GetComponent<RG_SceneManager>();
	}

	void OnTriggerEnter (Collider col) {
		for(int i = 0; i < RGT_PlayerPrefs.raceData.numberOfRacers[RGT_PlayerPrefs.raceData.raceNumber]; i++){
			if (i == 0) {
				if (col.transform.root.name == RGT_PlayerPrefs.playableVehicles.playerName) {
					sceneManager.ChangeTarget (0, waypointNumber);
					if(sceneManager.racerInfo[0].finishedRace) col.transform.parent.parent.SendMessage ("AdjustAIWaypointSpeedFactor", AISpeedFactor, SendMessageOptions.DontRequireReceiver);
				}
			} else if (col.transform.root.name == RGT_PlayerPrefs.opponentVehicles.opponentNames[i - 1]) {
				sceneManager.ChangeTarget (i, waypointNumber);
				col.transform.parent.SendMessage ("AdjustAIWaypointSpeedFactor", AISpeedFactor, SendMessageOptions.DontRequireReceiver);
			}
		}
	}

}