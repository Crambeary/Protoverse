using UnityEngine;
using System.Collections;

public class ChangeCameraSpeedWhenTouched : MonoBehaviour {
	public CameraController2D cameraController;
	public float speed;

	void Start() {
		if(cameraController == null) {
			cameraController = Camera.main.GetComponent<CameraController2D>();
		}
	}

	void OnTriggerEnter() {
		cameraController.maxMoveSpeedPerSecond = speed;
	}
}