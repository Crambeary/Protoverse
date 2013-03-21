using UnityEngine;
using System.Collections;

public class DisablePlayerControlsTemporarily : MonoBehaviour {
	public float disableTime;

	void OnTriggerEnter(Collider other) {
		if(other.tag == "Player") {
			StartCoroutine(DisableControls(other.gameObject, disableTime));
		}
	}

	IEnumerator DisableControls(GameObject target, float time) {
		var controls = target.GetComponent<PlayerMovement>();
		controls.enabled = false;
		yield return new WaitForSeconds(time);
		controls.enabled = true;
	}
}