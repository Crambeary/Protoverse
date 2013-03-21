using UnityEngine;
using System.Collections;

public class ChangeComponentActiveWhenTouched : MonoBehaviour {
	public MonoBehaviour component;
	public bool setEnabledWhenTouched;

	void OnTriggerEnter() {
		component.enabled = setEnabledWhenTouched;
	}
}