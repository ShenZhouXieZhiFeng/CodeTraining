using UnityEngine;
using System.Collections;
using TurnTheGameOn.RacingGameTemplate;

public class PlayableVehicles : ScriptableObject {

	//Currency given to the player the first time the game is launched.
	public int startingCurrency;
	//Price to paint vehicle body color.
	public int paintPrice;
	//Price to paint vehicle brakes color.
	public int brakeColorPrice;
	//Price to paint vehicle rims color.
	public int rimColorPrice;
	//Price to change vehicle glass tint color.
	public int glassColorPrice;
	//Price to change vehicle neon light color.
	public int glowPrice;
	//Price to upgrade vehicle top speed.
	public int upgradeSpeedPrice;
	//Price to upgrade vehicle acceleration.
	public int upgradeAccelerationPrice;
	//Price to upgrade vehicle brake power.
	public int upgradeBrakesPrice;
	//Price to upgrade vehicle tire traction.
	public int upgradeTiresPrice;
	//Price to upgrade vehicle steer sensitivity.
	public int upgradeSteeringPrice;
	//Current selected vehicle.
	public int currentVehicleNumber;
	//Number of playable vehicles.
	public int numberOfCars;
	//Player name.
	public string playerName = "Player";
	//Playable vehicle prefabs.
	public GameObject[] vehicles;
	//Playable vehicle names.
	public string[] vehicleNames;
	//Playable vehicle prices.
	public int[] price;
	//Playable vehicle body materials.
	public Material[] carMaterial;
	//Playable vehicle brake materials.
	public Material[] brakeMaterial;
	//Playable vehicle glass materials.
	public Material[] glassMaterial;
	//Playable vehicle rim materials.
	public Material[] rimMaterial;
	//Playable vehicle default body material color.
	public Color[] defaultBodyColors;
	//Playable vehicle default brake material color.
	public Color[] defaultBrakeColors;
	//Playable vehicle default glass material color.
	public Color[] defaultGlassColors;
	//Playable vehicle default rim material color.
	public Color[] defaultRimColors;
	//Playable vehicle default neon light particle effect color.
	public Color[] defaultNeonColors;
	//Playable vehicle neon light particle system.
	public ParticleSystem[] carGlowLight;
	//Playable vehicle unlock status.
	public bool[] carUnlocked;
	//Playable vehicle top speed level.
	[Range(0,9)] public int[] topSpeedLevel;
	//Playable vehicle acceleration level.
	[Range(0,9)] public int[] torqueLevel;
	//Playable vehicle brake power level.
	[Range(0,9)] public int[] brakeTorqueLevel;
	//Playable vehicle tire traction level.
	[Range(0,9)] public int[] tireTractionLevel;
	//Playable vehicle steer sensitivity level.
	[Range(0,9)] public int[] steerSensitivityLevel;
	//Garage scene paint shop menu preset button body colors.
	public Color[] carBodyColorPreset;
	//Garage scene paint shop menu preset button glass colors.
	public Color[] carGlassColorPreset;
	//Garage scene paint shop menu preset button brake colors.
	public Color[] carBrakeColorPreset;
	//Garage scene paint shop menu preset button rim colors.
	public Color[] carRimColorPreset;
	//Garage scene paint shop menu preset button neon light colors.
	public Color[] carNeonColorPreset;
	//Toggle rear view mirror rig object on or off.
	public bool rearMirror = true;
	//Toggle side view mirror rig object on or off.
	public bool sideMirrors = true;
	//Toggle side view mirror rig object on or off.
	public bool reflectionProbe = true;
	//Spedometer
	public RG_DistanceMetrics.SpeedType[] speedometerType;
	//Vehicle Upgrades
	public RG_VehicleUpgrades[] vehicleUpgrades;
	//mini map rotation
	public bool fixedMiniMapRotation;
	//manual transmission
	public bool manual;

	public bool EVPSupport;

	#if EVP_SUPPORT
	public enum CarPrefabType { Edys, Default }

	public CarPrefabType[] carPrefabType;
	#endif
}