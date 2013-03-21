using UnityEngine;
using System.Collections;

public class ChangeCursor : MonoBehaviour 
{
	//public Texture2D defaultCursor;
	public Texture2D cursorImage;
	
	private int cursorSizeX = 32;
	private int cursorSizeY = 32;
	private bool assaultMode = false;
	
	// Use this for initialization
	void Start () 
	{
		//Screen.showCursor = false;
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(Input.GetKeyDown(KeyCode.Space))
		{
			if(assaultMode)
			{
				assaultMode = false;
				Screen.showCursor = true;
			}
			else
			{
				assaultMode = true;
				Screen.showCursor = false;
			}
		}
	}
	void OnGUI ()
	{
		if(assaultMode)
		{
			GUI.DrawTexture(new Rect(Input.mousePosition.x, Screen.height - Input.mousePosition.y, cursorSizeX, cursorSizeY), cursorImage);
		}
		else
		{
			//GUI.DrawTexture(new Rect(Input.mousePosition.x, Screen.height - Input.mousePosition.y, cursorSizeX, cursorSizeY), defaultCursor);
		}
	}
}
