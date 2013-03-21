using UnityEngine;
using System.Collections;

public class FaceTarget : MonoBehaviour {
	public Transform target;

	void Update () {
		transform.rotation = Quaternion.LookRotation((target.position - transform.position).normalized, Vector3.up);
	}
}
