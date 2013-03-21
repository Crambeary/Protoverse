using UnityEngine;
using System.Collections;

public class DestroyObjectsWhenTouched : MonoBehaviour {
	public GameObject[] objectsToDestroy;

	void OnTriggerEnter() {
		foreach(var target in objectsToDestroy) {
			Destroy(target);
		}
	}
}