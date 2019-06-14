using UnityEngine;
using System.Collections;

public class RaceData : ScriptableObject {
	public enum Switch { Off, On }
	public enum GameType { Circuit, PointToPoint, Elimination, TimeAttack, TimeTrial }

	//Total number of races the game contains.
	public int numberOfRaces;
	//Index for all race names the game contains.
	public string[] raceNames;
	//Index for all race images the game contains.
	public Sprite[] raceImages;
	//Index of race game types.
	public GameType[] gameType;
	//Index to toggle race locks.
	public bool[] raceLocked;
	//Index for race unlock amount.
	public int[] unlockAmount;
	//Index to determine if players can continue to receive rewards after completing a race.
	public bool[] unlimitedRewards;
	//Index for race first prize amount.
	public int[] firstPrize;
	//Index for race first prize amount.
	public int[] secondPrize;
	//Index for race first prize amount.
	public int[] thirdPrize;
	//Current race number.
	public int raceNumber;
	//Index for total laps in each race.
	public int[] raceLaps;
	//Index to set max selectable lap count in garage scene.
	public int[] lapLimit;
	//Index for current race final standings reward.
	public int[] raceRewards;
	//Index for race racer count.
	[Range(1,64)]public int[] numberOfRacers;
	//Index to set max selectable racer count in garage scene.
	[Range(1,64)]public int[] racerLimit;
//Current vehicle number.
	public int vehicleNumber;
	//Best race finish time.
	public float bestRaceTime;
	//Best race lap finish time.
	public float bestLapTime;
//Current player currency.
	public int currency;
	//Allow uses the ability to purchase level unlocks with currency in the garage menu?
	public Switch purchaseLevelUnlock;
	//Auto unlock the next race after finishing first place in the current race?
	public Switch autoUnlockNextRace;
//Garage scene scene-locked button text.
	public string lockButtonText;
	//Delay before wrong way message will appear on the screen.
	public float wrongWayDelay;
	//Countdown to start delay for each race.
	public float[] readyTime;
	//Modifier to reward the player with extra currency based on the number of laps.
	public int[] extraLapRewardMultiplier;
	//Modifier to reward the player with extra currency based on the number of racers.
	public int[] extraRacerRewardMultiplier;
	//Should the waypoint mesh's be shown in races
	public bool[] showWaypoints;
	//Should the player waypoint arrow be shown in races
	public bool[] showWaypointArrow;
}