using UnityEngine;
using System.Collections;

public class TurnOffSwitchWhenTouched : MonoBehaviour {
	public Material turnedOffMaterial;

	void OnTriggerEnter() {
		collider.enabled = false;
		renderer.sharedMaterial = turnedOffMaterial;
	}
}
