using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class KeepFocusedTargetsOnscreen : MonoBehaviour {
	public CameraController2D cameraController;
	// percentage of screen edge that triggers zooming out if any target is in
	public float zoomOutBorder = .05f;
	// percentage of screen edge that triggers zooming in if all targets are in
	public float zoomInBorder = .3f;
	public float multiplierChangeSpeed = 1;
	public float maxMultiplier = 2;

	// pre-allocate array so we aren't generating a lot of garbage each update
	Vector2[] viewportPoints = new Vector2[10];

	bool needToRecalculateScreenBounds;

	void Start() {
		if(cameraController == null) {
			cameraController = Camera.main.GetComponent<CameraController2D>();
		}
	}

	void LateUpdate() {
		// Don't zoom out if we're panning to a new target such as when doing a reveal after hitting a switch
		if(cameraController.MovingToNewTarget) return;

		var targets = cameraController.CurrentTarget;
		if(targets.Count() > viewportPoints.Count()) {
			viewportPoints = new Vector2[targets.Count()];
		}

		var i = 0;
		bool zoomOut = false;
		bool zoomIn = true;
		var farZoomOutBorder = 1 - zoomOutBorder;
		var farZoomInBorder = 1 - zoomInBorder;

		foreach(var target in targets) {
			viewportPoints[i] = cameraController.camera.WorldToViewportPoint(target.transform.position);
			// determine if any target is in the zoom out border
			if(viewportPoints[i].x < zoomOutBorder || viewportPoints[i].x > farZoomOutBorder || viewportPoints[i].y < zoomOutBorder || viewportPoints[i].y > farZoomOutBorder) {
				zoomOut = true;
				needToRecalculateScreenBounds = true;
			}

			// determine if all targets are in the zoom in border
			if(viewportPoints[i].x < zoomInBorder || viewportPoints[i].x > farZoomInBorder || viewportPoints[i].y < zoomInBorder || viewportPoints[i].y > farZoomInBorder) {
				zoomIn = false;
				needToRecalculateScreenBounds = true;
			}

			++i;
		}

		if(zoomOut) {
			var zoomAmount = multiplierChangeSpeed * Time.deltaTime;
			if(cameraController.DistanceMultiplier + zoomAmount > maxMultiplier) cameraController.DistanceMultiplier = maxMultiplier;
			else cameraController.DistanceMultiplier += zoomAmount;
		}
		else if(zoomIn) {
			var zoomAmount = multiplierChangeSpeed * Time.deltaTime;
			if(cameraController.DistanceMultiplier - zoomAmount < 1) cameraController.DistanceMultiplier = 1;
			else cameraController.DistanceMultiplier -= zoomAmount;
		}
		else {
			if(needToRecalculateScreenBounds) cameraController.CalculateScreenBounds();
		}
	}
}