//Holds a static reference to all ScriptableObjects used in the project and provides an interface to get/set saved data using PlayerPrefs.

using UnityEngine;
using System.Collections;

namespace TurnTheGameOn.RacingGameTemplate{
	public static class RGT_PlayerPrefs {

		//ScriptablObject that contains race data
		private static RaceData RaceData;
		public static RaceData raceData{
			get{ 
				if (RaceData == null)
					RaceData = Resources.Load ("RaceData") as RaceData;
				return RaceData;
			}
		}
		//ScriptablObject that contains custom car color data
		private static PlayerPrefsData PlayerPrefsData;
		public static PlayerPrefsData playerPrefsData{
			get{ 
				if (PlayerPrefsData == null)
					PlayerPrefsData = Resources.Load ("PlayerPrefsData") as PlayerPrefsData;
				return PlayerPrefsData;
			}
		}
		//ScriptablObject that contains playable vehicle data
		private static PlayableVehicles PlayableVehicles;
		public static PlayableVehicles playableVehicles{
			get{ 
				if (PlayableVehicles == null)
					PlayableVehicles = Resources.Load ("PlayableVehicles") as PlayableVehicles;
				return PlayableVehicles;
			}
		}
		//ScriptablObject that contains opponent vehicle data
		private static OpponentVehicles OpponentVehicles;
		public static OpponentVehicles opponentVehicles{
			get{ 
				if (OpponentVehicles == null)
					OpponentVehicles = Resources.Load ("OpponentVehicles") as OpponentVehicles;
				return OpponentVehicles;
			}
		}
		//ScriptablObject that contains input settings
		private static InputData InputData;
		public static InputData inputData{
			get{ 
				if (InputData == null)
					InputData = Resources.Load ("InputData") as InputData;
				return InputData;
			}
		}
		//ScriptablObject that contains debug settings
		private static DebugData DebugData;
		public static DebugData debugData{
			get{ 
				if (DebugData == null)
					DebugData = Resources.Load ("DebugData") as DebugData;
				return DebugData;
			}
		}
		//ScriptablObject that contains audio settings
		private static AudioData AudioData;
		public static AudioData audioData{
			get{ 
				if (AudioData == null)
					AudioData = Resources.Load ("AudioData") as AudioData;
				return AudioData;
			}
		}




		#region Race Utility Methods
//Number Of Races
		public static int GetNumberOfRaces(){
			return PlayerPrefs.GetInt ("Number Of Races", 0);
		}
		public static void SetNumberOfRaces(int newValue){
			PlayerPrefs.SetInt ("Number Of Races", newValue);
		}
//Race Lock
		public static string GetRaceLock(string raceName){
			return PlayerPrefs.GetString ("Race Lock" + raceName, "LOCKED");
		}
		public static void SetRaceLock(string raceName, string newValue){
			PlayerPrefs.SetString ("Race Lock" + raceName, newValue);
		}
//Race Name
//		public static string GetRaceName(string raceName){
//			return PlayerPrefs.GetString ("Race Name" + raceName, "UNLOCKED");
//		}
		public static void SetRaceName(int raceNumber, string newValue){
			PlayerPrefs.SetString ("Race Name" + raceNumber.ToString(), newValue);
		}
//Race Status
		public static string GetRaceStatus(string raceName){
			return PlayerPrefs.GetString ("Race" + raceName + "Status");
		}
		public static void SetRaceStatus(string raceName, string newValue){
			PlayerPrefs.SetString ("Race" + raceName + "Status", newValue);
		}
//Race Number
		public static int GetRaceNumber(){
			return PlayerPrefs.GetInt ("Race Number", 0);
		}
		public static void SetRaceNumber(int newValue){
			PlayerPrefs.SetInt ("Race Number", newValue);
		}
//Race Laps
		public static int GetRaceLaps(int raceNumber, int defaultValue){
			return PlayerPrefs.GetInt ("Race " + raceNumber.ToString () + " Laps", defaultValue);
		}
		public static void SetRaceLaps(int raceNumber, int newValue){
			PlayerPrefs.SetInt ("Race " + raceNumber.ToString () + " Laps", newValue);
		}
//Race Rewards
		public static int GetRaceReward(int rewardNumber, int defaultValue){
			return PlayerPrefs.GetInt ("Race Reward " + rewardNumber.ToString(), defaultValue);
		}
		public static void SetRaceReward(int rewardNumber, int newValue){
			PlayerPrefs.SetInt ("Race Reward " + rewardNumber.ToString(), newValue);
		}
//Best Race Time
		public static float GetBestRaceTime(int raceNumber, float defaultValue){
			return PlayerPrefs.GetFloat ("Best Race Time " + raceNumber.ToString (), defaultValue);
		}
		public static void SetBestRaceTime(int raceNumber, float newValue){
			PlayerPrefs.SetFloat ("Best Race Time " + raceNumber.ToString (), newValue);
		}
//Best Lap Time
		public static float GetBestLapTime(int raceNumber, float defaultValue){
			return PlayerPrefs.GetFloat ("Best Lap Time " + raceNumber.ToString (), defaultValue);
		}
		public static void SetBestLapTime(int raceNumber, float newValue){
			PlayerPrefs.SetFloat ("Best Lap Time " + raceNumber.ToString (), newValue);
		}
		#endregion
//Currency
		public static int GetCurrency(){
			return PlayerPrefs.GetInt ("Currency", 0);
		}
		public static void SetCurrency(int newValue){
			PlayerPrefs.SetInt ("Currency", newValue);
		}
//Audio
		public static string GetAudio(){
			return PlayerPrefs.GetString ("Audio", "ON");
		}
		public static void SetAudio(string newValue){
			PlayerPrefs.SetString ("Audio", newValue);
		}
//Transmission
		public static string GetTransmission(){
			return PlayerPrefs.GetString ("Transmission", "Auto");
		}
		public static void SetTransmission(string newValue){
			PlayerPrefs.SetString ("Transmission", newValue);
		}
//Game Mode
		public static string GetGameMode(){
			return PlayerPrefs.GetString ("Game Mode", "SINGLE PLAYER");
		}
		public static void SetGameMode(string newValue){
			PlayerPrefs.SetString ("Game Mode", newValue);
		}
//Host Drop
		public static string GetHostDrop(){
			return PlayerPrefs.GetString("Host Drop");
		}
		public static void SetHostDrop(string newValue){
			PlayerPrefs.SetString ("Host Drop", newValue);
		}
//Mobile Steering Type
		public static int GetMobileSteeringType(){
			return PlayerPrefs.GetInt ("Mobile Steering Type", 0);
		}
		public static void SetMobileSteeringType(int newValue){
			PlayerPrefs.SetInt ("Mobile Steering Type", newValue);
		}
//Quality Settings
		public static int GetQualitySettings(){
			return PlayerPrefs.GetInt ("Quality Settings", 0);
		}
		public static void SetQualitySettings(int newValue){
			PlayerPrefs.SetInt ("Quality Settings", newValue);
		}
//Last Ad Time
		public static string GetLastAdTime(){
			return PlayerPrefs.GetString ("Last Ad Time");
		}
		public static void SetLastAdTime(string newValue){
			PlayerPrefs.SetString ("Last Ad Time", newValue);
		}
//Slot
		public static int GetSlot(){
			return PlayerPrefs.GetInt ("Slot", 0);
		}
		public static void SetSlot(int newValue){
			PlayerPrefs.SetInt ("Slot", newValue);
		}

		#region Vehicle Utility Methods
//Vehicle Number
		public static int GetVehicleNumber(){
			return PlayerPrefs.GetInt ("Vehicle Number", 0);
		}
		public static void SetVehicleNumber(int newValue){
			PlayerPrefs.SetInt ("Vehicle Number", newValue);
		}
//Vehicle Lock
		public static string GetVehicleLock(int vehicleNumber){
			return PlayerPrefs.GetString("Vehicle Lock" + vehicleNumber.ToString(), "LOCKED");
		}
		public static void SetVehicleLock(int vehicleNumber, string newValue){
			PlayerPrefs.SetString ("Vehicle Lock" + vehicleNumber.ToString(), newValue);
		}
//Vehicle Top Speed Level
		public static int GetVehicleTopSpeedLevel(int vehicleNumber){
			return PlayerPrefs.GetInt("Vehicle Top Speed Level" + vehicleNumber.ToString(), 0);
		}
		public static void SetVehicleTopSpeedLevel(int vehicleNumber, int newValue){
			PlayerPrefs.SetInt ("Vehicle Top Speed Level" + vehicleNumber.ToString(), newValue);
		}
//Vehicle Acceleration Level
		public static int GetVehicleAccelerationLevel(int vehicleNumber){
			return PlayerPrefs.GetInt("Vehicle Acceleration Level" + vehicleNumber.ToString(), 0);
		}
		public static void SetVehicleAccelerationLevel(int vehicleNumber, int newValue){
			PlayerPrefs.SetInt ("Vehicle Acceleration Level" + vehicleNumber.ToString(), newValue);
		}
//Vehicle Brake Power Level
		public static int GetVehicleBrakePowerLevel(int vehicleNumber){
			return PlayerPrefs.GetInt("Vehicle Brake Power Level" + vehicleNumber.ToString(), 0);
		}
		public static void SetVehicleBrakePowerLevel(int vehicleNumber, int newValue){
			PlayerPrefs.SetInt ("Vehicle Brake Power Level" + vehicleNumber.ToString(), newValue);
		}
//Vehicle Tire Traction Level
		public static int GetVehicleTireTractionLevel(int vehicleNumber){
			return PlayerPrefs.GetInt("Vehicle Tire Traction Level" + vehicleNumber.ToString(), 0);
		}
		public static void SetVehicleTireTractionLevel(int vehicleNumber, int newValue){
			PlayerPrefs.SetInt ("Vehicle Tire Traction Level" + vehicleNumber.ToString(), newValue);
		}
//Vehicle Steer Sensitivity Level
		public static int GetVehicleSteerSensitivityLevel(int vehicleNumber){
			return PlayerPrefs.GetInt("Vehicle Steer Sensitivity Level" + vehicleNumber.ToString(), 0);
		}
		public static void SetVehicleSteerSensitivityLevel(int vehicleNumber, int newValue){
			PlayerPrefs.SetInt ("Vehicle Steer Sensitivity Level" + vehicleNumber.ToString(), newValue);
		}
		#endregion


		#region Vehicle Color Customization Methods
//Vehicle Body Color
		public static float GetVehicleBodyColor(string color, int vehicleNumber, float defaultValue){
			return PlayerPrefs.GetFloat(color + " " + vehicleNumber.ToString(), defaultValue);
		}
		public static void SetVehicleBodyColor(string color, int vehicleNumber, float newValue){
			PlayerPrefs.SetFloat(color + " " + vehicleNumber.ToString(), newValue);
		}
//Vehicle Neon Light Color
		public static float GetVehicleNeonLightColor(string color, int vehicleNumber, float defaultValue){
			return PlayerPrefs.GetFloat(color + " Neon Light " + vehicleNumber.ToString(), defaultValue);
		}
		public static void SetVehicleNeonLightColor(string color, int vehicleNumber, float newValue){
			PlayerPrefs.SetFloat(color + " Neon Light " + vehicleNumber.ToString(), newValue);
		}
//Vehicle Brake Color
		public static float GetVehicleBrakeColor(string color, int vehicleNumber, float defaultValue){
			return PlayerPrefs.GetFloat(color + " Brake " + vehicleNumber.ToString(), defaultValue);
		}
		public static void SetVehicleBrakeColor(string color, int vehicleNumber, float newValue){
			PlayerPrefs.SetFloat(color + " Brake " + vehicleNumber.ToString(), newValue);
		}
//Vehicle Rim Color
		public static float GetVehicleRimColor(string color, int vehicleNumber, float defaultValue){
			return PlayerPrefs.GetFloat(color + " Rim " + vehicleNumber.ToString(), defaultValue);
		}
		public static void SetVehicleRimColor(string color, int vehicleNumber, float newValue){
			PlayerPrefs.SetFloat(color + " Rim " + vehicleNumber.ToString(), newValue);
		}
//Vehicle Glass Color
		public static float GetVehicleGlassColor(string color, int vehicleNumber, float defaultValue){
			return PlayerPrefs.GetFloat(color + " Glass " + vehicleNumber.ToString(), defaultValue);
		}
		public static void SetVehicleGlassColor(string color, int vehicleNumber, float newValue){
			PlayerPrefs.SetFloat(color + " Glass " + vehicleNumber.ToString(), newValue);
		}
		#endregion
	}
}