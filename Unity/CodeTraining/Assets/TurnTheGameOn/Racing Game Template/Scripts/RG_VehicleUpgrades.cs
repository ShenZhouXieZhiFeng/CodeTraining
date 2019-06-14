using UnityEngine;
using System.Collections;

namespace TurnTheGameOn.RacingGameTemplate{

	[System.Serializable]
	public class RG_VehicleUpgrades {
		
		public float[] tireTraction = new float[10];
		public float[] steerSensitivity = new float[10];
		public float[] topSpeed = new float[10];
		public float[] acceleration = new float[10];
		public float[] brakePower = new float[10];
		
	}
}