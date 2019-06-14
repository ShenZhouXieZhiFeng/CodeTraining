using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TurnTheGameOn.RacingGameTemplate;

public class RG_SettingsManager : MonoBehaviour {
	
	public Toggle audioToggle;
	public Toggle automaticTransmissionToggle;
	public Text qualityText;
	public string qualityTextPrefix;
	public GameObject mobileSteering;
	public Dropdown mobileSteeringdropdown;

	void Start(){
		if (RGT_PlayerPrefs.inputData.useMobileController) {
			mobileSteering.SetActive (true);
			mobileSteeringdropdown.value = RGT_PlayerPrefs.GetMobileSteeringType();
		} else {
			mobileSteering.SetActive (false);
		}
		int qualityNumber = RGT_PlayerPrefs.GetQualitySettings ();
		QualitySettings.SetQualityLevel(qualityNumber, true);
		qualityText.text = qualityTextPrefix + QualitySettings.names[QualitySettings.GetQualityLevel()];
		// check saved audio preferences
		if(RGT_PlayerPrefs.GetAudio() == "OFF") audioToggle.isOn = false;
		if(RGT_PlayerPrefs.GetAudio() == "ON") audioToggle.isOn = true;
		UpdateAudio();
		// check saved transmission preferences
		if(RGT_PlayerPrefs.GetTransmission() == "Auto") automaticTransmissionToggle.isOn = true;
		if(RGT_PlayerPrefs.GetTransmission() == "Manual") automaticTransmissionToggle.isOn = false;
		UpdateTransmissionType ();
	}

	public void UpdateAudio(){
		if(audioToggle.isOn){
			AudioListener.pause = false;
			RGT_PlayerPrefs.SetAudio ("ON");
		}else {
			AudioListener.pause = true;
			RGT_PlayerPrefs.SetAudio ("OFF");
		}
	}

	public void QualityUp(){
		QualitySettings.IncreaseLevel();
		qualityText.text = qualityTextPrefix + QualitySettings.names[QualitySettings.GetQualityLevel()];
		RGT_PlayerPrefs.SetQualitySettings (QualitySettings.GetQualityLevel());
	}
	
	public void QualityDown(){
		QualitySettings.DecreaseLevel();
		qualityText.text = qualityTextPrefix + QualitySettings.names[QualitySettings.GetQualityLevel()];
		RGT_PlayerPrefs.SetQualitySettings (QualitySettings.GetQualityLevel());
	}

	public void UpdateMobileSteering(){
		for(int i = 0; i < mobileSteeringdropdown.options.Count; i++){
			if(mobileSteeringdropdown.value == i){
				RGT_PlayerPrefs.SetMobileSteeringType (i);
			}
		}
	}

	public void UpdateTransmissionType () {
		if(automaticTransmissionToggle.isOn){
			RGT_PlayerPrefs.playableVehicles.manual = false;
			RGT_PlayerPrefs.SetTransmission ("Auto");
		}else {
			RGT_PlayerPrefs.playableVehicles.manual = true;
			RGT_PlayerPrefs.SetTransmission ("Manual");
		}
	}

}