using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using TurnTheGameOn.RacingGameTemplate;
using System.Collections;

public enum CarDriveType{
	FrontWheelDrive,
	RearWheelDrive,
	FourWheelDrive
}

public class RG_CarController : MonoBehaviour{

	public int vehicleNumber;
	public bool playerCar;
	[Header("Acceleration, Speed and Braking")]
	public float topSpeed = 125.0f;
	public float topSpeedReverse = 45.0f;
	public float fullTorqueOverAllWheels = 5000.0f;
	public float reverseTorque = 1200.0f;
	public float brakeTorque = 3000.0f;
	public float maxHandbrakeTorque = 0.0f;
	[Header("Transmission")]
	public bool manual = false;
	public int NoOfGears = 5;
	[Range(0,1)] public float minRPM = 0.2f;
	[Range(0,1)] public float maxRPM = 0.8f;
	[Range(0,5)] public float RPMFallRate = 1.0f;
	public AnimationCurve torqueCurve = AnimationCurve.EaseInOut (0, 0.85f, 1, 0.65f);
	public AnimationCurve gearSpeedLimitCurve = AnimationCurve.Linear (0, 0, 1, 1);
	public float revRangeBoundary = 1f;
	public EventTrigger.Entry shiftUp;
	public EventTrigger.Entry shiftDown;
	[Header("Nitro")]
	public float nitroTopSpeed;
	public float nitroFullTorque;
	public float nitroDuration;
	public float nitroSpendRate;
	public float nitroRefillRate;
	public GameObject nitroFX;
	public EventTrigger.Entry nitroON;
	public EventTrigger.Entry nitroOFF;
	public bool nitroOn;
	[Header("Physics and Handling")]
	public Vector3 centerOfMass;
	public CarDriveType m_CarDriveType = CarDriveType.FourWheelDrive;
	public float downforce = 100f;
	public float slipLimit = 0.3f;
	public float steerSensitivity = 0.15f;
	[Range(0, 90)] public float maximumSteerAngle = 35.0f;
	[Range(0, 90)] public float steerAngleAtMaxSpeed = 35.0f;
	[Tooltip("0 is raw physics , 1 the car will grip in the direction it is facing")] [Range(0, 1)] public float steerHelper = 0.57f;
	[Tooltip("0 is no traction control, 1 is full interference")] [Range(0, 1)] public float tractionControl = 0.77f;
	[Header("HUD UI Text")]
	public string speedTextName = "Speed Text";
	public string gearTextName = "Gear Text";
	public string RPMSliderName = "RPM Slider";
	public string distanceMetricTextName = "MPH Text";
	[Header("Wheels")]
	public WheelCollider[] m_WheelColliders = new WheelCollider[4];
	public GameObject[] m_WheelMeshes = new GameObject[4];
	[SerializeField] private RG_WheelEffects[] m_WheelEffects = new RG_WheelEffects[4];
	[Header("Controller Override")]
	public bool overrideBrake;
	public float overrideBrakePower;//overrides the brake input value, used to force ai to brake
	public bool overrideAcceleration;
	public float overrideAccelerationPower;//overrides the brake input value, used to force ai to brake
	public bool overrideSteering;
	[Range(0,1)] public float overrideSteeringPower;//overrides the steer input value, used to force ai to brake
	[Header("Read Only Variables")]
	public float speed;
	public bool reversing;
	public float CurrentSteerAngle{ get { return steerAngle; }}
	public float CurrentSpeed{ get { return rbody.velocity.magnitude*2.23693629f; }}
	public float MaxSpeed{get { return topSpeed; }}
	public float Revs { get; private set; }
	public float AccelInput { get; private set; }
	public float BrakeInput { get; private set; }

	public Rigidbody rbody;
	private Text distanceMetricText;
	private Text speedText;
	private Text gearText;
	private Slider RPMSlider;
	private Slider nitroSlider;
	private float levelBonusTopSpeed;
	private float levelBonusAcceleration;
	private float levelBonusBrakePower;
	private float levelBonusTireTraction;
	private float levelBonusSteerSensitivity;
	private float currentGearSpeedLimit;
	private float currentTorque;
	private float nitroAmount;
	private int currentGear;
	private float gearSpeedRange;
	private float gearFactor;
	private float upGearLimit;
	private float downGearLimit;
	private float steerAngle;
	private float previousRotation;
	private bool isAudioMuted;
	private bool canShift = true;
	private float thrustTorque;
	private Quaternion[] m_WheelMeshLocalRotations;
	private const float k_ReversingThreshold = 0.01f;

	float f;
	float targetGearFactor;

	private bool AnySkidSoundPlaying(){
		for (int i = 0; i < 4; i++)	{
			if (m_WheelEffects[i].PlayingAudio)	{
				return true;
			}
		}
		return false;
	}

	private void Start(){
		if (playerCar){
			if (RGT_PlayerPrefs.GetAudio() == "OFF") {
				isAudioMuted = true;
			} else {
				isAudioMuted = false;
			}
			manual = RGT_PlayerPrefs.playableVehicles.manual;
			GameObject lobbyImage = GameObject.Find ("Lobby Loading Image");
			if(lobbyImage != null)	lobbyImage.SetActive(false);
			SetupVehicleStats ();
			if (GameObject.Find("Player Input")){
				//Setup Nitro UI Button
				EventTrigger mobileButton = GameObject.Find("Nitro Button").GetComponent<EventTrigger>();
				EventTrigger.Entry entry = nitroON;
				mobileButton.triggers.Add(entry);
				entry = nitroOFF;
				mobileButton.triggers.Add(entry);
				if (manual) {
					//Setup Shift Up UI Button
					mobileButton = GameObject.Find ("Shift Up Button").GetComponent<EventTrigger> ();
					entry = shiftUp;
					mobileButton.triggers.Add (entry);
					//Setup Shift Down UI Button
					mobileButton = GameObject.Find ("Shift Down Button").GetComponent<EventTrigger> ();
					entry = shiftDown;
					mobileButton.triggers.Add (entry);
				}
			}
			nitroSlider = GameObject.Find("Nitro Slider").GetComponent<Slider>();
			nitroAmount = nitroDuration;
		}

		distanceMetricText = GameObject.Find(distanceMetricTextName).GetComponent<Text>();
		switch (RGT_PlayerPrefs.playableVehicles.speedometerType[RGT_PlayerPrefs.raceData.vehicleNumber]) {
		case RG_DistanceMetrics.SpeedType.KilometerPerHour:
			distanceMetricText.text = "KPH";
			break;
		case RG_DistanceMetrics.SpeedType.MilesPerHour:
			distanceMetricText.text = "MPH";
			break;
		}

		speedText = GameObject.Find(speedTextName).GetComponent<Text>();
		gearText = GameObject.Find(gearTextName).GetComponent<Text>();
		RPMSlider = GameObject.Find(RPMSliderName).GetComponent<Slider>();
		m_WheelMeshLocalRotations = new Quaternion[4];
		for (int i = 0; i < 4; i++){
			m_WheelMeshLocalRotations[i] = m_WheelMeshes[i].transform.localRotation;
		}

		maxHandbrakeTorque = float.MaxValue;

		if(rbody == null) rbody = GetComponent<Rigidbody>();
		rbody.centerOfMass = centerOfMass;
		currentTorque = fullTorqueOverAllWheels - (tractionControl*fullTorqueOverAllWheels);
	}

	void Update() {
		if (playerCar) {
			nitroSlider.value = nitroAmount;
			if (!nitroOn && nitroAmount < nitroDuration) {
				nitroAmount += nitroRefillRate * Time.deltaTime;
				if (nitroAmount > nitroDuration)
					nitroAmount = nitroDuration;
			}
			else {
				nitroAmount -= nitroSpendRate * Time.deltaTime;
				if (nitroAmount < 0) { 
					nitroAmount = 0;
					NitroOff();
				}
			}
		}
	}

	void UpdateUI(){
		//Speed
		speed = rbody.velocity.magnitude;
		float speedometerMultiplier = 0f;
		switch (RGT_PlayerPrefs.playableVehicles.speedometerType[RGT_PlayerPrefs.raceData.vehicleNumber]){
		case RG_DistanceMetrics.SpeedType.MilesPerHour:
			speedometerMultiplier = 2.23693629f;
			break;
		case RG_DistanceMetrics.SpeedType.KilometerPerHour:
			speedometerMultiplier = 3.6f;
			break;
		}
		speed *= speedometerMultiplier;
		if(speedText){		speedText.text = speed.ToString("F0");	}

		if (!playerCar)	return;
		//Gears
		if (gearText && !manual) {
			if (BrakeInput > 0f && reversing) {
				gearText.text = "R";
			} else {
				if (currentGear == 0)
					gearText.text = "N";
			}
			if (AccelInput > 0f) {
				gearText.text = (currentGear + 1f).ToString ();
			}
		} else if(gearText){
			if (currentGear == 0) {
				gearText.text = "N";
			} else if (currentGear == -1) {
				gearText.text = "R";
			} else {
				gearText.text = (currentGear).ToString ();
			}
		}
	}

	void CheckGear(){
		gearSpeedRange = Mathf.Abs(CurrentSpeed/MaxSpeed);
		upGearLimit = (1/(float) NoOfGears)*(currentGear + 1);
		downGearLimit = (1/(float) NoOfGears)*currentGear;

		if (!manual) { 
			if (currentGear > 0 && gearSpeedRange < downGearLimit) {
				currentGear--;
			}
			if (gearSpeedRange > upGearLimit && (currentGear < (NoOfGears - 1))) {
				currentGear++;
			}
		} else {
			if (canShift) {
				if (Input.GetKeyDown (RGT_PlayerPrefs.inputData.shiftUp) || Input.GetKeyDown (RGT_PlayerPrefs.inputData.shiftUpJoystick)) {
					canShift = false;
					ShiftUp ();			
				}
				if (Input.GetKeyDown (RGT_PlayerPrefs.inputData.shiftDown) || Input.GetKeyDown (RGT_PlayerPrefs.inputData.shiftDownJoystick)) {
					canShift = false;
					ShiftDown ();
				}
			}
			else {
				if (Input.GetKeyUp (RGT_PlayerPrefs.inputData.shiftUp) || Input.GetKeyUp (RGT_PlayerPrefs.inputData.shiftUpJoystick)) {
					canShift = true;		
				}
				if (Input.GetKeyUp (RGT_PlayerPrefs.inputData.shiftDown) || Input.GetKeyUp (RGT_PlayerPrefs.inputData.shiftDownJoystick)) {
					canShift = true;
				}
			}
		}
	}

	void CalculateGearFactor(){
		
		if (playerCar) {
			if (manual) {
				if (currentGear == 0) {
					targetGearFactor = minRPM;
				} else if (currentGear == -1) {
//				if (RGT_PlayerPrefs.inputData.useMobileController) {
//					
//				} else {
//					
//				}
					targetGearFactor = (minRPM + (speed / currentGearSpeedLimit) * BrakeInput) - (1 - maxRPM);
				} else {
					targetGearFactor = (minRPM + (speed / currentGearSpeedLimit) * AccelInput) - (1 - maxRPM);
				}
				gearFactor = Mathf.Lerp (gearFactor, targetGearFactor, Time.deltaTime * RPMFallRate);
				if (System.Single.IsNaN (gearFactor)) {
					gearFactor = minRPM;
				}
				if (RPMSlider)
					RPMSlider.value = gearFactor;
			} else {
				f = (1 / (float)NoOfGears);
				targetGearFactor = Mathf.InverseLerp (f * currentGear, f * (currentGear + 1), Mathf.Abs (CurrentSpeed / MaxSpeed));
				gearFactor = Mathf.Lerp (gearFactor, targetGearFactor, Time.deltaTime * 5f);
				if (RPMSlider) {
					switch (RGT_PlayerPrefs.playableVehicles.speedometerType [RGT_PlayerPrefs.raceData.vehicleNumber]) {
					case RG_DistanceMetrics.SpeedType.KilometerPerHour:
						if (currentGear != 3)	RPMSlider.value = gearFactor;
						if (currentGear == 3)	RPMSlider.value = 0.9f - gearFactor;
						break;
					case RG_DistanceMetrics.SpeedType.MilesPerHour:
						RPMSlider.value = gearFactor;
						break;
					}
				}
			}
		} else {			
			// gear factor is a normalised representation of the current speed within the current gear's range of speeds.
			// We smooth towards the 'target' gear factor, so that revs don't instantly snap up or down when changing gear.
			f = (1 / (float)NoOfGears);
			targetGearFactor = Mathf.InverseLerp (f * currentGear, f * (currentGear + 1), Mathf.Abs (CurrentSpeed / MaxSpeed));
			gearFactor = Mathf.Lerp (gearFactor, targetGearFactor, Time.deltaTime * 5f);
			if (RPMSlider) {
				switch (RGT_PlayerPrefs.playableVehicles.speedometerType [RGT_PlayerPrefs.raceData.vehicleNumber]) {
				case RG_DistanceMetrics.SpeedType.KilometerPerHour:
					if (currentGear != 3)	RPMSlider.value = gearFactor;
					if (currentGear == 3)	RPMSlider.value = 0.9f - gearFactor;
					break;
				case RG_DistanceMetrics.SpeedType.MilesPerHour:
					RPMSlider.value = gearFactor;
					break;
				}
			}
		}

		// calculate engine revs (for display / sound)
		// (this is done in retrospect - revs are not used in force/power calculations)
		var gearNumFactor = currentGear/(float) NoOfGears;
		var revsRangeMin = ULerp(0f, revRangeBoundary, CurveFactor(gearNumFactor));
		var revsRangeMax = ULerp(revRangeBoundary, 1f, gearNumFactor);
		Revs = ULerp(revsRangeMin, revsRangeMax, gearFactor);


	}

	// simple function to add a curved bias towards 1 for a value in the 0-1 range
	private static float CurveFactor(float factor){
		return 1 - (1 - factor)*(1 - factor);
	}

	// unclamped version of Lerp, to allow value to exceed the from-to range
	private static float ULerp(float from, float to, float value){
		return (1.0f - value)*from + value*to;
	}

	public void Move(float steering, float accel, float footbrake, float handbrake){

		if(playerCar){
			if (!reversing && currentGear == -1) {	rbody.AddForce (transform.forward * -5000);	accel = 0;	}
			//if (!reversing && speed < 2)	rbody.AddForce (transform.forward * 5000);
		}



		for (int i = 0; i < 4; i++){
			Quaternion quat;
			Vector3 position;
			m_WheelColliders[i].GetWorldPose(out position, out quat);
			m_WheelMeshes[i].transform.position = position;
			m_WheelMeshes[i].transform.rotation = quat;
		}

		//clamp input values
		if (overrideSteering) steering = overrideSteeringPower;
		steering = Mathf.Clamp(steering, -1, 1);
		AccelInput = accel = Mathf.Clamp(accel, 0, 1);
		BrakeInput = footbrake = -1 * Mathf.Clamp (footbrake, -1, 0);
		handbrake = Mathf.Clamp(handbrake, 0, 1);

		//Set the steer on the front wheels.
		//Assuming that wheels 0 and 1 are the front wheels.
		float speedFactor = steerSensitivity * CurrentSpeed * 1.609344f / MaxSpeed;

		steerAngle = Mathf.Lerp(maximumSteerAngle,steerAngleAtMaxSpeed, speedFactor);
		steerAngle *= steering;
		m_WheelColliders[0].steerAngle = steerAngle;
		m_WheelColliders[1].steerAngle = steerAngle;

		if(overrideBrake){
			footbrake = overrideBrakePower;
		}
		if(overrideAcceleration){
			accel = overrideAccelerationPower;
			ApplyDrive(accel, footbrake);
			return;
		}
		SteerHelper();
		ApplyDrive(accel, footbrake);
		//Set the handbrake.
		//Assuming that wheels 2 and 3 are the rear wheels.
		if (handbrake > 0f)	{
			var hbTorque = handbrake*maxHandbrakeTorque;
			m_WheelColliders[2].brakeTorque = hbTorque;
			m_WheelColliders[3].brakeTorque = hbTorque;
		}
		CalculateGearFactor ();
		CheckGear();
		UpdateUI ();
		AddDownForce();
		CheckForWheelSpin();
		TractionControl();
	}

	private void ApplyDrive(float accel, float footbrake){
		if (playerCar && manual) {
			switch (m_CarDriveType) {
			case CarDriveType.FourWheelDrive:
				if (speed < currentGearSpeedLimit) {
					thrustTorque = accel * (currentTorque / 4f);
					for (int i = 0; i < 4; i++) {
						m_WheelColliders [i].motorTorque = thrustTorque;
					}
				} else {
					for (int i = 0; i < 4; i++) {
						m_WheelColliders [i].motorTorque = 0;
					}
				}
				break;
			case CarDriveType.FrontWheelDrive:
				if (speed < currentGearSpeedLimit) {
					thrustTorque = accel * (currentTorque / 2f);
					m_WheelColliders [0].motorTorque = m_WheelColliders [1].motorTorque = thrustTorque;
				} else {
					m_WheelColliders [0].motorTorque = m_WheelColliders [1].motorTorque = 0;
				}
				break;
			case CarDriveType.RearWheelDrive:
				if (speed < currentGearSpeedLimit) {
					thrustTorque = accel * (currentTorque / 2f);
					m_WheelColliders [2].motorTorque = m_WheelColliders [3].motorTorque = thrustTorque;
				} else {
					m_WheelColliders [2].motorTorque = m_WheelColliders [3].motorTorque = 0;
				}
				break;
			}
		} else {
			switch (m_CarDriveType) {
			case CarDriveType.FourWheelDrive:
				thrustTorque = accel * (currentTorque / 4f);
				for (int i = 0; i < 4; i++) {
					m_WheelColliders [i].motorTorque = thrustTorque;
				}
				break;
			case CarDriveType.FrontWheelDrive:
				thrustTorque = accel * (currentTorque / 2f);
				m_WheelColliders [0].motorTorque = m_WheelColliders [1].motorTorque = thrustTorque;
				break;
			case CarDriveType.RearWheelDrive:
				thrustTorque = accel * (currentTorque / 2f);
				m_WheelColliders [2].motorTorque = m_WheelColliders [3].motorTorque = thrustTorque;
				break;
			}
		}

		if (overrideBrake) footbrake = overrideBrakePower;
		if (overrideAcceleration) accel = overrideAccelerationPower;

		for (int i = 0; i < 4; i++) {
			if (CurrentSpeed > 0 && Vector3.Angle (transform.forward, rbody.velocity) < 50f) {
				reversing = false;
				m_WheelColliders [i].brakeTorque = brakeTorque * footbrake;
			} else if (footbrake > 0) {
				if (playerCar && manual) {
					if (currentGear == -1) {
						reversing = true;
						if (speed < currentGearSpeedLimit) {
							m_WheelColliders [i].brakeTorque = 0f;
							m_WheelColliders [i].motorTorque = -reverseTorque * footbrake;
						}else{
							m_WheelColliders [i].motorTorque = 0;
						}
					}
				} else {
					reversing = true;
					m_WheelColliders [i].brakeTorque = 0f;
					m_WheelColliders [i].motorTorque = -reverseTorque * footbrake;
				}
			}
		}

	}

	private void SteerHelper(){
		for (int i = 0; i < 4; i++){
			WheelHit wheelhit;
			m_WheelColliders[i].GetGroundHit(out wheelhit);
			if (wheelhit.normal == Vector3.zero)
				return; // wheels arent on the ground so dont realign the rigidbody velocity
		}

		// this if is needed to avoid gimbal lock problems that will make the car suddenly shift direction
		if (Mathf.Abs(previousRotation - transform.eulerAngles.y) < 10f){
			var turnadjust = (transform.eulerAngles.y - previousRotation) * steerHelper;
			Quaternion velRotation = Quaternion.AngleAxis(turnadjust, Vector3.up);
			rbody.velocity = velRotation * rbody.velocity;
		}
		previousRotation = transform.eulerAngles.y;
	}

	// this is used to add more grip in relation to speed
	private void AddDownForce(){
		rbody.AddForce(-transform.up * downforce * rbody.velocity.magnitude);
	}

	// checks if the wheels are spinning and is so does three things
	// 1) emits particles
	// 2) plays tiure skidding sounds
	// 3) leaves skidmarks on the ground
	// these effects are controlled through the WheelEffects class
	private void CheckForWheelSpin(){
		// loop through all wheels
		for (int i = 0; i < 4; i++){
			WheelHit wheelHit;
			m_WheelColliders[i].GetGroundHit(out wheelHit);

			// is the tire slipping above the given threshhold
			if (Mathf.Abs(wheelHit.forwardSlip) >= slipLimit || Mathf.Abs(wheelHit.sidewaysSlip) >= slipLimit){
				m_WheelEffects[i].EmitTyreSmoke();

				// avoiding all four tires screeching at the same time
				// if they do it can lead to some strange audio artefacts
				if (!AnySkidSoundPlaying())	{
					if(m_WheelEffects[i].enabled)
						m_WheelEffects[i].PlayAudio();
				}
				continue;
			}

			// if it wasnt slipping stop all the audio
			if (m_WheelEffects[i].PlayingAudio)	{
				m_WheelEffects[i].StopAudio();
			}
			// end the trail generation
			m_WheelEffects[i].EndSkidTrail();
		}
	}

	// crude traction control that reduces the power to wheel if the car is wheel spinning too much
	private void TractionControl(){
		WheelHit wheelHit;
		switch (m_CarDriveType)
		{
		case CarDriveType.FourWheelDrive:
			// loop through all wheels
			for (int i = 0; i < 4; i++){
				m_WheelColliders[i].GetGroundHit(out wheelHit);

				AdjustTorque(wheelHit.forwardSlip);
			}
			break;

		case CarDriveType.RearWheelDrive:
			m_WheelColliders[2].GetGroundHit(out wheelHit);
			AdjustTorque(wheelHit.forwardSlip);

			m_WheelColliders[3].GetGroundHit(out wheelHit);
			AdjustTorque(wheelHit.forwardSlip);
			break;

		case CarDriveType.FrontWheelDrive:
			m_WheelColliders[0].GetGroundHit(out wheelHit);
			AdjustTorque(wheelHit.forwardSlip);

			m_WheelColliders[1].GetGroundHit(out wheelHit);
			AdjustTorque(wheelHit.forwardSlip);
			break;
		}
	}

	private void AdjustTorque(float forwardSlip){
		
		if (forwardSlip >= slipLimit && currentTorque >= 0){
			currentTorque -= 10 * tractionControl;
		}
		else{
			currentTorque += 10 * tractionControl;
			if (currentTorque > fullTorqueOverAllWheels){
				currentTorque = fullTorqueOverAllWheels;
			}
		}
		float curvePoint = (float)currentGear / (float)NoOfGears;
		if (playerCar) {
			currentTorque = fullTorqueOverAllWheels * torqueCurve.Evaluate (curvePoint);
			if (currentGear >= 0) {
				currentGearSpeedLimit = topSpeed * gearSpeedLimitCurve.Evaluate (curvePoint);
			} else {
				currentGearSpeedLimit = topSpeedReverse;
			}

		}

	}

	[ContextMenu("Shift Up")]
	public void ShiftUp(){
		if (!manual)	return;
		if ((currentGear < (NoOfGears - 1))) {
			currentGear++;
			gearFactor = minRPM;
		}
	}

	[ContextMenu("Shift Down")]
	public void ShiftDown(){
		if (!manual)	return;
		if (gearSpeedRange < downGearLimit) {
			currentGear--;
			gearFactor = minRPM;
		} else if (currentGear == 0) {
			currentGear--;
			gearFactor = minRPM;
		}
	}

	[ContextMenu("NitroON")]
	public void NitroOn() {
		if (!nitroOn && nitroAmount > 2.0f) {
			if (!isAudioMuted) {
				GameObject tempObject = Instantiate (Resources.Load ("Audio Clip - Nitro")) as GameObject;
				tempObject.name = "Audio Clip - Nitro";
				tempObject = null;
			}
			nitroFX.SetActive (true);
			topSpeed = topSpeed + nitroTopSpeed;
			fullTorqueOverAllWheels = fullTorqueOverAllWheels + nitroFullTorque;
			nitroOn = true;
		}
	}

	[ContextMenu("NitroOFF")]
	public void NitroOff() {
		if (nitroOn) {
			nitroFX.SetActive(false);
			topSpeed = topSpeed - nitroTopSpeed;
			fullTorqueOverAllWheels = fullTorqueOverAllWheels - nitroFullTorque;
			nitroOn = false;
		}
	}

	void SetupVehicleStats(){
		vehicleNumber = RGT_PlayerPrefs.GetVehicleNumber ();

		for(int i = 0; i < RGT_PlayerPrefs.GetVehicleTopSpeedLevel(vehicleNumber) + 1; i++){
			levelBonusTopSpeed += RGT_PlayerPrefs.playableVehicles.vehicleUpgrades [vehicleNumber].topSpeed [i];
		}
		topSpeed += levelBonusTopSpeed;

		for(int i = 0; i < RGT_PlayerPrefs.GetVehicleAccelerationLevel(vehicleNumber) + 1; i++){
			levelBonusAcceleration += RGT_PlayerPrefs.playableVehicles.vehicleUpgrades [vehicleNumber].acceleration [i];
		}
		fullTorqueOverAllWheels += levelBonusAcceleration;

		for(int i = 0; i < RGT_PlayerPrefs.GetVehicleBrakePowerLevel(vehicleNumber) + 1; i++){
			levelBonusBrakePower += RGT_PlayerPrefs.playableVehicles.vehicleUpgrades [vehicleNumber].brakePower [i];
		}
		brakeTorque += levelBonusBrakePower;

		for(int i = 0; i < RGT_PlayerPrefs.GetVehicleTireTractionLevel(vehicleNumber) + 1; i++){
			levelBonusTireTraction += RGT_PlayerPrefs.playableVehicles.vehicleUpgrades [vehicleNumber].tireTraction [i];
		}
		tractionControl += levelBonusTireTraction;

		for(int i = 0; i < RGT_PlayerPrefs.GetVehicleSteerSensitivityLevel(vehicleNumber) + 1; i++){
			levelBonusSteerSensitivity += RGT_PlayerPrefs.playableVehicles.vehicleUpgrades [vehicleNumber].steerSensitivity [i];
		}
		steerSensitivity += levelBonusSteerSensitivity;
	}

}