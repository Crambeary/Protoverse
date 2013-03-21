using UnityEngine;
using System.Collections;

public class AdjustCameraMoveSpeedWhenTouched : MonoBehaviour {
	public CameraController2D cameraController;
	public float cameraMoveSpeed;

	void Start() {
		if(cameraController == null) {
			cameraController = Camera.main.GetComponent<CameraController2D>();
		}
	}

	void OnTriggerEnter() {
		cameraController.maxMoveSpeedPerSecond = cameraMoveSpeed;
	}
}