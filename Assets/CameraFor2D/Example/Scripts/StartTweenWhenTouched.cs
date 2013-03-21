using UnityEngine;
using System.Collections;

public class StartTweenWhenTouched : MonoBehaviour {
	public GameObject tweenTarget;
	public string tweenName;

	void OnTriggerEnter() {
		iTweenEvent.GetEvent(tweenTarget, tweenName).Play();
	}
}