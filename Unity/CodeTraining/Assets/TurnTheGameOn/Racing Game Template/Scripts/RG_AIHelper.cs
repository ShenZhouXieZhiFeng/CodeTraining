using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
[ExecuteInEditMode]
[RequireComponent(typeof (RG_CarController))]
public class RG_AIHelper : MonoBehaviour {

	public enum ProgressStyle { SmoothAlongRoute, PointToPoint,	}
	public enum BrakeCondition	{
		NeverBrake,                 // the car simply accelerates at full throttle all the time.
		TargetDirectionDifference,  // the car will brake according to the upcoming change in direction of the target. Useful for route-based AI, slowing for corners.
		TargetDistance,             // the car will brake as it approaches its target, regardless of the target's direction. Useful if you want the car to head for a stationary target and come to rest when it arrives there.
	}

	//References
	public RG_WaypointCircuit circuit;		// A route of waypoints used for pathfinding
	public Transform target;														// Current waypoint to drive toward
	public RG_AIDetection detection;												// AI sensor system used for obstacle detection and avoidance
	private RG_SceneManager sceneManager;
	private RG_CarController carController;

	[Header("Look Ahead For Path")]
	public float lookDistance = 25;											//Distance the AI vehicle will look ahead
	public float speedAddLookAhead = 0.75f;									//Adds additional distance distance to "look ahead" based on current speed
	[Range(0, 180)] public float cautiousAngle = 40;						// angle of approaching corner to treat as warranting maximum caution
	public float cautiousDistance = 100f;									// distance at which distance-based cautiousness begins
	public float cautiousAngularVelocity = 30f;								// how cautious the AI should be when considering its own current angular velocity (i.e. easing off acceleration if spinning!)
	public ProgressStyle progressStyle = ProgressStyle.SmoothAlongRoute;	// whether to update the position smoothly along the route (good for curved paths) or just when we reach each waypoint.
	public float reachTargetThreshold = 2;									//Determines distance between target to consider it reached
	public float pointToPointThreshold = 4;
	public BrakeCondition m_BrakeCondition = BrakeCondition.TargetDistance;	// what should the AI consider when accelerating/braking?

	[Header("AI Input & Sensitivity")]
	[Range(0,1)] public float maxSpeedFactor = 1.0f;
	[Space()][Range(0.001f, 1.0f)] public float steerSensitivity = 0.01f;		//Input multiplier
	[Range(0.001f, 1.0f)] public float AccelerationSensitivity = 1f;			//Input multiplier
	[Range(0.001f, 1.0f)] public float BrakeSensitivity = 1f; 					//Input multiplier
	private float defaultSteerSensitivity;										//Input multiplier

	[Header("Teleport to Waypoint")]
	public float respawnYOffset = 5.0f;
	public float stuckSpeedFactor = 0.05f;
	public float stuckReset = 7.0f;
	public float resetAfter = 60.0f;
	private float timer;
	private float stuckTimer;
	private Vector3 tempPosition;

	[Header("Obstacle Avoidance")]
	public float avoidPathOffset = 15.0f;
	private Vector3 routePointTargetPosition;
	public float rotationSpeed;

	//Runtime variables
	[HideInInspector] public int opponentNumber;
	private float waypointSpeedFactor = 1;
	private float cautiousSpeedFactor = 0.05f;				// percentage of max speed factor to use when being maximally cautious
	private float currentSpeed;								// current speed of this object (calculated from delta since last frame)
	private float desiredSpeed;
	private float progressDistance;							// The progress round the route, used in smooth mode.
	private int progressNum;								// the current waypoint number, used in point-to-point mode.
	private Vector3 lastPosition;							// Used to calculate current speed (since we may not have a rigidbody component)
	private bool avoidingObstacle;							//
	private bool randomLeft;

	// these are public, readable by other objects - i.e. for an AI to know where to head!
	public RG_WaypointCircuit.RoutePoint targetPoint { get; private set; }
	public RG_WaypointCircuit.RoutePoint speedPoint { get; private set; }
	public RG_WaypointCircuit.RoutePoint progressPoint { get; private set; }

	bool targetingZero = true;

	private void Awake(){
		carController = GetComponent<RG_CarController>();
		//carController.rbody.AddForce (Vector3.forward * 50000, ForceMode.Impulse );
	}

	void Start(){
		defaultSteerSensitivity = steerSensitivity;
		sceneManager = GameObject.Find ("Scene Manager").GetComponent<RG_SceneManager>();
		circuit = GameObject.Find("Scene Manager").GetComponent<RG_WaypointCircuit>();
		if (target == null)
			target = new GameObject(name + " Waypoint Target").transform;
		progressDistance = 0;
		progressNum = 0;
		if (progressStyle == ProgressStyle.PointToPoint){
			target.position = circuit.Waypoints[progressNum].position;
			target.rotation = circuit.Waypoints[progressNum].rotation;
		}
	}

	void Update () {
		if (sceneManager.racerInfo [opponentNumber].nextWP == 0 && sceneManager.gameTime < 30.0f) {
			targetingZero = true;
			target.position = circuit.Waypoints [0].position;
			target.rotation = circuit.Waypoints [0].rotation;
		} else {
			targetingZero = false;
		}
		if(!avoidingObstacle) randomLeft = !randomLeft;	
		if (progressStyle == ProgressStyle.SmoothAlongRoute){
			// determine the position we should currently be aiming for
			// (this is different to the current progress position, it is a a certain amount ahead along the route)
			// we use lerp as a simple way of smoothing out the speed over time.
			if (Time.deltaTime > 0)	{
				currentSpeed = Mathf.Lerp(currentSpeed, (lastPosition - transform.position).magnitude/Time.deltaTime, Time.deltaTime);
			}

			if (!targetingZero) {
				if (avoidingObstacle) {
					routePointTargetPosition = circuit.GetRoutePoint (progressDistance + lookDistance + speedAddLookAhead * currentSpeed).position;
					if (!detection.sensorHits.forwardRight1Hit && !detection.sensorHits.forwardRight2Hit && !detection.sensorHits.forwardRight3Hit) { 
						if (!detection.sensorHits.forwardLeft1Hit && !detection.sensorHits.forwardLeft2Hit && !detection.sensorHits.forwardLeft3Hit && randomLeft) {
							target.position = routePointTargetPosition - (Vector3.right * avoidPathOffset);
						} else {
							target.position = routePointTargetPosition + (Vector3.right * avoidPathOffset);
						}
					} else {
						target.position = routePointTargetPosition - (Vector3.right * avoidPathOffset);
					}
					target.rotation = Quaternion.LookRotation (circuit.GetRoutePoint (progressDistance + speedAddLookAhead * currentSpeed).direction);
				} else {
					routePointTargetPosition = circuit.GetRoutePoint (progressDistance + lookDistance + speedAddLookAhead * currentSpeed).position;
					target.position = routePointTargetPosition;
					target.rotation = Quaternion.LookRotation (circuit.GetRoutePoint (progressDistance + speedAddLookAhead * currentSpeed).direction);
				}
			}

			// get our current progress along the route
			progressPoint = circuit.GetRoutePoint(progressDistance);
			Vector3 progressDelta = progressPoint.position - transform.position;
			if (Vector3.Dot(progressDelta, progressPoint.direction) < 0){
				progressDistance += progressDelta.magnitude*0.5f;
			}
			lastPosition = transform.position;
		}
		else{
			// point to point mode. Just increase the waypoint if we're close enough:
			Vector3 targetDelta = target.position - transform.position;
			if (targetDelta.magnitude < pointToPointThreshold)	{
				progressNum = (progressNum + 1)%circuit.Waypoints.Length;
			}

			if (!targetingZero) {
				target.position = circuit.Waypoints [progressNum].position;
				target.rotation = circuit.Waypoints [progressNum].rotation;
			}

			// get our current progress along the route
			progressPoint = circuit.GetRoutePoint(progressDistance);
			Vector3 progressDelta = progressPoint.position - transform.position;
			if (Vector3.Dot(progressDelta, progressPoint.direction) < 0){
				progressDistance += progressDelta.magnitude;
			}
			lastPosition = transform.position;
		}

		if (carController.speed < 1.0f && sceneManager.countUp && sceneManager.gameTime > 0.5f) {
			if (detection.sensorHits.forward1Hit || detection.sensorHits.forward2Hit || detection.sensorHits.forward3Hit) {
				if (!detection.sensorHits.right2Hit) {
					//Rotate right
					transform.Rotate (0, Time.deltaTime * (-rotationSpeed * 20), 0, Space.World);
					cautiousSpeedFactor = stuckSpeedFactor;
				}else if (!detection.sensorHits.left2Hit) {
					//Rotate left
					transform.Rotate (0, Time.deltaTime * (rotationSpeed * 20), 0, Space.World);
					cautiousSpeedFactor = stuckSpeedFactor;
				}else{
					transform.position = target.position;
					transform.rotation = target.rotation;
				}
			}
		}
		if (sceneManager.countUp) ObstacleDetection ();
		if (sceneManager.countUp) StuckCheck ();
	}

	void ObstacleDetection(){
		//Check if we need to turn right
		if (detection.sensorHits.forward1Hit || detection.sensorHits.forwardLeft1Hit || detection.sensorHits.forwardLeft2Hit || detection.sensorHits.forwardLeft3Hit || detection.forwardObstacle) {
			if (!detection.sensorHits.forwardRight1Hit && !detection.sensorHits.forwardRight2Hit && !detection.sensorHits.forwardRight3Hit) { 
				transform.Rotate (0, Time.deltaTime * (rotationSpeed * 20), 0, Space.World);
				//avoidingObstacle = true;
			}
			cautiousSpeedFactor = 0.1f;
		}

		//Check if we need to turn left
		if (detection.sensorHits.forward3Hit || detection.sensorHits.forwardRight1Hit || detection.sensorHits.forwardRight2Hit || detection.sensorHits.forwardRight3Hit || detection.forwardObstacle) {
			if (!detection.sensorHits.forwardLeft1Hit && !detection.sensorHits.forwardLeft2Hit && !detection.sensorHits.forwardLeft3Hit) {
				transform.Rotate (0, Time.deltaTime * (-rotationSpeed * 20), 0, Space.World);
				//avoidingObstacle = true;
			}
			cautiousSpeedFactor = 0.1f;
		}
		avoidingObstacle = false;
		if (detection.forwardObstacle) {
			//transform.Rotate (0, Time.deltaTime * (rotationSpeed * 20), 0, Space.World);
			avoidingObstacle = true;
		} else {
			cautiousSpeedFactor = 0.1f;
		}
		//check if we detect a vehicle in front of us, if not go faster
		if (!detection.sensorHits.forward2Hit || detection.forwardObstacle) {
			steerSensitivity = defaultSteerSensitivity;
			cautiousSpeedFactor = maxSpeedFactor;
		}
	}

	void StuckCheck (){
		timer += Time.deltaTime * 1;
		if (timer >= resetAfter) {
			timer = 0;
			transform.position = target.position;
			transform.rotation = target.rotation;
		}
		if (stuckTimer >= stuckReset) {
			stuckTimer = 0;
			transform.position = target.position;
			transform.rotation = target.rotation;
		}
		if (detection.sensorHits.forward1Hit || detection.sensorHits.forward2Hit || detection.sensorHits.forward3Hit || detection.forwardObstacle) {
			if (stuckTimer > 2.0f) {
				carController.overrideBrake = true;
				carController.overrideAcceleration = true;
				carController.overrideSteering = true;
				carController.overrideBrakePower = 0.75f;
				carController.overrideAccelerationPower = 0;
				carController.overrideSteeringPower = 0;
			}
		}else {
			carController.overrideBrake = false;
			carController.overrideAcceleration = false;
			carController.overrideSteering = false;
			carController.overrideBrakePower = 0;
			carController.overrideAccelerationPower = 1;
			cautiousSpeedFactor = maxSpeedFactor;
		}
		if (carController.speed < 5.0f) {
			stuckTimer += Time.deltaTime * 1;
		} else {
			stuckTimer = 0;
		}
	}

	private void FixedUpdate() {
		// use handbrake to stop	carController.Move(0, 0, -1f, 1f);
		Vector3 fwd = transform.forward;
		if (carController.rbody.velocity.magnitude > carController.MaxSpeed*0.1f) {
			fwd = carController.rbody.velocity;
		}
		desiredSpeed = carController.MaxSpeed;
		// now it's time to decide if we should be slowing down...
		switch (m_BrakeCondition)                {
		case BrakeCondition.TargetDirectionDifference:
			{
				// the car will brake according to the upcoming change in direction of the target. Useful for route-based AI, slowing for corners.
				// check out the angle of our target compared to the current direction of the car
				float approachingCornerAngle = Vector3.Angle(target.forward, fwd);
				// also consider the current amount we're turning, multiplied up and then compared in the same way as an upcoming corner angle
				float spinningAngle = carController.rbody.angularVelocity.magnitude*cautiousAngularVelocity;
				// if it's different to our current angle, we need to be cautious (i.e. slow down) a certain amount
				float cautiousnessRequired = Mathf.InverseLerp(0, cautiousAngle, Mathf.Max(spinningAngle, approachingCornerAngle));
				desiredSpeed = Mathf.Lerp(carController.MaxSpeed, carController.MaxSpeed*cautiousSpeedFactor, cautiousnessRequired);
				break;
			}
		case BrakeCondition.TargetDistance:
			{
				// the car will brake as it approaches its target, regardless of the target's direction. Useful if you want the car to
				// head for a stationary target and come to rest when it arrives there.
				// check out the distance to target
				Vector3 delta = target.position - transform.position;
				float distanceCautiousFactor = Mathf.InverseLerp(cautiousDistance, 0, delta.magnitude);
				// also consider the current amount we're turning, multiplied up and then compared in the same way as an upcoming corner angle
				float spinningAngle = carController.rbody.angularVelocity.magnitude*cautiousAngularVelocity;
				// if it's different to our current angle, we need to be cautious (i.e. slow down) a certain amount
				float cautiousnessRequired = Mathf.Max(	Mathf.InverseLerp(0, cautiousAngle, spinningAngle), distanceCautiousFactor);
				desiredSpeed = Mathf.Lerp(carController.MaxSpeed, carController.MaxSpeed*cautiousSpeedFactor,cautiousnessRequired);
				break;
			}
		case BrakeCondition.NeverBrake:
			break;
		}

		desiredSpeed = desiredSpeed * waypointSpeedFactor;

		// use different sensitivity depending on whether accelerating or braking:
		float accelBrakeSensitivity = (desiredSpeed < carController.CurrentSpeed)	? BrakeSensitivity : AccelerationSensitivity;
		// decide the actual amount of accel/brake input to achieve desired speed.
		float accel = Mathf.Clamp((desiredSpeed - carController.CurrentSpeed)*accelBrakeSensitivity, -1, 1);
		// calculate the local-relative position of the target, to steer towards
		Vector3 localTarget = transform.InverseTransformPoint(target.position);
		// work out the local angle towards the target
		float targetAngle = Mathf.Atan2(localTarget.x, localTarget.z)*Mathf.Rad2Deg;
		// get the amount of steering needed to aim the car towards the target
		float steer = Mathf.Clamp(targetAngle*steerSensitivity, -1, 1)*Mathf.Sign(carController.CurrentSpeed);
		// feed input to the car controller.
		carController.Move(steer, accel, accel, 0f);
	}

	public void ResetTimer(){
		timer = 0;
	}

	public void AdjustAIWaypointSpeedFactor (float newSpeed) {
		waypointSpeedFactor = newSpeed;
	}

	private void OnDrawGizmos()	{
		if (Application.isPlaying && circuit != null){
			Gizmos.color = Color.green;
			Gizmos.DrawLine(transform.position, target.position);
			Gizmos.DrawWireSphere(circuit.GetRoutePosition(progressDistance), 1);
			Gizmos.color = Color.yellow;
			Gizmos.DrawLine(target.position, target.position + target.forward);
		}
	}
	
}