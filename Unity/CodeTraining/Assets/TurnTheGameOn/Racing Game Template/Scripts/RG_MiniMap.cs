using System;
using UnityEngine;

namespace TurnTheGameOn.RacingGameTemplate{
	public class RG_MiniMap : MonoBehaviour {

		public bool fixedRotation;
	    public Transform target;
	    public Vector3 offset = new Vector3(0f, 7.5f, 0f);

		void Start(){
			fixedRotation = RGT_PlayerPrefs.playableVehicles.fixedMiniMapRotation;
			transform.parent = null;
		}

	    private void LateUpdate(){
			if (target) {
				
				if (fixedRotation) {
					transform.position = target.position + offset;
					transform.LookAt (target);
				} else {
					Vector3 rotation = new Vector3 (90, target.rotation.eulerAngles.y, 0);
					transform.eulerAngles = rotation;
					transform.position = target.position + offset;
				}

			}
	    }

	}
}