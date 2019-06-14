using UnityEngine;
using System.Collections;

public class OpponentVehicles : ScriptableObject {

	//Opponent names.
	public string[] opponentNames;
	//Opponent vehicle prefabs.
	public GameObject[] vehicles;
	//Opponent vehicle body materials.
	public Material[] opponentBodyMaterials;
	//Opponent vehicle body material colors.
	public Color[] opponentBodyColors;
	//Opponent vehicle glass materials.
	public Material[] opponentGlassMaterials;
	//Opponent vehicle glass material colors.
	public Color[] opponentGlassColors;
}