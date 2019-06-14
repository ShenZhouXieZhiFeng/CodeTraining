using System;
using UnityEngine;
using TurnTheGameOn.RacingGameTemplate;

[ExecuteInEditMode]
public class RG_MobileController : MonoBehaviour{

	public GameObject turnLeftButton;
	public GameObject turnRightButton;
	public GameObject steeringJoystick;
	public GameObject tiltInput;
	public GameObject steeringWheel;
	public GameObject shiftUpButton;
	public GameObject shiftDownButton;

//	void Update(){
//		#if UNITY_EDITOR
//		if(!Application.isPlaying)	EnableControlRig (RGT_PlayerPrefs.inputData.useMobileController);
//		#endif
//	}
//
	void OnEnable (){
		EnableControlRig (RGT_PlayerPrefs.inputData.useMobileController);
	}

	void EnableControlRig (bool enabled){
		foreach (Transform t in transform){
			t.gameObject.SetActive(enabled);
		}

		steeringJoystick.SetActive (false);
		turnLeftButton.SetActive (false);
		turnRightButton.SetActive (false);
		tiltInput.SetActive (false);
		steeringWheel.SetActive (false);

		int mobileType = RGT_PlayerPrefs.GetMobileSteeringType ();
		switch (mobileType) {
		case 0:									//Arrow Button Steering
			turnLeftButton.SetActive (true);
			turnRightButton.SetActive (true);
			break;
		case 1:									//Tilt Steering
			tiltInput.SetActive (true);
			break;
		case 2:									//Joystick Steering
			steeringJoystick.SetActive (true);
			break;
		case 3:									//Steering Wheel
			steeringWheel.SetActive (true);
			break;
		}

		shiftUpButton.SetActive (RGT_PlayerPrefs.playableVehicles.manual);
		shiftDownButton.SetActive (RGT_PlayerPrefs.playableVehicles.manual);

	}

}