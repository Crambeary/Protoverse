using UnityEngine;
using System.Collections;

public class TestMovement : MonoBehaviour {
	
	public GameObject player;
	
	private bool walkingRight = false;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
		if(Input.GetKeyDown(KeyCode.D))
		{
			walkingRight = true;
		}
		if (walkingRight == true)
		{
			gameObject.rigidbody.velocity = new Vector3(1.0f,0.0f,0.0f);
		}
		if(Input.GetKey(KeyCode.A))
		{
			gameObject.rigidbody.velocity = new Vector3(-1.0f,0.0f,0.0f);
		}
		if(Input.GetKey(KeyCode.W))
		{
			gameObject.rigidbody.velocity = new Vector3(0.0f,1.0f,0.0f);
		}
		if(Input.GetKey(KeyCode.S))
		{
			gameObject.rigidbody.velocity = new Vector3(0.0f,-1.0f,0.0f);
		}
		if(Input.GetKeyUp(KeyCode.A))
		{
			gameObject.rigidbody.velocity = new Vector3(0.0f,0.0f,0.0f);
		}
		if(Input.GetKeyUp(KeyCode.W))
		{
			gameObject.rigidbody.velocity = new Vector3(0.0f,0.0f,0.0f);
		}
		if(Input.GetKeyUp(KeyCode.S))
		{
			gameObject.rigidbody.velocity = new Vector3(0.0f,0.0f,0.0f);
		}
		if(Input.GetKeyUp(KeyCode.D))
		{
			gameObject.rigidbody.velocity = new Vector3(0.0f,0.0f,0.0f);
			walkingRight = false;
		}
	}
	
	void OnCollisionEnter()
	{
		
	}
}
