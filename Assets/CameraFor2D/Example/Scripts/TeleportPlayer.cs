using UnityEngine;
using System.Collections;

public class TeleportPlayer : MonoBehaviour {
	public CameraController2D cameraController;
	public Transform teleportTarget;
	public bool snapCameraToTarget = true;

	void Start() {
		if(cameraController == null) {
			cameraController = Camera.main.GetComponent<CameraController2D>();
		}
	}

	void OnTriggerEnter(Collider other) {
		if(other.tag == "Player") {
			other.transform.position = teleportTarget.position;
			other.transform.rotation = teleportTarget.rotation;
			if(snapCameraToTarget) {
				cameraController.ExclusiveModeEnabled = true;
				cameraController.JumpToIdealPosition();
				cameraController.ExclusiveModeEnabled = false;
			}
		}
	}
}
