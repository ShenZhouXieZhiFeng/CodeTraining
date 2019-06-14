using UnityEngine;
using System.Collections;

namespace TurnTheGameOn.RacingGameTemplate{
	public class RG_WaypointArrow : MonoBehaviour {

		public GameObject wrongWayWarning;
		private float wrongWayTimer;
		private bool wrongWay;
		private Transform waypointArrow;

		void Update(){
			if (waypointArrow == null) {
				GameObject findObject = GameObject.Find ("Waypoint Arrow");
				if (findObject != null) {
					waypointArrow = findObject.transform;
					if (RGT_PlayerPrefs.raceData.showWaypointArrow[RGT_PlayerPrefs.raceData.raceNumber]) {

					} else {
						waypointArrow.gameObject.SetActive (false);
						enabled = false;
					}
				}
			} else {
				waypointArrow.LookAt (RG_SceneManager.instance.racerInfo [0].currentWaypoint);
				Vector3 forward = RG_SceneManager.instance.racerInfo [0].racer.transform.TransformDirection (Vector3.forward);
				Vector3 toOther = RG_SceneManager.instance.racerInfo [0].currentWaypoint.localPosition - RG_SceneManager.instance.racerInfo [0].racer.transform.localPosition;
				if (Vector3.Dot (forward, toOther) < 0) {
					wrongWay = true;
				} else {
					wrongWay = false;
				}
			}
			WrongWayDetection ();
		}

		void WrongWayDetection(){
			if (wrongWay) {
				wrongWayTimer += 1 * Time.deltaTime;
				if (wrongWayTimer >= RGT_PlayerPrefs.raceData.wrongWayDelay) {
					wrongWayWarning.SetActive (true);
				}
			} else {
				wrongWayTimer = 0;
				wrongWayWarning.SetActive (false);
			}
		}

		public void DisableSystem(){
			waypointArrow.gameObject.SetActive (false);
			enabled = false;
		}


	}
}