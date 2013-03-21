using UnityEngine;
using System.Collections;

public class MovePlayer : MonoBehaviour 
{
	
	public float speed = 0.8f;
	public float angledSpeed = 0.6f;
	public string idleDirection = "Down";
	public bool assaultMode = false;
	public float assaultSpeed = 0.3f;
	public float assaultAngledSpeed = 0.2f;
	
	private tk2dAnimatedSprite anim;
	private bool idle = true;
	private bool walkingDown = false;
	private bool walkingUp = false;
	private bool walkingRight = false;
	private bool walkingLeft = false;
	//private float mousePosX;
	
	// Use this for initialization
	void Start () 
	{
		anim = GetComponent<tk2dAnimatedSprite>();
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(Input.GetKeyDown(KeyCode.Space))
		{
			if(assaultMode)
			{
				assaultMode = false;
			}
			else
			{
				assaultMode = true;
			}
		}
		//mousePosX = Input.mousePosition.x;
		
		//When Till is moving then check what keys are pressed
		if(idle == false)
		{
			if(!assaultMode)
			{
				if(walkingDown == true)
				{
					if(!walkingUp & !walkingRight & !walkingLeft)
					{
						transform.Translate(Vector3.down * speed * Time.deltaTime, Space.World);
					}
					else if (walkingUp & walkingLeft)
					{
						transform.Translate(Vector3.left * speed * Time.deltaTime, Space.World);
					}
					else if (walkingUp & walkingRight)
					{
						transform.Translate (Vector3.right * speed * Time.deltaTime, Space.World);
					}
					if(Input.GetKeyUp(KeyCode.S))
					{
						if(walkingRight){anim.Play ("walkingRight");}
						else if (walkingLeft){anim.Play ("walkingLeft");}
						else if (walkingUp){anim.Play ("walkingUp");}
						if (walkingRight & walkingLeft){anim.Play ("idleDown");}
						walkingDown = false;
						idleDirection = "Down";
						}
					}
													
				if(walkingUp == true)
				{
					if(!walkingLeft & !walkingRight & !walkingDown)
					{
						transform.Translate (Vector3.up * speed * Time.deltaTime, Space.World);
					}
					else if(walkingDown & walkingLeft)
					{
						transform.Translate (Vector3.left * speed * Time.deltaTime, Space.World);
					}
					else if (walkingDown & walkingRight)
					{
						transform.Translate (Vector3.right * speed * Time.deltaTime, Space.World);
					}
					if(Input.GetKeyUp (KeyCode.W))
					{
						if(walkingRight){anim.Play ("walkingRight");}
						else if (walkingLeft){anim.Play ("walkingLeft");}
						else if (walkingDown){anim.Play ("walkingDown");}
						if (walkingRight & walkingLeft){anim.Play ("idleUp");}
						walkingUp = false;
						idleDirection = "Up";
					}
				}
				if(walkingLeft == true)
				{
					if(!walkingUp & !walkingDown & !walkingRight)
					{
						transform.Translate (Vector3.left * speed * Time.deltaTime, Space.World);
					}
					else if(walkingDown & !walkingRight)
					{
						transform.Translate((Vector3.down + Vector3.left) * angledSpeed * Time.deltaTime, Space.World);
					}
					else if(walkingUp & !walkingRight)
					{
						transform.Translate((Vector3.up + Vector3.left) * angledSpeed * Time.deltaTime, Space.World);
					}
					else if(walkingUp & walkingDown)
					{
						transform.Translate (Vector3.left * speed * Time.deltaTime, Space.World);
					}
					else if(walkingRight & walkingUp & walkingDown)
					{
						transform.Translate (Vector3.zero * speed * Time.deltaTime, Space.World);
					}
					else if(walkingUp & walkingDown)
					{
						transform.Translate (Vector3.left * speed * Time.deltaTime, Space.World);
					}
					else if(walkingRight & walkingDown)
					{
						transform.Translate (Vector3.down * speed * Time.deltaTime, Space.World);
					}
					else if(walkingRight & walkingUp)
					{
						transform.Translate (Vector3.up * speed * Time.deltaTime, Space.World);
					}
					if(Input.GetKeyUp (KeyCode.A))
					{
						if(walkingUp){anim.Play ("walkingUp");}
						else if (walkingDown){anim.Play ("walkingDown");}
						else if (walkingRight){anim.Play ("walkingRight");}
						walkingLeft = false;
						idleDirection = "Left";
					}
				}
				if(walkingRight == true)
				{
					if(!walkingLeft & !walkingUp & !walkingDown)
					{
						transform.Translate (Vector3.right * speed * Time.deltaTime, Space.World);
					}
					else if(walkingDown & !walkingLeft)
					{
						transform.Translate((Vector3.down + Vector3.right) * angledSpeed * Time.deltaTime, Space.World);
					}
					else if(walkingUp & !walkingLeft)
					{
						transform.Translate((Vector3.up + Vector3.right) * angledSpeed * Time.deltaTime, Space.World);
					}
					else if(walkingLeft & walkingUp & walkingDown)
					{
						transform.Translate (Vector3.right * speed * Time.deltaTime, Space.World);
					}
					if(Input.GetKeyUp (KeyCode.D))
					{
						if(walkingUp){anim.Play ("walkingUp");}
						else if (walkingDown){anim.Play ("walkingDown");}
						else if (walkingLeft){anim.Play ("walkingLeft");}
						walkingRight = false;
						idleDirection = "Right";
					}
				}
			}
			else
			{
				
				if(walkingDown == true)
				{
					if(!walkingUp & !walkingRight & !walkingLeft)
					{
						transform.Translate(Vector3.down * assaultSpeed * Time.deltaTime, Space.World);
					}
					else if (walkingUp & walkingLeft)
					{
						transform.Translate(Vector3.left * assaultSpeed * Time.deltaTime, Space.World);
					}
					else if (walkingUp & walkingRight)
					{
						transform.Translate (Vector3.right * assaultSpeed * Time.deltaTime, Space.World);
					}
					if(Input.GetKeyUp(KeyCode.S))
					{
						/*
						if(walkingRight){anim.Play ("walkingRight");}
						else if (walkingLeft){anim.Play ("walkingLeft");}
						else if (walkingUp){anim.Play ("walkingUp");}
						if (walkingRight & walkingLeft){anim.Play ("idleDown");}
						*/
						walkingDown = false;
						idleDirection = "Down";
						}
					}
													
				if(walkingUp == true)
				{
					if(!walkingLeft & !walkingRight & !walkingDown)
					{
						transform.Translate (Vector3.up * assaultSpeed * Time.deltaTime, Space.World);
					}
					else if(walkingDown & walkingLeft)
					{
						transform.Translate (Vector3.left * assaultSpeed * Time.deltaTime, Space.World);
					}
					else if (walkingDown & walkingRight)
					{
						transform.Translate (Vector3.right * assaultSpeed * Time.deltaTime, Space.World);
					}
					if(Input.GetKeyUp (KeyCode.W))
					{
						/*
						if(walkingRight){anim.Play ("walkingRight");}
						else if (walkingLeft){anim.Play ("walkingLeft");}
						else if (walkingDown){anim.Play ("walkingDown");}
						if (walkingRight & walkingLeft){anim.Play ("idleUp");}
						*/
						walkingUp = false;
						idleDirection = "Up";
					}
				}
				if(walkingLeft == true)
				{
					if(!walkingUp & !walkingDown & !walkingRight)
					{
						transform.Translate (Vector3.left * assaultSpeed * Time.deltaTime, Space.World);
					}
					else if(walkingDown & !walkingRight)
					{
						transform.Translate((Vector3.down + Vector3.left) * assaultAngledSpeed * Time.deltaTime, Space.World);
					}
					else if(walkingUp & !walkingRight)
					{
						transform.Translate((Vector3.up + Vector3.left) * assaultAngledSpeed * Time.deltaTime, Space.World);
					}
					else if(walkingUp & walkingDown)
					{
						transform.Translate (Vector3.left * assaultSpeed * Time.deltaTime, Space.World);
					}
					else if(walkingRight & walkingUp & walkingDown)
					{
						transform.Translate (Vector3.zero * assaultSpeed * Time.deltaTime, Space.World);
					}
					else if(walkingUp & walkingDown)
					{
						transform.Translate (Vector3.left * assaultSpeed * Time.deltaTime, Space.World);
					}
					else if(walkingRight & walkingDown)
					{
						transform.Translate (Vector3.down * assaultSpeed * Time.deltaTime, Space.World);
					}
					else if(walkingRight & walkingUp)
					{
						transform.Translate (Vector3.up * assaultSpeed * Time.deltaTime, Space.World);
					}
					if(Input.GetKeyUp (KeyCode.A))
					{
						/*
						if(walkingUp){anim.Play ("walkingUp");}
						else if (walkingDown){anim.Play ("walkingDown");}
						else if (walkingRight){anim.Play ("walkingRight");}
						*/
						walkingLeft = false;
						idleDirection = "Left";
					}
				}
				if(walkingRight == true)
				{
					if(!walkingLeft & !walkingUp & !walkingDown)
					{
						transform.Translate (Vector3.right * assaultSpeed * Time.deltaTime, Space.World);
					}
					else if(walkingDown & !walkingLeft)
					{
						transform.Translate((Vector3.down + Vector3.right) * assaultAngledSpeed * Time.deltaTime, Space.World);
					}
					else if(walkingUp & !walkingLeft)
					{
						transform.Translate((Vector3.up + Vector3.right) * assaultAngledSpeed * Time.deltaTime, Space.World);
					}
					else if(walkingLeft & walkingUp & walkingDown)
					{
						transform.Translate (Vector3.right * assaultSpeed * Time.deltaTime, Space.World);
					}
					if(Input.GetKeyUp (KeyCode.D))
					{
						/*
						if(walkingUp){anim.Play ("walkingUp");}
						else if (walkingDown){anim.Play ("walkingDown");}
						else if (walkingLeft){anim.Play ("walkingLeft");}
						*/
						walkingRight = false;
						idleDirection = "Right";
					}
				}
			}
			if(!walkingUp & !walkingDown & !walkingLeft & !walkingRight){idle = true;transform.Translate (Vector3.zero);}
		}
		//When Till is not moving then check where he was looking
		if (idle == true)
		{
			if(idleDirection == "Down")
			{anim.Play ("idleDown");}
			if(idleDirection == "Up")
			{anim.Play ("idleUp");}
			if(idleDirection == "Left")
			{anim.Play ("idleLeft");}
			if(idleDirection == "Right")
			{anim.Play ("idleRight");}
		}
		
		if(Input.GetKeyDown(KeyCode.S))
		{
			if(!assaultMode)
			{
				if(!walkingUp & !walkingRight & !walkingLeft){anim.Play("walkingDown");}
				else if(walkingUp){anim.Play ("idleDown");}
				else if(walkingLeft & walkingRight){anim.Play ("walkingDown");}
			}
			walkingDown = true;
			idle = false;
		}
		if(Input.GetKeyDown(KeyCode.W))
		{
			if(!assaultMode)
			{
				if(!walkingDown & !walkingLeft & !walkingRight){anim.Play ("walkingUp");}
				else if(walkingDown){anim.Play ("idleUp");}
				else if(walkingLeft & walkingRight){anim.Play ("walkingUp");}
			}
			walkingUp = true;
			idle = false;
		}
		if(Input.GetKeyDown(KeyCode.A))
		{
			if(!assaultMode)
			{
				if(!walkingUp & !walkingRight & !walkingDown){anim.Play ("walkingLeft");}
				else if(walkingRight){anim.Play ("idleRight");}
				if(walkingDown & walkingRight){anim.Play("walkingDown");}
				if(walkingUp & walkingRight){anim.Play ("walkingUp");}
			}
			walkingLeft = true;
			idle = false;
		}
		if(Input.GetKeyDown(KeyCode.D))
		{
			if(!assaultMode)
			{
				if(!walkingUp & !walkingDown & !walkingLeft){anim.Play ("walkingRight");}
				else if (walkingLeft){anim.Play ("idleLeft");}
				if (walkingLeft & walkingDown){anim.Play ("walkingDown");}
				else if (walkingLeft & walkingUp){anim.Play ("walkingUp");}
			}
			walkingRight = true;
			idle = false;
		}
	}
	void OnCollisionEnter(Collision collision)
	{
		Debug.Log(collision.relativeVelocity.magnitude);
	}
}