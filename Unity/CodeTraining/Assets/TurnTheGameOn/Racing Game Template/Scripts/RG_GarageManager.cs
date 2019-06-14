using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TurnTheGameOn.RacingGameTemplate;
using System.Collections;
using System;

[ExecuteInEditMode]
public class RG_GarageManager : MonoBehaviour {
	public enum Switch { On, Off }
	public enum MusicSelection { ListOrder, Random }
	[System.Serializable]
	public class GarageSceneReference{
		public string developerAddress;
		public string reviewAddress;

		[Header("Text")]
		public Text currencyText;
		public Text raceNameText;
		public Text carNameText;
		public Text carSpeedText;
		public Text rewardText;
		public Text lapText;
		public Text numberOfRacersText;
		public Text selectRaceText;
		public Text selectCarText;
		public Text selectPaintText;
		public Text selectGlowText;
		public Text selectBrakeColorText;
		public Text selectRimColorText;
		public Text selectGlassTintText;
		public Text topSpeedPriceText;
		public Text accelerationPriceText;
		public Text brakePowerPriceText;
		public Text tireTractionPriceText;
		public Text steerSensetivityPriceText;
		public Text buyCarText;
		public Text buyPaintText;
		public Text buyGlowText;
		public Text buyGlassTintText;
		public Text buyBrakeColorText;
		public Text buyRimColorText;
		public Text unlockLevelButtonText;
		public Text unlockLevelText;
		public Text upgradeConfirmText;
		[Header("Sliders")]
		public Slider redSlider;
		public Slider blueSlider;
		public Slider greenSlider;
		public Slider redGlowSlider;
		public Slider blueGlowSlider;
		public Slider greenGlowSlider;
		public Slider redBrakeSlider;
		public Slider blueBrakeSlider;
		public Slider greenBrakeSlider;
		public Slider redGlassSlider;
		public Slider blueGlassSlider;
		public Slider greenGlassSlider;
		public Slider alphaGlassSlider;
		public Slider redRimSlider;
		public Slider blueRimSlider;
		public Slider greenRimSlider;
		public Slider topSpeedSlider;
		public Slider accelerationSlider;
		public Slider brakePowerSlider;
		public Slider tireTractionSlider;
		public Slider steerSensitivitySlidetr;
		[Header("Garage Menu Windows")]
		public GameObject mainMenuWindow;
		public GameObject settingsMenuWindow;
		public GameObject garageUI;
		public GameObject quitConfirmWindow;
		public GameObject buyCarConfirmWindow;
		public GameObject buyCarButton;
		public GameObject carConfirmWindow;
		public GameObject paintWindow;
		public GameObject rimColorWindow;
		public GameObject glassColorWindow;
		public GameObject brakeColorWindow;
		public GameObject paintConfirmWindow;
		public GameObject rimColorConfirmWindow;
		public GameObject glassColorConfirmWindow;
		public GameObject brakeColorConfirmWindow;
		public GameObject glowWindow;
		public GameObject glowConfirmWindow;
		public GameObject upgradesWindow;
		public GameObject upgradesConfirmWindow;
		public GameObject raceDetailsWindow;
		public GameObject unlockRaceConfirmWindow;
		public GameObject loadingWindow;
		public GameObject raceDetails;
		public GameObject racesWindow;
		public GameObject multiplayerWindow;
		public GameObject singlePlayerModeWindow;
		public GameObject multiplayerModeWindow;
		public GameObject LANWindow;
		public GameObject paintShopWindow;
		public GameObject garageCarSelectionWindow;
		public GameObject currencyAndCarTextWindow;
		[Header("Other")]
		public GameObject raceLockedIcon;
		public GameObject unlockRaceButton;
		public GameObject upgradeTopSpeedButton;
		public GameObject upgradeAccelerationButton;
		public GameObject upgradeBrakePowerButton;
		public GameObject upgradeTireTractionButton;
		public GameObject upgradeSteerSensitivityButton;
		public GameObject selectCarButton;
		public GameObject mainCaameraObject;
		public Button[] carBodyColorButtons;
		public Button[] carGlassColorButtons;
		public Button[] carBrakeColorButtons;
		public Button[] carRimColorButtons;
		public Button[] carNeonColorButtons;
	}

	//public RaceData RGT_PlayerPrefs.raceData;
	//public PlayableVehicles RGT_PlayerPrefs.playableVehicles;
	//public PlayerPrefsData RGT_PlayerPrefs.playerPrefsData;
	//public AudioData RGT_PlayerPrefs.audioData;
	public GameObject lobbyManager;
	//public Switch configureRaceSize;
	public Switch configureCarSize;
	[Tooltip("Enable an option to increase or decrease the amount of races available.")]

	public GarageSceneReference uI;
	public ParticleSystem[] sceneCarGlowLight;
	public GameObject[] sceneCarModel;
	public Image raceImage;
	private GameObject audioContainer;
	private GameObject emptyObject;
	private AudioSource garageAudioSource;
	private bool colorChange;
	private bool brakeColorChange;
	private bool glassColorChange;
	private bool rimColorChange;
	private bool glowChange;
	private bool upgradeChange;
	private bool quitConfirmIsOpen;
	private Color carColor;
	private int vehicleNumber;
	private int raceNumber;
	private int currency;
	[Range(0,1f)] private float carAlpha;
	private int totalRaces;
	private string raceNameToLoad;
	private float cameraOffset;
	public float cameraOffsetDefault;
	public float cameraOffsetUpgrades;
	public float cameraShiftSpeed;
	private Vector3 velocity = Vector3.zero;
	public ReflectionProbe reflectProbe;
	public RG_RotateAround cameraControl;
	int firstPrize;
	int secondPrize;
	int thirdPrize;
	float levelBonusTopSpeed;
	float levelBonusAcceleration;
	float levelBonusBrakePower;
	float levelBonusTireTraction;
	float levelBonusSteerSensitivity;
//	public string currentVersion;


	void Start () {
//		if (PlayerPrefs.GetString("CURRENTVERSION") != currentVersion)  {
//			PlayerPrefs.DeleteAll();
//			PlayerPrefs.SetString("CURRENTVERSION", currentVersion);
//		}
		if (Application.isPlaying) {
			cameraOffset = cameraOffsetDefault;
			audioContainer = new GameObject ();
			audioContainer.name = "Audio Container";
			audioContainer.transform.SetParent (gameObject.transform);
			Time.timeScale = 1.0f;
			AudioMusic ();
			GetPlayerData ();
			UpdateCurrency ();
			for (int i = 0; i < RGT_PlayerPrefs.playableVehicles.numberOfCars; i++) {
				sceneCarModel[i].SetActive (false);
				if (i > 0 && RGT_PlayerPrefs.GetVehicleLock(i) == "UNLOCKED") {
					RGT_PlayerPrefs.playableVehicles.carUnlocked[i] = true;
				}
				if (i == vehicleNumber) {
					sceneCarModel[i].SetActive (true);
					uI.topSpeedSlider.value = RGT_PlayerPrefs.playableVehicles.topSpeedLevel[i];
					uI.accelerationSlider.value = RGT_PlayerPrefs.playableVehicles.torqueLevel[i];
					uI.brakePowerSlider.value = RGT_PlayerPrefs.playableVehicles.brakeTorqueLevel[i];
					uI.tireTractionSlider.value = RGT_PlayerPrefs.playableVehicles.tireTractionLevel[i];
					uI.steerSensitivitySlidetr.value = RGT_PlayerPrefs.playableVehicles.steerSensitivityLevel[i];
				}
			}

			for(int i = 0; i < uI.carBodyColorButtons.Length; i++){
				Color myColor = RGT_PlayerPrefs.playableVehicles.carBodyColorPreset [i];
				UnityEngine.UI.ColorBlock colorBlock = uI.carBodyColorButtons [i].colors;
				colorBlock.normalColor = myColor;
				colorBlock.highlightedColor = myColor;
				myColor.a = 0.25f;
				colorBlock.pressedColor = myColor;
				uI.carBodyColorButtons [i].colors = colorBlock;

				myColor = RGT_PlayerPrefs.playableVehicles.carGlassColorPreset [i];
				colorBlock = uI.carGlassColorButtons [i].colors;
				colorBlock.normalColor = myColor;
				colorBlock.highlightedColor = myColor;
				myColor.a = 0.25f;
				colorBlock.pressedColor = myColor;				
				uI.carGlassColorButtons [i].colors = colorBlock;

				myColor = RGT_PlayerPrefs.playableVehicles.carBrakeColorPreset [i];
				colorBlock = uI.carBrakeColorButtons [i].colors;
				colorBlock.normalColor = myColor;
				colorBlock.highlightedColor = myColor;
				myColor.a = 0.25f;
				colorBlock.pressedColor = myColor;				
				uI.carBrakeColorButtons [i].colors = colorBlock;

				myColor = RGT_PlayerPrefs.playableVehicles.carRimColorPreset [i];
				colorBlock = uI.carRimColorButtons [i].colors;
				colorBlock.normalColor = myColor;
				colorBlock.highlightedColor = myColor;
				myColor.a = 0.25f;
				colorBlock.pressedColor = myColor;				
				uI.carRimColorButtons [i].colors = colorBlock;

				myColor = RGT_PlayerPrefs.playableVehicles.carGlassColorPreset [i];
				colorBlock = uI.carNeonColorButtons [i].colors;
				colorBlock.normalColor = myColor;
				colorBlock.highlightedColor = myColor;
				myColor.a = 0.25f;
				colorBlock.pressedColor = myColor;				
				uI.carNeonColorButtons [i].colors = colorBlock;
			}
			uI.topSpeedPriceText.text = RGT_PlayerPrefs.playableVehicles.upgradeSpeedPrice.ToString ("C0");
			uI.accelerationPriceText.text = RGT_PlayerPrefs.playableVehicles.upgradeAccelerationPrice.ToString ("C0");
			uI.brakePowerPriceText.text = RGT_PlayerPrefs.playableVehicles.upgradeBrakesPrice.ToString ("C0");
			uI.tireTractionPriceText.text = RGT_PlayerPrefs.playableVehicles.upgradeTiresPrice.ToString ("C0");
			uI.steerSensetivityPriceText.text = RGT_PlayerPrefs.playableVehicles.upgradeSteeringPrice.ToString ("C0");
			uI.buyGlowText.text = "Change Neon Light\nfor\n$" + RGT_PlayerPrefs.playableVehicles.glowPrice.ToString ("N0");
			uI.buyPaintText.text = "Paint this car\nfor\n$" + RGT_PlayerPrefs.playableVehicles.paintPrice.ToString ("N0");
			uI.buyBrakeColorText.text = "Change Brake Color\nfor\n$" + RGT_PlayerPrefs.playableVehicles.brakeColorPrice.ToString ("N0");
			uI.buyRimColorText.text = "Change Rim Color\nfor\n$" + RGT_PlayerPrefs.playableVehicles.rimColorPrice.ToString ("N0");
			uI.buyGlassTintText.text = "Change Glass Tint\nfor\n$" + RGT_PlayerPrefs.playableVehicles.glassColorPrice.ToString ("N0");
			uI.selectGlowText.text = "$" + RGT_PlayerPrefs.playableVehicles.glowPrice.ToString ("N0");
			uI.selectPaintText.text = "$" + RGT_PlayerPrefs.playableVehicles.paintPrice.ToString ("N0");
			uI.selectBrakeColorText.text = "$" + RGT_PlayerPrefs.playableVehicles.brakeColorPrice.ToString ("N0");
			uI.selectRimColorText.text = "$" + RGT_PlayerPrefs.playableVehicles.rimColorPrice.ToString ("N0");
			uI.selectGlassTintText.text = "$" + RGT_PlayerPrefs.playableVehicles.glassColorPrice.ToString ("N0");
			uI.carNameText.text = RGT_PlayerPrefs.playableVehicles.vehicleNames[vehicleNumber];
			UpdateRaceDetails ();
			if (RGT_PlayerPrefs.raceData.purchaseLevelUnlock == RaceData.Switch.Off) {
				uI.unlockRaceButton.SetActive (false);
			} else {
				uI.unlockRaceButton.SetActive (true);
			}
		}
		UpdateCar ();
		carColor.a = 0.1f;
		carColor.r = RGT_PlayerPrefs.playerPrefsData.redGlowValues[vehicleNumber];
		carColor.b = RGT_PlayerPrefs.playerPrefsData.blueGlowValues[vehicleNumber];
		carColor.g = RGT_PlayerPrefs.playerPrefsData.greenGlowValues[vehicleNumber];
	}

	// Called on Scene start to load data from PlayerPrefs and Scriptable Objects
	public void GetPlayerData(){
		for(int i = 0; i < RGT_PlayerPrefs.playableVehicles.numberOfCars; i++){
			RGT_PlayerPrefs.playableVehicles.topSpeedLevel [i] = RGT_PlayerPrefs.GetVehicleTopSpeedLevel (i);
			RGT_PlayerPrefs.playableVehicles.torqueLevel [i] = RGT_PlayerPrefs.GetVehicleAccelerationLevel (i);
			RGT_PlayerPrefs.playableVehicles.brakeTorqueLevel [i] = RGT_PlayerPrefs.GetVehicleBrakePowerLevel (i);
			RGT_PlayerPrefs.playableVehicles.tireTractionLevel[i] = RGT_PlayerPrefs.GetVehicleTireTractionLevel (i);
			RGT_PlayerPrefs.playableVehicles.steerSensitivityLevel[i] = RGT_PlayerPrefs.GetVehicleSteerSensitivityLevel (i);
			//get vehicle body color values
			RGT_PlayerPrefs.playerPrefsData.redValues[i] = RGT_PlayerPrefs.GetVehicleBodyColor("Red", i, RGT_PlayerPrefs.playableVehicles.defaultBodyColors [i].r);
			RGT_PlayerPrefs.playerPrefsData.blueValues[i] = RGT_PlayerPrefs.GetVehicleBodyColor("Blue", i, RGT_PlayerPrefs.playableVehicles.defaultBodyColors [i].b);
			RGT_PlayerPrefs.playerPrefsData.greenValues[i] = RGT_PlayerPrefs.GetVehicleBodyColor("Green", i, RGT_PlayerPrefs.playableVehicles.defaultBodyColors [i].g);
			//get vehicle neon light color values
			RGT_PlayerPrefs.playerPrefsData.redGlowValues[i] = RGT_PlayerPrefs.GetVehicleNeonLightColor("Red", i, RGT_PlayerPrefs.playableVehicles.defaultNeonColors [i].r);
			RGT_PlayerPrefs.playerPrefsData.blueGlowValues[i] = RGT_PlayerPrefs.GetVehicleNeonLightColor("Blue", i, RGT_PlayerPrefs.playableVehicles.defaultNeonColors [i].b);
			RGT_PlayerPrefs.playerPrefsData.greenGlowValues[i] = RGT_PlayerPrefs.GetVehicleNeonLightColor("Green", i, RGT_PlayerPrefs.playableVehicles.defaultNeonColors [i].g);
			//get vehicle brake color values
			RGT_PlayerPrefs.playerPrefsData.redBrakeValues[i] = RGT_PlayerPrefs.GetVehicleBrakeColor("Red", i, RGT_PlayerPrefs.playableVehicles.defaultBrakeColors [i].r);
			RGT_PlayerPrefs.playerPrefsData.blueBrakeValues[i] = RGT_PlayerPrefs.GetVehicleBrakeColor("Blue", i, RGT_PlayerPrefs.playableVehicles.defaultBrakeColors [i].b);
			RGT_PlayerPrefs.playerPrefsData.greenBrakeValues[i] = RGT_PlayerPrefs.GetVehicleBrakeColor("Green", i, RGT_PlayerPrefs.playableVehicles.defaultBrakeColors [i].g);
			//get vehicle rim color values
			RGT_PlayerPrefs.playerPrefsData.redRimValues[i] = RGT_PlayerPrefs.GetVehicleRimColor("Red", i, RGT_PlayerPrefs.playableVehicles.defaultRimColors [i].r);
			RGT_PlayerPrefs.playerPrefsData.blueRimValues[i] = RGT_PlayerPrefs.GetVehicleRimColor("Blue", i, RGT_PlayerPrefs.playableVehicles.defaultRimColors [i].b);
			RGT_PlayerPrefs.playerPrefsData.greenRimValues[i] = RGT_PlayerPrefs.GetVehicleRimColor("Green", i, RGT_PlayerPrefs.playableVehicles.defaultRimColors [i].g);
			//get vehicle glass color values
			RGT_PlayerPrefs.playerPrefsData.redGlassValues[i] = RGT_PlayerPrefs.GetVehicleGlassColor("Red", i, RGT_PlayerPrefs.playableVehicles.defaultGlassColors [i].r);
			RGT_PlayerPrefs.playerPrefsData.blueGlassValues[i] = RGT_PlayerPrefs.GetVehicleGlassColor("Blue", i, RGT_PlayerPrefs.playableVehicles.defaultGlassColors [i].b);
			RGT_PlayerPrefs.playerPrefsData.greenGlassValues[i] = RGT_PlayerPrefs.GetVehicleGlassColor("Green", i, RGT_PlayerPrefs.playableVehicles.defaultGlassColors [i].g);
			RGT_PlayerPrefs.playerPrefsData.alphaGlassValues[i] = RGT_PlayerPrefs.GetVehicleGlassColor("Alpha", i, RGT_PlayerPrefs.playableVehicles.defaultGlassColors [i].a);

			carColor.a = carAlpha;
			carColor.r = RGT_PlayerPrefs.playerPrefsData.redValues[i];
			carColor.b = RGT_PlayerPrefs.playerPrefsData.blueValues[i];
			carColor.g = RGT_PlayerPrefs.playerPrefsData.greenValues[i];
			RGT_PlayerPrefs.playableVehicles.carMaterial[i].color = carColor;
			carColor.a = 0.1f;
			carColor.r = RGT_PlayerPrefs.playerPrefsData.redGlowValues[i];
			carColor.b = RGT_PlayerPrefs.playerPrefsData.blueGlowValues[i];
			carColor.g = RGT_PlayerPrefs.playerPrefsData.greenGlowValues[i];
			var main = RGT_PlayerPrefs.playableVehicles.carGlowLight [i].main;
			main.startColor = carColor;


			var main0 = sceneCarGlowLight[i].main;
			main0.startColor = carColor;


			carColor.a = carAlpha;
			carColor.r = RGT_PlayerPrefs.playerPrefsData.redBrakeValues[i];
			carColor.b = RGT_PlayerPrefs.playerPrefsData.blueBrakeValues[i];
			carColor.g = RGT_PlayerPrefs.playerPrefsData.greenBrakeValues[i];
			RGT_PlayerPrefs.playableVehicles.brakeMaterial[i].color = carColor;
			carColor.a = carAlpha;
			carColor.r = RGT_PlayerPrefs.playerPrefsData.redRimValues[i];
			carColor.b = RGT_PlayerPrefs.playerPrefsData.blueRimValues[i];
			carColor.g = RGT_PlayerPrefs.playerPrefsData.greenRimValues[i];
			RGT_PlayerPrefs.playableVehicles.rimMaterial[i].color = carColor;
			carColor.a = RGT_PlayerPrefs.playerPrefsData.alphaGlassValues[i];
			carColor.r = RGT_PlayerPrefs.playerPrefsData.redGlassValues[i];
			carColor.b = RGT_PlayerPrefs.playerPrefsData.blueGlassValues[i];
			carColor.g = RGT_PlayerPrefs.playerPrefsData.greenGlassValues[i];
			RGT_PlayerPrefs.playableVehicles.glassMaterial[i].color = carColor;

			var main1 = RGT_PlayerPrefs.playableVehicles.carGlowLight[i].main;
			main1.startColor = carColor;
			sceneCarGlowLight[i].gameObject.SetActive(false);
			sceneCarGlowLight[i].gameObject.SetActive(true);

		}
		raceNumber = RGT_PlayerPrefs.GetRaceNumber ();
		//get race laps
		for(int i = 0; i < RGT_PlayerPrefs.raceData.numberOfRaces; i++){
			RGT_PlayerPrefs.GetRaceLaps (i, RGT_PlayerPrefs.raceData.raceLaps [raceNumber]);
		}
		//get vehicle number
		vehicleNumber = RGT_PlayerPrefs.GetVehicleNumber();
		RGT_PlayerPrefs.playableVehicles.currentVehicleNumber = vehicleNumber;
		//get currency
		currency = RGT_PlayerPrefs.GetCurrency ();
		if(currency == 0){
			//set starting currency
			currency = RGT_PlayerPrefs.playableVehicles.startingCurrency;
			RGT_PlayerPrefs.SetCurrency (RGT_PlayerPrefs.playableVehicles.startingCurrency);
		}
		//set number of races
		RGT_PlayerPrefs.SetNumberOfRaces (RGT_PlayerPrefs.raceData.numberOfRaces);
		for(int i = 1; i < RGT_PlayerPrefs.raceData.numberOfRaces; i++){
			//set race names
			RGT_PlayerPrefs.SetRaceName(i, RGT_PlayerPrefs.raceData.raceNames[i]);
			if (RGT_PlayerPrefs.raceData.raceLocked [i]) {
				//get race locks
				if (RGT_PlayerPrefs.GetRaceLock(RGT_PlayerPrefs.raceData.raceNames [i]) == "UNLOCKED") {
					RGT_PlayerPrefs.raceData.raceLocked [i] = false;
				} else {
					RGT_PlayerPrefs.raceData.raceLocked [i] = true;
				}
			}
		}
		//raceDetails[raceNumber].bestPosition = PlayerPrefs.GetInt ("Best Position" + raceNumber.ToString(), 8);
		//raceDetails[raceNumber].bestTime = PlayerPrefs.GetFloat ("Best Time" + raceNumber.ToString(), 9999.99f);
		//raceDetails[raceNumber].bestLapTime = PlayerPrefs.GetFloat ("Best Lap Time" + raceNumber.ToString(), 9999.99f);

	}

	// Here we check to see if the player is in a car part color modification menu and update the color changes in real time as they adjust sliders
	public void Update(){
		if(Application.isPlaying){
			if(RGT_PlayerPrefs.audioData.music.Length > 0){
				if(!garageAudioSource.isPlaying)		PlayNextAudioTrack ();
			}
			if(colorChange)	CarColor ();
			if(brakeColorChange) BrakeColor ();
			if(rimColorChange) RimColor ();
			if(glassColorChange) GlassColor ();
			if(glowChange) GlowColor();
		}
		Vector3 pos = uI.mainCaameraObject.transform.localPosition;
		pos.x = cameraOffset;;
		uI.mainCaameraObject.transform.localPosition = Vector3.SmoothDamp (uI.mainCaameraObject.transform.localPosition, pos, ref velocity, cameraShiftSpeed);
	}

	// These methods are used for the main menu
	#region MainMenu Methods
	// Loads garage menu
	public void Button_Play() {
		if (uI.mainMenuWindow.activeInHierarchy) {
			uI.mainMenuWindow.SetActive(false);
			uI.garageUI.SetActive(true);
			uI.currencyAndCarTextWindow.SetActive (true);
			AudioMenuSelect();
		}
		else {
			if (RGT_PlayerPrefs.GetVehicleLock(vehicleNumber) != "UNLOCKED"){
				sceneCarModel[vehicleNumber].SetActive(false);
				vehicleNumber = 0;
				RGT_PlayerPrefs.playableVehicles.currentVehicleNumber = vehicleNumber;
				uI.carNameText.text = RGT_PlayerPrefs.playableVehicles.vehicleNames[vehicleNumber];
				sceneCarModel[vehicleNumber].SetActive(true);
			}
			uI.currencyAndCarTextWindow.SetActive (false);
			uI.buyCarConfirmWindow.SetActive(false);
			for (int i = 0; i < RGT_PlayerPrefs.playableVehicles.numberOfCars; i++){
				sceneCarModel[i].SetActive(false);
				if(i == vehicleNumber){
					sceneCarModel[i].SetActive(true);
				}
			}
			uI.garageUI.SetActive(false);
			uI.mainMenuWindow.SetActive(true);
			uI.topSpeedSlider.value = RGT_PlayerPrefs.playableVehicles.topSpeedLevel[vehicleNumber];
			uI.accelerationSlider.value = RGT_PlayerPrefs.playableVehicles.torqueLevel[vehicleNumber];
			uI.brakePowerSlider.value = RGT_PlayerPrefs.playableVehicles.brakeTorqueLevel[vehicleNumber];
			uI.tireTractionSlider.value = RGT_PlayerPrefs.playableVehicles.tireTractionLevel[vehicleNumber];
			uI.steerSensitivitySlidetr.value = RGT_PlayerPrefs.playableVehicles.steerSensitivityLevel[vehicleNumber];
			AudioMenuBack();
		}
		UpdateCar();
	}

	// Loads the options menu
	public void Button_Settings() {
		if (uI.settingsMenuWindow.activeInHierarchy) {
			uI.garageUI.SetActive(true);
			uI.settingsMenuWindow.SetActive(false);
			AudioMenuBack();
		}
		else {
			uI.settingsMenuWindow.SetActive(true);
			uI.garageUI.SetActive(false);
			AudioMenuSelect();
		}
	}

	// Prompts user with a quit confirm window
	public void Button_QuitGame() {
		if (quitConfirmIsOpen) {
			quitConfirmIsOpen = false;
			uI.quitConfirmWindow.SetActive(false);
		} else {
			quitConfirmIsOpen = true;
			uI.quitConfirmWindow.SetActive(true);
		}
	}

	//Closes the quit confirm window
	public void Button_QuitConfirm(){
		Application.Quit ();
	}

	public void Button_Review() {
		Application.OpenURL(uI.reviewAddress);
	}

	public void Button_More() {
		Application.OpenURL(uI.developerAddress);
	}
	#endregion

	// These methods check player currency and prompt player with a purchase confirmation window if the player has enough currency
	#region SelectItemToPurchase Methods

	public void Button_Garage(){
		if (uI.garageCarSelectionWindow.activeInHierarchy) {
			cameraOffset = cameraOffsetDefault;
			uI.garageCarSelectionWindow.SetActive (false);
			uI.garageUI.SetActive (true);
		} else {
			cameraOffset = 0;
			uI.garageCarSelectionWindow.SetActive (true);
			uI.garageUI.SetActive (false);
		}
	}

	public void Button_PaintShop(){
		if (uI.paintShopWindow.activeInHierarchy) {
			cameraOffset = cameraOffsetDefault;
			uI.paintShopWindow.SetActive (false);
			uI.garageUI.SetActive (true);
		} else {
			uI.paintShopWindow.SetActive (true);
			uI.garageUI.SetActive (false);
		}
	}

	public void Button_Upgrades() {		
		if (upgradeChange) {
			cameraOffset = cameraOffsetDefault;
			upgradeChange = false;
			uI.upgradesWindow.SetActive(false);
			uI.upgradesConfirmWindow.SetActive(false);
			uI.garageUI.SetActive (true);
		}
		else {
			cameraOffset = cameraOffsetUpgrades;
			uI.garageUI.SetActive (false);
			uI.upgradesWindow.SetActive(true);
			AudioMenuSelect();
			upgradeChange = true;
		}
	}

	public void Button_MultiplayerMenu(){
		if (uI.multiplayerModeWindow.activeInHierarchy) {
			uI.multiplayerModeWindow.SetActive (false);
			uI.garageUI.SetActive (true);
		} else {
			uI.garageUI.SetActive (false);
			uI.multiplayerModeWindow.SetActive (true);
		}
	}

	public void Button_SinglePlayerMenu(){
		if (uI.singlePlayerModeWindow.activeInHierarchy) {
			uI.singlePlayerModeWindow.SetActive (false);
			uI.garageUI.SetActive (true);
		} else {
			uI.garageUI.SetActive (false);
			uI.singlePlayerModeWindow.SetActive (true);
		}
	}

	public void Button_BuyCar() {
		uI.buyCarText.text = "Buy " + RGT_PlayerPrefs.playableVehicles.vehicleNames[vehicleNumber].ToString() + "\nfor\n$" + RGT_PlayerPrefs.playableVehicles.price[vehicleNumber].ToString("N0");
		if (currency >= RGT_PlayerPrefs.playableVehicles.price[vehicleNumber])
			uI.carConfirmWindow.SetActive(true);
		AudioMenuSelect();
	}

	public void Button_BodyColorChange(int buttonNumber){
		uI.redSlider.value = RGT_PlayerPrefs.playableVehicles.carBodyColorPreset[buttonNumber].r;
		uI.blueSlider.value = RGT_PlayerPrefs.playableVehicles.carBodyColorPreset[buttonNumber].b;
		uI.greenSlider.value = RGT_PlayerPrefs.playableVehicles.carBodyColorPreset[buttonNumber].g;
	}

	public void Button_GlassColorChange(int buttonNumber){
		uI.redGlassSlider.value = RGT_PlayerPrefs.playableVehicles.carGlassColorPreset[buttonNumber].r;
		uI.blueGlassSlider.value = RGT_PlayerPrefs.playableVehicles.carGlassColorPreset[buttonNumber].b;
		uI.greenGlassSlider.value = RGT_PlayerPrefs.playableVehicles.carGlassColorPreset[buttonNumber].g;
	}

	public void Button_BrakeColorChange(int buttonNumber){
		uI.redBrakeSlider.value = RGT_PlayerPrefs.playableVehicles.carBrakeColorPreset[buttonNumber].r;
		uI.blueBrakeSlider.value = RGT_PlayerPrefs.playableVehicles.carBrakeColorPreset[buttonNumber].b;
		uI.greenBrakeSlider.value = RGT_PlayerPrefs.playableVehicles.carBrakeColorPreset[buttonNumber].g;
	}

	public void Button_RimColorChange(int buttonNumber){
		uI.redRimSlider.value = RGT_PlayerPrefs.playableVehicles.carRimColorPreset[buttonNumber].r;
		uI.blueRimSlider.value = RGT_PlayerPrefs.playableVehicles.carRimColorPreset[buttonNumber].b;
		uI.greenRimSlider.value = RGT_PlayerPrefs.playableVehicles.carRimColorPreset[buttonNumber].g;
	}

	public void Button_NeonColorChange(int buttonNumber){
		uI.redGlowSlider.value = RGT_PlayerPrefs.playableVehicles.carNeonColorPreset[buttonNumber].r;
		uI.blueGlowSlider.value = RGT_PlayerPrefs.playableVehicles.carNeonColorPreset[buttonNumber].b;
		uI.greenGlowSlider.value = RGT_PlayerPrefs.playableVehicles.carNeonColorPreset[buttonNumber].g;
	}

	// Loads the Change Paint Color menu for the selected car
	public void Button_BodyColor() {
		if (colorChange) {
			cameraControl.CanControl ();
			cameraOffset = cameraOffsetDefault;
			colorChange = false;
			uI.paintWindow.SetActive(false);
			uI.paintConfirmWindow.SetActive(false);
			uI.paintShopWindow.SetActive (true);
			uI.redSlider.value = RGT_PlayerPrefs.playerPrefsData.redValues[vehicleNumber];
			uI.blueSlider.value = RGT_PlayerPrefs.playerPrefsData.blueValues[vehicleNumber];
			uI.greenSlider.value = RGT_PlayerPrefs.playerPrefsData.greenValues[vehicleNumber];
			CarColor();
		}
		else {
			cameraControl.CanControl ();
			cameraOffset = cameraOffsetUpgrades;
			uI.paintShopWindow.SetActive (false);
			uI.paintWindow.SetActive(true);
			AudioMenuSelect();
			colorChange = true;
			uI.redSlider.value = RGT_PlayerPrefs.playerPrefsData.redValues[vehicleNumber];
			uI.blueSlider.value = RGT_PlayerPrefs.playerPrefsData.blueValues[vehicleNumber];
			uI.greenSlider.value = RGT_PlayerPrefs.playerPrefsData.greenValues[vehicleNumber];
			CarColor();
		}
	}

	// Loads the Change Glass Color menu for the selected car
	public void Button_GlassColor() {
		if (glassColorChange) {
			cameraControl.CanControl ();
			cameraOffset = cameraOffsetDefault;
			glassColorChange = false;
			uI.glassColorWindow.SetActive(false);
			uI.glassColorConfirmWindow.SetActive(false);
			uI.paintShopWindow.SetActive (true);
			uI.redGlassSlider.value = RGT_PlayerPrefs.playerPrefsData.redGlassValues[vehicleNumber];
			uI.blueGlassSlider.value = RGT_PlayerPrefs.playerPrefsData.blueGlassValues[vehicleNumber];
			uI.greenGlassSlider.value = RGT_PlayerPrefs.playerPrefsData.greenGlassValues[vehicleNumber];
			uI.alphaGlassSlider.value = RGT_PlayerPrefs.playerPrefsData.alphaGlassValues[vehicleNumber];
			GlassColor();
		}
		else {
			cameraControl.CanControl ();
			cameraOffset = cameraOffsetUpgrades;
			uI.paintShopWindow.SetActive (false);
			uI.glassColorWindow.SetActive(true);		
			AudioMenuSelect();
			glassColorChange = true;
			uI.redGlassSlider.value = RGT_PlayerPrefs.playerPrefsData.redGlassValues[vehicleNumber];
			uI.blueGlassSlider.value = RGT_PlayerPrefs.playerPrefsData.blueGlassValues[vehicleNumber];
			uI.greenGlassSlider.value = RGT_PlayerPrefs.playerPrefsData.greenGlassValues[vehicleNumber];
			uI.alphaGlassSlider.value = RGT_PlayerPrefs.playerPrefsData.alphaGlassValues[vehicleNumber];
			GlassColor();
		}
	}

	// Loads the Change Brake Color menu for the selected car
	public void Button_BrakeColor() {
		if (brakeColorChange) {
			cameraControl.CanControl ();
			cameraOffset = cameraOffsetDefault;
			brakeColorChange = false;
			uI.brakeColorWindow.SetActive(false);
			uI.brakeColorConfirmWindow.SetActive(false);
			uI.paintShopWindow.SetActive (true);
			uI.redBrakeSlider.value = RGT_PlayerPrefs.playerPrefsData.redBrakeValues[vehicleNumber];
			uI.blueBrakeSlider.value = RGT_PlayerPrefs.playerPrefsData.blueBrakeValues[vehicleNumber];
			uI.greenBrakeSlider.value = RGT_PlayerPrefs.playerPrefsData.greenBrakeValues[vehicleNumber];
			BrakeColor();
		}
		else {
			cameraControl.CanControl ();
			cameraOffset = cameraOffsetUpgrades;
			uI.paintShopWindow.SetActive (false);
			uI.brakeColorWindow.SetActive(true);		
			AudioMenuSelect();
			brakeColorChange = true;
			uI.redBrakeSlider.value = RGT_PlayerPrefs.playerPrefsData.redBrakeValues[vehicleNumber];
			uI.blueBrakeSlider.value = RGT_PlayerPrefs.playerPrefsData.blueBrakeValues[vehicleNumber];
			uI.greenBrakeSlider.value = RGT_PlayerPrefs.playerPrefsData.greenBrakeValues[vehicleNumber];
			BrakeColor();
		}
	}

	// Loads the Change Rim Color menu for the selected car
	public void Button_RimColor() {
		if (rimColorChange) {
			cameraControl.CanControl ();
			cameraOffset = cameraOffsetDefault;
			rimColorChange = false;
			uI.rimColorWindow.SetActive(false);
			uI.rimColorConfirmWindow.SetActive(false);
			uI.paintShopWindow.SetActive (true);
			uI.redRimSlider.value = RGT_PlayerPrefs.playerPrefsData.redRimValues[vehicleNumber];
			uI.blueRimSlider.value = RGT_PlayerPrefs.playerPrefsData.blueRimValues[vehicleNumber];
			uI.greenRimSlider.value = RGT_PlayerPrefs.playerPrefsData.greenRimValues[vehicleNumber];
			RimColor();
		}
		else {
			cameraControl.CanControl ();
			cameraOffset = cameraOffsetUpgrades;
			uI.paintShopWindow.SetActive (false);
			uI.rimColorWindow.SetActive(true);		
			AudioMenuSelect();
			rimColorChange = true;
			uI.redRimSlider.value = RGT_PlayerPrefs.playerPrefsData.redRimValues[vehicleNumber];
			uI.blueRimSlider.value = RGT_PlayerPrefs.playerPrefsData.blueRimValues[vehicleNumber];
			uI.greenRimSlider.value = RGT_PlayerPrefs.playerPrefsData.greenRimValues[vehicleNumber];
			RimColor();
		}
	}

	// Loads the Change Neon Glow menu for the selected car
	public void Button_NeonLightColor() {
		if (glowChange) {
			cameraControl.CanControl ();
			cameraOffset = cameraOffsetDefault;
			glowChange = false;
			uI.glowWindow.SetActive(false);
			uI.glowConfirmWindow.SetActive(false);
			uI.paintShopWindow.SetActive (true);
			uI.redGlowSlider.value = RGT_PlayerPrefs.playerPrefsData.redGlowValues[vehicleNumber];
			uI.blueGlowSlider.value = RGT_PlayerPrefs.playerPrefsData.blueGlowValues[vehicleNumber];
			uI.greenGlowSlider.value = RGT_PlayerPrefs.playerPrefsData.greenGlowValues[vehicleNumber];
			GlowColor();
		} else {
			cameraControl.CanControl ();
			cameraOffset = cameraOffsetUpgrades;
			uI.paintShopWindow.SetActive (false);
			uI.glowWindow.SetActive(true);		
			AudioMenuSelect();
			glowChange = true;
			uI.redGlowSlider.value = RGT_PlayerPrefs.playerPrefsData.redGlowValues[vehicleNumber];
			uI.blueGlowSlider.value = RGT_PlayerPrefs.playerPrefsData.blueGlowValues[vehicleNumber];
			uI.greenGlowSlider.value = RGT_PlayerPrefs.playerPrefsData.greenGlowValues[vehicleNumber];
			GlowColor();
		}
	}

	public void SelectGlow(){
		AudioMenuSelect ();
		if(currency >= RGT_PlayerPrefs.playableVehicles.glowPrice)	uI.glowConfirmWindow.SetActive(true);
	}    

	public void SelectPaint(){
		AudioMenuSelect ();
		if(currency >= RGT_PlayerPrefs.playableVehicles.paintPrice)	uI.paintConfirmWindow.SetActive(true);
	}

	public void SelectBrakeColor(){
		AudioMenuSelect ();
		if(currency >= RGT_PlayerPrefs.playableVehicles.brakeColorPrice)	uI.brakeColorConfirmWindow.SetActive(true);
	}

	public void SelectRimColor(){
		AudioMenuSelect ();
		if(currency >= RGT_PlayerPrefs.playableVehicles.rimColorPrice)	uI.rimColorConfirmWindow.SetActive(true);
	}

	public void SelectGlassColor(){
		AudioMenuSelect ();
		if(currency >= RGT_PlayerPrefs.playableVehicles.glassColorPrice)	uI.glassColorConfirmWindow.SetActive(true);
	}
	#endregion

	// These methods are used to confirm a purchase and update PlayerPrefs data and other components with the changes
	#region AcceptPurchases Methods

	public void AcceptPurchasePaint(){		
		AudioMenuSelect ();
		//set currency
		RGT_PlayerPrefs.SetCurrency(currency - RGT_PlayerPrefs.playableVehicles.paintPrice);
		//set vehicle body color values
		RGT_PlayerPrefs.SetVehicleBodyColor("Red", vehicleNumber, uI.redSlider.value);
		RGT_PlayerPrefs.SetVehicleBodyColor("Blue", vehicleNumber, uI.blueSlider.value);
		RGT_PlayerPrefs.SetVehicleBodyColor("Green", vehicleNumber, uI.greenSlider.value);
		RGT_PlayerPrefs.playerPrefsData.redValues[vehicleNumber] = uI.redSlider.value;
		RGT_PlayerPrefs.playerPrefsData.blueValues[vehicleNumber] = uI.blueSlider.value;
		RGT_PlayerPrefs.playerPrefsData.greenValues[vehicleNumber] = uI.greenSlider.value;
		uI.paintConfirmWindow.SetActive (false);
		UpdateCurrency ();
		Button_BodyColor ();
	}

	public void AcceptPurchaseGlassColor(){
		AudioMenuSelect ();
		//set currency
		RGT_PlayerPrefs.SetCurrency(currency - RGT_PlayerPrefs.playableVehicles.glassColorPrice);
		//set vehicle glass color values
		RGT_PlayerPrefs.SetVehicleGlassColor("Red", vehicleNumber, uI.redGlassSlider.value);
		RGT_PlayerPrefs.SetVehicleGlassColor("Blue", vehicleNumber, uI.blueGlassSlider.value);
		RGT_PlayerPrefs.SetVehicleGlassColor("Green", vehicleNumber, uI.greenGlassSlider.value);
		RGT_PlayerPrefs.SetVehicleGlassColor("Alpha", vehicleNumber, uI.alphaGlassSlider.value);
		RGT_PlayerPrefs.playerPrefsData.redGlassValues[vehicleNumber] = uI.redGlassSlider.value;
		RGT_PlayerPrefs.playerPrefsData.blueGlassValues[vehicleNumber] = uI.blueGlassSlider.value;
		RGT_PlayerPrefs.playerPrefsData.greenGlassValues[vehicleNumber] = uI.greenGlassSlider.value;
		RGT_PlayerPrefs.playerPrefsData.alphaGlassValues[vehicleNumber] = uI.alphaGlassSlider.value;
		uI.glassColorConfirmWindow.SetActive (false);
		UpdateCurrency ();
		Button_GlassColor ();
	}

	public void AcceptPurchaseBrakeColor(){
		AudioMenuSelect ();
		//set currency
		RGT_PlayerPrefs.SetCurrency(currency - RGT_PlayerPrefs.playableVehicles.brakeColorPrice);
		//set vehicle brake color values
		RGT_PlayerPrefs.SetVehicleBrakeColor("Red", vehicleNumber, uI.redBrakeSlider.value);
		RGT_PlayerPrefs.SetVehicleBrakeColor("Blue", vehicleNumber, uI.blueBrakeSlider.value);
		RGT_PlayerPrefs.SetVehicleBrakeColor("Green", vehicleNumber, uI.greenBrakeSlider.value);
		RGT_PlayerPrefs.playerPrefsData.redBrakeValues[vehicleNumber] = uI.redBrakeSlider.value;
		RGT_PlayerPrefs.playerPrefsData.blueBrakeValues[vehicleNumber] = uI.blueBrakeSlider.value;
		RGT_PlayerPrefs.playerPrefsData.greenBrakeValues[vehicleNumber] = uI.greenBrakeSlider.value;
		uI.brakeColorConfirmWindow.SetActive (false);
		UpdateCurrency ();
		Button_BrakeColor ();
	}

	public void AcceptPurchaseRimColor(){
		AudioMenuSelect ();
		//set currency
		RGT_PlayerPrefs.SetCurrency(currency - RGT_PlayerPrefs.playableVehicles.rimColorPrice);
		//set vehicle rim color values
		RGT_PlayerPrefs.SetVehicleRimColor("Red", vehicleNumber, uI.redRimSlider.value);
		RGT_PlayerPrefs.SetVehicleRimColor("Blue", vehicleNumber, uI.blueRimSlider.value);
		RGT_PlayerPrefs.SetVehicleRimColor("Green", vehicleNumber, uI.greenRimSlider.value);
		RGT_PlayerPrefs.playerPrefsData.redRimValues[vehicleNumber] = uI.redRimSlider.value;
		RGT_PlayerPrefs.playerPrefsData.blueRimValues[vehicleNumber] = uI.blueRimSlider.value;
		RGT_PlayerPrefs.playerPrefsData.greenRimValues[vehicleNumber] = uI.greenRimSlider.value;
		uI.rimColorConfirmWindow.SetActive (false);
		UpdateCurrency ();
		Button_RimColor ();
	}

	public void AcceptPurchaseGlow(){
		AudioMenuSelect ();
		//set currency
		RGT_PlayerPrefs.SetCurrency(currency - RGT_PlayerPrefs.playableVehicles.glowPrice);
		//set vehicle neon light color values
		RGT_PlayerPrefs.SetVehicleNeonLightColor("Red", vehicleNumber, uI.redGlowSlider.value);
		RGT_PlayerPrefs.SetVehicleNeonLightColor("Blue", vehicleNumber, uI.blueGlowSlider.value);
		RGT_PlayerPrefs.SetVehicleNeonLightColor("Green", vehicleNumber, uI.greenGlowSlider.value);
		RGT_PlayerPrefs.playerPrefsData.redGlowValues[vehicleNumber] = uI.redGlowSlider.value;
		RGT_PlayerPrefs.playerPrefsData.blueGlowValues[vehicleNumber] = uI.blueGlowSlider.value;
		RGT_PlayerPrefs.playerPrefsData.greenGlowValues[vehicleNumber] = uI.greenGlowSlider.value;
		uI.glowConfirmWindow.SetActive (false);
		var main = RGT_PlayerPrefs.playableVehicles.carGlowLight[vehicleNumber].main;
		main.startColor = carColor;

		UpdateCurrency ();
		Button_NeonLightColor ();
	}

	public void Button_AcceptUpgrade() {
		if (uI.upgradeConfirmText.text == "Upgrade " + "Top Speed" + "\nfor\n$" + RGT_PlayerPrefs.playableVehicles.upgradeSpeedPrice.ToString("N0")) {
			currency -= RGT_PlayerPrefs.playableVehicles.upgradeSpeedPrice;
			RGT_PlayerPrefs.playableVehicles.topSpeedLevel[vehicleNumber] += 1;
			RGT_PlayerPrefs.SetVehicleTopSpeedLevel(vehicleNumber, RGT_PlayerPrefs.playableVehicles.topSpeedLevel[vehicleNumber]);
		}
		if (uI.upgradeConfirmText.text == "Upgrade " + "Acceleration" + "\nfor\n$" + RGT_PlayerPrefs.playableVehicles.upgradeAccelerationPrice.ToString("N0")) {
			currency -= RGT_PlayerPrefs.playableVehicles.upgradeAccelerationPrice;
			RGT_PlayerPrefs.playableVehicles.torqueLevel[vehicleNumber] += 1;
			RGT_PlayerPrefs.SetVehicleAccelerationLevel(vehicleNumber, RGT_PlayerPrefs.playableVehicles.torqueLevel[vehicleNumber]);
		}
		if (uI.upgradeConfirmText.text == "Upgrade " + "Brake Power" + "\nfor\n$" + RGT_PlayerPrefs.playableVehicles.upgradeBrakesPrice.ToString("N0")) {
			currency -= RGT_PlayerPrefs.playableVehicles.upgradeBrakesPrice;
			RGT_PlayerPrefs.playableVehicles.brakeTorqueLevel[vehicleNumber] += 1;
			RGT_PlayerPrefs.SetVehicleBrakePowerLevel(vehicleNumber, RGT_PlayerPrefs.playableVehicles.brakeTorqueLevel[vehicleNumber]);
		}
		if (uI.upgradeConfirmText.text == "Upgrade " + "Tire Traction" + "\nfor\n$" + RGT_PlayerPrefs.playableVehicles.upgradeTiresPrice.ToString("N0")) {
			currency -= RGT_PlayerPrefs.playableVehicles.upgradeTiresPrice;
			RGT_PlayerPrefs.playableVehicles.tireTractionLevel[vehicleNumber] += 1;
			RGT_PlayerPrefs.SetVehicleTireTractionLevel(vehicleNumber, RGT_PlayerPrefs.playableVehicles.tireTractionLevel[vehicleNumber]);
		}
		if (uI.upgradeConfirmText.text == "Upgrade " + "Steer Sensitivity" + "\nfor\n$" + RGT_PlayerPrefs.playableVehicles.upgradeSteeringPrice.ToString("N0")) {
			currency -= RGT_PlayerPrefs.playableVehicles.upgradeSteeringPrice;
			RGT_PlayerPrefs.playableVehicles.steerSensitivityLevel[vehicleNumber] += 1;
			RGT_PlayerPrefs.SetVehicleSteerSensitivityLevel(vehicleNumber, RGT_PlayerPrefs.playableVehicles.steerSensitivityLevel[vehicleNumber]);
		}
		uI.upgradesConfirmWindow.SetActive(false);
		//set currency
		RGT_PlayerPrefs.SetCurrency(currency);
		UpdateCar();
	}
	#endregion

	// These methods are used to change the color of a car part
	#region ColorChange Methods
	public void GlassColor(){
		carColor.a = uI.alphaGlassSlider.value;
		carColor.r = uI.redGlassSlider.value;
		carColor.b = uI.blueGlassSlider.value;
		carColor.g = uI.greenGlassSlider.value;
		RGT_PlayerPrefs.playableVehicles.glassMaterial[vehicleNumber].color = carColor;
	}

	public void BrakeColor(){
		carColor.a = carAlpha;
		carColor.r = uI.redBrakeSlider.value;
		carColor.b = uI.blueBrakeSlider.value;
		carColor.g = uI.greenBrakeSlider.value;
		RGT_PlayerPrefs.playableVehicles.brakeMaterial[vehicleNumber].color = carColor;
	}

	public void RimColor(){
		carColor.a = carAlpha;
		carColor.r = uI.redRimSlider.value;
		carColor.b = uI.blueRimSlider.value;
		carColor.g = uI.greenRimSlider.value;
		RGT_PlayerPrefs.playableVehicles.rimMaterial[vehicleNumber].color = carColor;
	}

	public void CarColor(){
		carColor.a = carAlpha;
		carColor.r = uI.redSlider.value;
		carColor.b = uI.blueSlider.value;
		carColor.g = uI.greenSlider.value;
		RGT_PlayerPrefs.playableVehicles.carMaterial[vehicleNumber].color = carColor;
	}

	public void GlowColor(){
		if (carColor.g != uI.greenGlowSlider.value  || carColor.r != uI.redGlowSlider.value || carColor.b != uI.blueGlowSlider.value) {
			carColor.a = 0.1f;
			carColor.r = uI.redGlowSlider.value;
			carColor.b = uI.blueGlowSlider.value;
			carColor.g = uI.greenGlowSlider.value;
			var main = sceneCarGlowLight[vehicleNumber].main;
			main.startColor = carColor;
			///Reset the particle effect
			sceneCarGlowLight[vehicleNumber].gameObject.SetActive(false);
			sceneCarGlowLight[vehicleNumber].gameObject.SetActive(true);
			//reflectProbe.backgroundColor = carColor;
		}
	}
	#endregion

	// These methods can be called to play an audio sound
	#region PlayAudio Methods
	void AudioMusic(){
		if (RGT_PlayerPrefs.audioData.music.Length > 0) {
			emptyObject = new GameObject ("Audio Clip: Music");
			emptyObject.transform.parent = audioContainer.transform;
			emptyObject.AddComponent<AudioSource> ();
			garageAudioSource = emptyObject.GetComponent<AudioSource> ();
			RGT_PlayerPrefs.audioData.currentAudioTrack = 0;
			garageAudioSource.clip = RGT_PlayerPrefs.audioData.music [RGT_PlayerPrefs.audioData.currentAudioTrack];
			garageAudioSource.loop = false;
			garageAudioSource.Play ();
		}
	}

	void PlayNextAudioTrack(){
		if (RGT_PlayerPrefs.audioData.musicSelection == AudioData.MusicSelection.ListOrder) {
			if (RGT_PlayerPrefs.audioData.currentAudioTrack < RGT_PlayerPrefs.audioData.music.Length - 1) {
				RGT_PlayerPrefs.audioData.currentAudioTrack += 1;
			} else {
				RGT_PlayerPrefs.audioData.currentAudioTrack = 0;
			}
		}else if(RGT_PlayerPrefs.audioData.musicSelection == AudioData.MusicSelection.Random){
			RGT_PlayerPrefs.audioData.currentAudioTrack = UnityEngine.Random.Range ( 0, RGT_PlayerPrefs.audioData.music.Length );
		}
		garageAudioSource.clip = RGT_PlayerPrefs.audioData.music [RGT_PlayerPrefs.audioData.currentAudioTrack];
		garageAudioSource.Play ();
	}

	public void AudioMenuSelect(){
		emptyObject = new GameObject ("Audio Clip: Menu Select");
		emptyObject.transform.parent = audioContainer.transform;
		emptyObject.AddComponent<AudioSource>();
		emptyObject.GetComponent<AudioSource>().clip = RGT_PlayerPrefs.audioData.menuSelect;
		emptyObject.GetComponent<AudioSource>().loop = false;
		emptyObject.GetComponent<AudioSource>().Play ();
		emptyObject.AddComponent<DestroyAudio>();
		emptyObject = null;
	}

	public void AudioMenuBack(){
		emptyObject = new GameObject ("Audio Clip: Menu Back");
		emptyObject.transform.parent = audioContainer.transform;
		emptyObject.AddComponent<AudioSource>();
		emptyObject.GetComponent<AudioSource>().clip = RGT_PlayerPrefs.audioData.menuBack;
		emptyObject.GetComponent<AudioSource>().loop = false;
		emptyObject.GetComponent<AudioSource>().Play ();
		emptyObject.AddComponent<DestroyAudio>();
		emptyObject = null;
	}
	#endregion

	public void Button_TopSpeed() {
		AudioMenuSelect();
		uI.upgradeConfirmText.text = "Upgrade " + "Top Speed" + "\nfor\n$" + RGT_PlayerPrefs.playableVehicles.upgradeSpeedPrice.ToString("N0");
		if (currency >= RGT_PlayerPrefs.playableVehicles.upgradeSpeedPrice) uI.upgradesConfirmWindow.SetActive(true);
	}

	public void Button_Acceleration() {
		AudioMenuSelect();
		uI.upgradeConfirmText.text = "Upgrade " + "Acceleration" + "\nfor\n$" + RGT_PlayerPrefs.playableVehicles.upgradeAccelerationPrice.ToString("N0");
		if (currency >= RGT_PlayerPrefs.playableVehicles.upgradeAccelerationPrice) uI.upgradesConfirmWindow.SetActive(true);
	}

	public void Button_BrakePower() {
		AudioMenuSelect();
		uI.upgradeConfirmText.text = "Upgrade " + "Brake Power" + "\nfor\n$" + RGT_PlayerPrefs.playableVehicles.upgradeBrakesPrice.ToString("N0");
		if (currency >= RGT_PlayerPrefs.playableVehicles.upgradeBrakesPrice) uI.upgradesConfirmWindow.SetActive(true);
	}

	public void Button_TireTraction() {
		AudioMenuSelect();
		uI.upgradeConfirmText.text = "Upgrade " + "Tire Traction" + "\nfor\n$" + RGT_PlayerPrefs.playableVehicles.upgradeTiresPrice.ToString("N0");
		if (currency >= RGT_PlayerPrefs.playableVehicles.upgradeTiresPrice) uI.upgradesConfirmWindow.SetActive(true);
	}

	public void Button_SteerSensitivity() {
		AudioMenuSelect();
		uI.upgradeConfirmText.text = "Upgrade " + "Steer Sensitivity" + "\nfor\n$" + RGT_PlayerPrefs.playableVehicles.upgradeSteeringPrice.ToString("N0");
		if (currency >= RGT_PlayerPrefs.playableVehicles.upgradeSteeringPrice) uI.upgradesConfirmWindow.SetActive(true);
	}

	// Call this to cancel a purchase confirmation
	public void DeclinePurchase(){
		uI.carConfirmWindow.SetActive(false);
		uI.paintConfirmWindow.SetActive (false);
		uI.glowConfirmWindow.SetActive (false);
		uI.glassColorConfirmWindow.SetActive(false);
		uI.brakeColorConfirmWindow.SetActive(false);
		uI.rimColorConfirmWindow.SetActive(false);
		uI.unlockRaceConfirmWindow.SetActive (false);
		uI.upgradesConfirmWindow.SetActive(false);
		AudioMenuBack();
	}

	public void AcceptPurchase(){
		uI.carConfirmWindow.SetActive(false);
		RGT_PlayerPrefs.SetVehicleLock (vehicleNumber, "UNLOCKED");
		RGT_PlayerPrefs.playableVehicles.carUnlocked[vehicleNumber] = true;
		//set currency
		RGT_PlayerPrefs.SetCurrency(currency - RGT_PlayerPrefs.playableVehicles.price[vehicleNumber]);
		//set Vehicle Number
		RGT_PlayerPrefs.SetVehicleNumber (vehicleNumber);
		UpdateCurrency ();
		uI.buyCarButton.SetActive(false);
		uI.selectCarButton.SetActive (true);
	}

	public void NextCar(){
		uI.buyCarConfirmWindow.SetActive(false);
		AudioMenuSelect ();
		sceneCarModel[vehicleNumber].SetActive(false);
		if (vehicleNumber < RGT_PlayerPrefs.playableVehicles.numberOfCars - 1) {
			vehicleNumber += 1;
			sceneCarModel[vehicleNumber].SetActive (true);
		} else {
			vehicleNumber = 0;
			sceneCarModel[vehicleNumber].SetActive (true);
		}
		uI.carNameText.text = RGT_PlayerPrefs.playableVehicles.vehicleNames[vehicleNumber];
		if (RGT_PlayerPrefs.playableVehicles.carUnlocked[vehicleNumber]) {
			uI.buyCarButton.SetActive(false);
			uI.selectCarButton.SetActive (false);
			uI.selectCarButton.SetActive (true);
			//set Vehicle Number
			RGT_PlayerPrefs.SetVehicleNumber (vehicleNumber);   
		} else {
			uI.selectCarButton.SetActive (false);
			uI.buyCarButton.SetActive(false);
			uI.buyCarButton.SetActive(true);
			uI.selectCarText.text = "$" + RGT_PlayerPrefs.playableVehicles.price[vehicleNumber].ToString("N0");
		}
		RGT_PlayerPrefs.playableVehicles.currentVehicleNumber = vehicleNumber;
		UpdateCar();
	}

	public void PreviousCar(){
		uI.buyCarConfirmWindow.SetActive(false);
		AudioMenuSelect ();
		sceneCarModel[vehicleNumber].SetActive(false);
		if (vehicleNumber > 0) {
			vehicleNumber -= 1;
			sceneCarModel[vehicleNumber].SetActive (true);
		} else {
			vehicleNumber = RGT_PlayerPrefs.playableVehicles.numberOfCars - 1;
			sceneCarModel[vehicleNumber].SetActive (true);
		}
		uI.carNameText.text = RGT_PlayerPrefs.playableVehicles.vehicleNames[vehicleNumber];
		if(RGT_PlayerPrefs.playableVehicles.carUnlocked[vehicleNumber]){
			uI.buyCarButton.SetActive(false);
			uI.selectCarButton.SetActive (false);
			uI.selectCarButton.SetActive (true);
			//set Vehicle Number
			RGT_PlayerPrefs.SetVehicleNumber (vehicleNumber);
		}
		else {
			uI.selectCarButton.SetActive (false);
			uI.buyCarButton.SetActive(false);
			uI.buyCarButton.SetActive(true);
			uI.selectCarText.text = "$" + RGT_PlayerPrefs.playableVehicles.price[vehicleNumber].ToString("N0");
		}
		RGT_PlayerPrefs.playableVehicles.currentVehicleNumber = vehicleNumber;
		UpdateCar();
	}

	public void UpdateCar() {
		UpdateCurrency ();


		levelBonusTopSpeed = 0;
		for(int i = 0; i < RGT_PlayerPrefs.GetVehicleTopSpeedLevel(vehicleNumber) + 1; i++){
			levelBonusTopSpeed += RGT_PlayerPrefs.playableVehicles.vehicleUpgrades [vehicleNumber].topSpeed [i];
		}

//		for(int i = 0; i < RGT_PlayerPrefs.GetVehicleAccelerationLevel(vehicleNumber) + 1; i++){
//			levelBonusAcceleration += RGT_PlayerPrefs.playableVehicles.vehicleUpgrades [vehicleNumber].acceleration [i];
//		}
//
//		for(int i = 0; i < RGT_PlayerPrefs.GetVehicleBrakePowerLevel(vehicleNumber) + 1; i++){
//			levelBonusBrakePower += RGT_PlayerPrefs.playableVehicles.vehicleUpgrades [vehicleNumber].brakePower [i];
//		}
//
//		for(int i = 0; i < RGT_PlayerPrefs.GetVehicleTireTractionLevel(vehicleNumber) + 1; i++){
//			levelBonusTireTraction += RGT_PlayerPrefs.playableVehicles.vehicleUpgrades [vehicleNumber].tireTraction [i];
//		}
//
//		for(int i = 0; i < RGT_PlayerPrefs.GetVehicleSteerSensitivityLevel(vehicleNumber) + 1; i++){
//			levelBonusSteerSensitivity += RGT_PlayerPrefs.playableVehicles.vehicleUpgrades [vehicleNumber].steerSensitivity [i];
//		}

		#if EVP_SUPPORT
		switch(RGT_PlayerPrefs.playableVehicles.speedometerType[vehicleNumber])
		{
		case RG_DistanceMetrics.SpeedType.KilometerPerHour:
			uI.carSpeedText.text = ((RGT_PlayerPrefs.playableVehicles.vehicles[vehicleNumber].GetComponent<EVP.VehicleController>().maxSpeedForward + levelBonusTopSpeed) * 3.6f).ToString("F0") + " KPH";
			break;
		case RG_DistanceMetrics.SpeedType.MilesPerHour:
			uI.carSpeedText.text = ((RGT_PlayerPrefs.playableVehicles.vehicles[vehicleNumber].GetComponent<EVP.VehicleController>().maxSpeedForward + levelBonusTopSpeed) * 2.23694f).ToString("F0") + " MPH";
			break;
		case RG_DistanceMetrics.SpeedType.MeterPerSecond:
			uI.carSpeedText.text = (RGT_PlayerPrefs.playableVehicles.vehicles[vehicleNumber].GetComponent<EVP.VehicleController>().maxSpeedForward + levelBonusTopSpeed).ToString() + " MPS";
			break;
		}
		#else
		switch(RGT_PlayerPrefs.playableVehicles.speedometerType[vehicleNumber])
		{
		case RG_DistanceMetrics.SpeedType.KilometerPerHour:
			uI.carSpeedText.text = (RGT_PlayerPrefs.playableVehicles.vehicles[vehicleNumber].GetComponent<RG_CarController>().topSpeed + levelBonusTopSpeed	).ToString() + " KPH";
			break;
		case RG_DistanceMetrics.SpeedType.MilesPerHour:
			uI.carSpeedText.text = (RGT_PlayerPrefs.playableVehicles.vehicles[vehicleNumber].GetComponent<RG_CarController>().topSpeed + levelBonusTopSpeed	).ToString() + " MPH";
			break;
		case RG_DistanceMetrics.SpeedType.MeterPerSecond:
			uI.carSpeedText.text = (RGT_PlayerPrefs.playableVehicles.vehicles[vehicleNumber].GetComponent<RG_CarController>().topSpeed + levelBonusTopSpeed	).ToString() + " MPS";
			break;
		}
		#endif



		uI.topSpeedSlider.value = RGT_PlayerPrefs.playableVehicles.topSpeedLevel[vehicleNumber];
		uI.accelerationSlider.value = RGT_PlayerPrefs.playableVehicles.torqueLevel[vehicleNumber];
		uI.brakePowerSlider.value = RGT_PlayerPrefs.playableVehicles.brakeTorqueLevel[vehicleNumber];
		uI.tireTractionSlider.value = RGT_PlayerPrefs.playableVehicles.tireTractionLevel[vehicleNumber];
		uI.steerSensitivitySlidetr.value = RGT_PlayerPrefs.playableVehicles.steerSensitivityLevel[vehicleNumber];
		if (RGT_PlayerPrefs.playableVehicles.topSpeedLevel[vehicleNumber] >= 9) { 
			uI.upgradeTopSpeedButton.SetActive(false);
		}
		else {
			uI.upgradeTopSpeedButton.SetActive(true);
		}
		if (RGT_PlayerPrefs.playableVehicles.torqueLevel[vehicleNumber] >= 9) { 
			uI.upgradeAccelerationButton.SetActive(false);
		}else {
			uI.upgradeAccelerationButton.SetActive(true);
		}
		if (RGT_PlayerPrefs.playableVehicles.brakeTorqueLevel[vehicleNumber] >= 9){
			uI.upgradeBrakePowerButton.SetActive(false);
		}else {
			uI.upgradeBrakePowerButton.SetActive(true);
		}
		if (RGT_PlayerPrefs.playableVehicles.tireTractionLevel[vehicleNumber] >= 9) { 
			uI.upgradeTireTractionButton.SetActive(false);
		}else {
			uI.upgradeTireTractionButton.SetActive(true);
		}
		if (RGT_PlayerPrefs.playableVehicles.steerSensitivityLevel[vehicleNumber] >= 9) { 
			uI.upgradeSteerSensitivityButton.SetActive(false);
		}else {
			uI.upgradeSteerSensitivityButton.SetActive(true);
		}
	}

	#region RaceSelection methods
	public void Button_RaceSelection(){
		if (uI.racesWindow.activeInHierarchy) {
			uI.racesWindow.SetActive (false);
			uI.singlePlayerModeWindow.SetActive (true);
			AudioMenuBack ();
		} else {
			uI.singlePlayerModeWindow.SetActive (false);
			uI.racesWindow.SetActive (true);		
			AudioMenuSelect ();
		}
	}

	public void SelectRace(){
		AudioMenuSelect ();
		if (!RGT_PlayerPrefs.raceData.raceLocked[raceNumber]) {
			RGT_PlayerPrefs.raceData.vehicleNumber = RGT_PlayerPrefs.playableVehicles.currentVehicleNumber;
			//set game mode
			RGT_PlayerPrefs.SetGameMode ("SINGLE PLAYER");
			uI.loadingWindow.SetActive(true);
			//set race number
			RGT_PlayerPrefs.SetRaceNumber(raceNumber);
			//set race rewards
			RGT_PlayerPrefs.SetRaceReward(1, firstPrize);
			RGT_PlayerPrefs.SetRaceReward(2, secondPrize);
			RGT_PlayerPrefs.SetRaceReward(3, thirdPrize);
			raceNameToLoad = raceNumber.ToString () + RGT_PlayerPrefs.raceData.raceNames[raceNumber];
			SceneManager.LoadScene(raceNameToLoad); 
		} else {

		}
	}

	public void NextRace(){
		AudioMenuSelect ();
		if (raceNumber < RGT_PlayerPrefs.raceData.numberOfRaces - 1) {
			raceNumber += 1;
		} else {
			raceNumber = 0;
		}
		UpdateRaceDetails ();
	}

	public void PreviousRace(){
		AudioMenuSelect ();
		if (raceNumber > 0) {
			raceNumber -= 1;
		} else {
			raceNumber = RGT_PlayerPrefs.raceData.numberOfRaces - 1;
		}
		UpdateRaceDetails ();
	}

	public void UpdateRaceDetails(){
		raceImage.sprite = RGT_PlayerPrefs.raceData.raceImages[raceNumber];
		uI.raceNameText.text = RGT_PlayerPrefs.raceData.raceNames[raceNumber];
		uI.lapText.text = "Laps " + RGT_PlayerPrefs.raceData.raceLaps [raceNumber].ToString ();
		uI.numberOfRacersText.text = "Racers " + RGT_PlayerPrefs.raceData.numberOfRacers [raceNumber].ToString();
		if (!RGT_PlayerPrefs.raceData.raceLocked[raceNumber]) {
			uI.raceDetailsWindow.SetActive (true);
			uI.raceDetails.SetActive (true);//check
			uI.selectRaceText.text = "Select Race";
			uI.raceLockedIcon.SetActive(false);
		}else {
			uI.raceDetails.SetActive (false);//check
			uI.unlockLevelButtonText.text = "Unlock\n" + RGT_PlayerPrefs.raceData.unlockAmount[raceNumber].ToString("C0");
			uI.selectRaceText.text = RGT_PlayerPrefs.raceData.lockButtonText;
			uI.raceLockedIcon.SetActive(true);
			uI.unlockLevelText.text = "Unlock " + RGT_PlayerPrefs.raceData.raceNames[raceNumber] + "\nfor\n" + RGT_PlayerPrefs.raceData.unlockAmount[raceNumber].ToString("C0");
		}
		CalculateRewardText ();
	}

	public void UnlockRaceButton(){
		if (currency >= RGT_PlayerPrefs.raceData.unlockAmount[raceNumber]) {
			uI.unlockRaceConfirmWindow.SetActive (true);
		}
	}

	public void AcceptPurchaseUnlockRace(){
		AudioMenuSelect ();
		//set currency
		RGT_PlayerPrefs.SetCurrency(currency - RGT_PlayerPrefs.raceData.unlockAmount[raceNumber]);
		//set race lock
		RGT_PlayerPrefs.SetRaceLock(RGT_PlayerPrefs.raceData.raceNames[raceNumber], "UNLOCKED");
		RGT_PlayerPrefs.raceData.raceLocked[raceNumber] = false;
		uI.raceLockedIcon.SetActive(false);
		uI.raceDetailsWindow.SetActive(true);
		uI.selectRaceText.text = "Select Race";
		uI.unlockRaceConfirmWindow.SetActive (false);
		UpdateCurrency ();
	}

	public void LapIncrease(){
		if (RGT_PlayerPrefs.raceData.raceLaps [raceNumber] < RGT_PlayerPrefs.raceData.lapLimit [raceNumber]) {
			RGT_PlayerPrefs.raceData.raceLaps [raceNumber] += 1;
		} else {
			RGT_PlayerPrefs.raceData.raceLaps [raceNumber] = 1;
		}
		uI.lapText.text = "Laps\n" + RGT_PlayerPrefs.raceData.raceLaps [raceNumber].ToString ();
		CalculateRewardText ();
		//set race laps
		RGT_PlayerPrefs.SetRaceLaps(raceNumber, RGT_PlayerPrefs.raceData.raceLaps[raceNumber]);
	}

	public void LapDecrease(){
		if (RGT_PlayerPrefs.raceData.raceLaps [raceNumber] > 1) {
			RGT_PlayerPrefs.raceData.raceLaps [raceNumber] -= 1;
		} else {
			RGT_PlayerPrefs.raceData.raceLaps [raceNumber] = RGT_PlayerPrefs.raceData.lapLimit [raceNumber];
		}
		uI.lapText.text = "Laps\n" + RGT_PlayerPrefs.raceData.raceLaps [raceNumber].ToString ();
		CalculateRewardText ();
		RGT_PlayerPrefs.SetRaceLaps(raceNumber, RGT_PlayerPrefs.raceData.raceLaps[raceNumber]);
	}

	public void NumberOfRacersIncrease(){
		if (RGT_PlayerPrefs.raceData.numberOfRacers[raceNumber] < RGT_PlayerPrefs.raceData.racerLimit[raceNumber]) {
			RGT_PlayerPrefs.raceData.numberOfRacers [raceNumber] += 1;
		} else {
			RGT_PlayerPrefs.raceData.numberOfRacers [raceNumber] = 1;
		}
		uI.numberOfRacersText.text = "Racers\n" + RGT_PlayerPrefs.raceData.numberOfRacers [raceNumber].ToString();
		CalculateRewardText ();
	}

	public void NumberOfRacersDecrease(){
		if (RGT_PlayerPrefs.raceData.numberOfRacers [raceNumber] > 1) {
			RGT_PlayerPrefs.raceData.numberOfRacers [raceNumber] -= 1;
		} else {
			RGT_PlayerPrefs.raceData.numberOfRacers [raceNumber] = RGT_PlayerPrefs.raceData.racerLimit [raceNumber];
		}
		uI.numberOfRacersText.text = "Racers\n" + RGT_PlayerPrefs.raceData.numberOfRacers [raceNumber].ToString();
		CalculateRewardText ();
	}

	public void LoadOpenWorldButton(){
		uI.loadingWindow.SetActive(true);
		//set game mode
		RGT_PlayerPrefs.SetGameMode("SINGLE PLAYER");
		SceneManager.LoadScene("M0Circuit Hills");
	}
	#endregion


	public void Button_MultiplayerGame(){
		if (uI.multiplayerWindow.activeInHierarchy) {
			cameraOffset = cameraOffsetDefault;
			uI.multiplayerWindow.SetActive (false);
			uI.multiplayerModeWindow.SetActive (true);
			AudioMenuSelect ();
		} else {
			cameraOffset = 0;
			uI.multiplayerModeWindow.SetActive (false);
			//set game mode
			RGT_PlayerPrefs.SetGameMode("MULTIPLAYER");
			uI.multiplayerWindow.SetActive (true);
			AudioMenuBack ();
		}
	}

	public void Button_BackLobby(){
		uI.multiplayerWindow.SetActive (false);
		uI.multiplayerModeWindow.SetActive (true);
	}

	public void ReloadGarageScene(){
		
	}

	void Reload(){
		//set host drop
		RGT_PlayerPrefs.SetHostDrop ("SCENERELOADED");
		Destroy (lobbyManager);
		//SceneManager.LoadScene ("Garage");
	}

	void CalculateRewardText(){
		if (RGT_PlayerPrefs.GetRaceStatus(RGT_PlayerPrefs.raceData.raceNames[raceNumber]) == "COMPLETE" && !RGT_PlayerPrefs.raceData.unlimitedRewards [raceNumber]) {
			firstPrize = 0;
			secondPrize = 0;
			thirdPrize = 0;
		} else {
			firstPrize = (RGT_PlayerPrefs.raceData.raceLaps [raceNumber] - 1) * RGT_PlayerPrefs.raceData.extraLapRewardMultiplier [raceNumber] * RGT_PlayerPrefs.raceData.firstPrize [raceNumber];
			firstPrize += (RGT_PlayerPrefs.raceData.numberOfRacers [raceNumber] - 1) * RGT_PlayerPrefs.raceData.extraRacerRewardMultiplier [raceNumber] * RGT_PlayerPrefs.raceData.firstPrize [raceNumber];
			firstPrize += RGT_PlayerPrefs.raceData.firstPrize [raceNumber];
			if (RGT_PlayerPrefs.raceData.numberOfRacers [raceNumber] > 1) {
				secondPrize = (RGT_PlayerPrefs.raceData.raceLaps [raceNumber] - 1) * RGT_PlayerPrefs.raceData.extraLapRewardMultiplier [raceNumber] * RGT_PlayerPrefs.raceData.secondPrize [raceNumber];
				secondPrize += (RGT_PlayerPrefs.raceData.numberOfRacers [raceNumber] - 1) * RGT_PlayerPrefs.raceData.extraRacerRewardMultiplier [raceNumber] * RGT_PlayerPrefs.raceData.secondPrize [raceNumber];
				secondPrize += RGT_PlayerPrefs.raceData.secondPrize [raceNumber];
			} else {
				secondPrize = 0;
			}
			if (RGT_PlayerPrefs.raceData.numberOfRacers [raceNumber] > 2) {
				thirdPrize = (RGT_PlayerPrefs.raceData.raceLaps [raceNumber] - 1) * RGT_PlayerPrefs.raceData.extraLapRewardMultiplier [raceNumber] * RGT_PlayerPrefs.raceData.thirdPrize [raceNumber];
				thirdPrize += (RGT_PlayerPrefs.raceData.numberOfRacers [raceNumber] - 1) * RGT_PlayerPrefs.raceData.extraRacerRewardMultiplier [raceNumber] * RGT_PlayerPrefs.raceData.thirdPrize [raceNumber];
				thirdPrize += RGT_PlayerPrefs.raceData.thirdPrize [raceNumber];
			} else {
				thirdPrize = 0;
			}
		}
		uI.rewardText.text = "\n" + firstPrize.ToString("C0") + "\n" + secondPrize.ToString("C0") + "\n" + thirdPrize.ToString("C0");
	}

	public void UpdateCurrency(){
		currency = RGT_PlayerPrefs.GetCurrency ();
		uI.currencyText.text = currency.ToString ("N0");
	}

}