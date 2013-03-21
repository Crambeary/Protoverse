using UnityEngine;
using System.Collections;

public class EnableObjectsWhenTouched : MonoBehaviour {
	public GameObject[] objects;

	void Start() {
		foreach(var target in objects) {
			target.SetActive(false);
		}
	}

	void OnTriggerEnter() {
		foreach(var target in objects) {
			target.SetActive(true);
		}
	}
}
