using UnityEngine;
using System.Collections;

public class AnimateFlowOnSubstance : MonoBehaviour {
	public ProceduralMaterial substance;
	public float flowSpeed = 1;

	void Update() {
		if(substance != null) {
			substance.SetProceduralFloat("Flow", substance.GetProceduralFloat("Flow") + Time.deltaTime * flowSpeed);
			substance.RebuildTextures();
		}
	}
}