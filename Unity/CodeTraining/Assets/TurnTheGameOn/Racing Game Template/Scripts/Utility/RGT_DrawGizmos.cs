using UnityEngine;
using System.Collections;

namespace TurnTheGameOn.RacingGameTemplate{
	public class RGT_DrawGizmos : MonoBehaviour {

		#region Public Variables
		public Color gizmoColor = Color.green;
		public Vector3 gizmoSize = new Vector3 (0.14f, 0.14f, 0.14f);
		#endregion

		#region Main Methods	
		void OnDrawGizmos(){
			
			Gizmos.color = gizmoColor;
			Gizmos.matrix = transform.localToWorldMatrix;
			Gizmos.DrawWireCube (Vector3.zero, gizmoSize);
		}
		#endregion

	}
}