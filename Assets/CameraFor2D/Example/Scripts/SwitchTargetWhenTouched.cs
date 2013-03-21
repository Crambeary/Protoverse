using UnityEngine;
using System.Collections;

public class SwitchTargetWhenTouched : MonoBehaviour {
	public CameraController2D cameraController;
	public Transform targetToSwitchTo;
	public float moveSpeed;

	void Start() {
		if(cameraController == null) {
			cameraController = Camera.main.GetComponent<CameraController2D>();
		}
	}

	void OnTriggerEnter() {
		cameraController.SetTarget(targetToSwitchTo, moveSpeed);
	}
}