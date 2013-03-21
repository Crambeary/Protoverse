using UnityEngine;
using System.Collections;

public class SwitchPlayerInput : MonoBehaviour {
	public PlayerMovement playerToDisable;
	public PlayerMovement playerToEnable;

	void OnTriggerEnter() {
		playerToDisable.enabled = false;
		playerToEnable.enabled = true;
	}
}