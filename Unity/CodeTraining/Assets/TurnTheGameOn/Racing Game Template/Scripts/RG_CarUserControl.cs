using System;
using UnityEngine;
using TurnTheGameOn.RacingGameTemplate;
using TurnTheGameOn.RacingGameTemplate.CrossPlatformInput.PlatformSpecific;

[RequireComponent(typeof (RG_CarController))]
public class RG_CarUserControl : MonoBehaviour{
	
	private RG_CarController carController;
	private float h;
	private float v;
	private float handBrake;

	private void Awake(){
		carController = GetComponent<RG_CarController>();
	}


	private void FixedUpdate()	{
		h = RG_CrossPlatformInputManager.GetAxis("Horizontal");
		v = RG_CrossPlatformInputManager.GetAxis("Vertical");

		if(!RGT_PlayerPrefs.inputData.useMobileController){
			handBrake = RG_CrossPlatformInputManager.GetAxis("Emergency Brake");
			carController.Move(h, v, v, handBrake);
		} else{
			carController.Move(h, v, v, 0f);
		}

	}
}