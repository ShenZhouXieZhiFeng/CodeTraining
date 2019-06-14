using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TurnTheGameOn.RacingGameTemplate{
	public class RG_PlatformOptions : MonoBehaviour {

		[Header("WebGL")]
		public GameObject[] disableObjectsWebgl;

		void OnEnable () {
			#if UNITY_WEBGL
			for (int i = 0; i < disableObjectsWebgl.Length; i++) {
				disableObjectsWebgl [i].SetActive (false);
			}
			#endif
		}

	}
}