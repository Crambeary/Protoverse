using UnityEngine;
using System.Collections;
using GoodStuff.NaturalLanguage;

public class FocusPointOfInterest : MonoBehaviour {	
	public CameraController2D cameraController;
	public GameObject target;
	public float focusDistance;
	public float exclusiveFocusPercentage = .25f;

	public bool drawDebugLines;

	float focusDistanceSquared;
	Vector3 influencePoint;

	void Start() {
		if(cameraController == null) {
			cameraController = Camera.main.GetComponent<CameraController2D>();
		}
		focusDistanceSquared = focusDistance * focusDistance;
	}

	void Update() {
		var vectorToTarget = target.transform.position - transform.position;
		var distanceSquared = vectorToTarget.sqrMagnitude;

		if(distanceSquared < focusDistanceSquared) {
			var percentOfDistance = (vectorToTarget.magnitude / focusDistance).MapToRange(exclusiveFocusPercentage, 1, 0, 1, true);
			cameraController.AddInfluence(-vectorToTarget * (1 - percentOfDistance));
		}
	}
	
	void OnDrawGizmos() {
		if(drawDebugLines) {
			Gizmos.color = Color.blue;
			Gizmos.DrawWireSphere(transform.position, focusDistance);
		}
	}
}