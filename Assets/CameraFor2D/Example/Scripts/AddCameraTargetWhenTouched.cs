using UnityEngine;
using System.Collections;

public class AddCameraTargetWhenTouched : MonoBehaviour {
	public CameraController2D cameraController;
	public Transform[] targets;
	public float moveSpeed;
	public bool removeTargetAfterDelay;
	public float delay = 5;
	public float revertMoveSpeed;

	public bool triggerTweenAtTarget;
	public GameObject tweenTarget;
	public string tweenName;
	
	void Start() {
		if(cameraController == null) {
			cameraController = Camera.main.GetComponent<CameraController2D>();
		}
	}

	void OnTriggerEnter() {

		if(removeTargetAfterDelay) {
			cameraController.AddTarget(targets, moveSpeed, delay, revertMoveSpeed);
		}
		else {
			cameraController.AddTarget(targets, moveSpeed);
		}

		if(triggerTweenAtTarget) {
			cameraController.OnNewTargetReached += StartTween;
		}
	}

	void StartTween() {
		iTweenEvent.GetEvent(tweenTarget, tweenName).Play();
		cameraController.OnNewTargetReached -= StartTween;
	}
}