using UnityEngine;
using System.Collections;

public class RG_AIDetectionBox : MonoBehaviour {

	public RG_AIDetection detection;

	void Awake(){
		if (detection == null)	detection = transform.parent.parent.GetComponent<RG_AIDetection> ();
	}

	void OnTriggerStay(Collider col){
		if (col.tag == "Car Collider") detection.forwardObstacle = true;
	}

	void OnTriggerExit(Collider col){
		detection.forwardObstacle = false;
	}

}