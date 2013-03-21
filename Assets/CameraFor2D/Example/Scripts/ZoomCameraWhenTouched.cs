using UnityEngine;
using System.Collections;
using GoodStuff.NaturalLanguage;

public class ZoomCameraWhenTouched : MonoBehaviour {
	public CameraController2D cameraController;
	public float zoomAmount;

	float disabledUntilTime;

	void Start() {
		if(cameraController == null) {
			cameraController = Camera.main.GetComponent<CameraController2D>();
		}
	}

	void OnTriggerEnter() {
		if(Time.time > disabledUntilTime) {
			iTween.ValueTo(gameObject, iTween.Hash("from", 1, "to", 3, "time", 1.5f, "onupdate", "UpdateCameraZoom", "oncomplete", "ZoomDown"));
			disabledUntilTime = Time.time + 3;
		}
	}

	public void UpdateCameraZoom(float value) {
		cameraController.DistanceMultiplier = value;
	}

	public void ZoomDown() {
		iTween.ValueTo(gameObject, iTween.Hash("from", 3, "to", 1, "time", 1.5f, "onupdate", "UpdateCameraZoom"));
	}
}