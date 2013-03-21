using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour {

	public string horizontalInput = "Horizontal";
	public string verticalInput = "Vertical";
	public float moveSpeed;
	public CameraController2D cameraController;
	public float lookahead;
	public float lookaheadEaseTime;

	// Very simple sprite system to show the player character
	public Renderer spriteRenderer;
	public int spriteSheetCellsWide;
	public int spriteSheetCellsHigh;
	public float animationFrameDelay;
	public Vector2[] upFrames;
	public Vector2[] downFrames;
	public Vector2[] leftFrames;
	public Vector2[] rightFrames;
	public bool flipLeftSide;

	CharacterController characterController;
	int currentAnimationFrameIndex;
	float cellWidth;
	float cellHeight;
	float changeToNextFrameAt;
	Vector3 lookaheadChangeVelocity;
	Vector3 currentLookahead;
	bool flippedHorizontally;

	void Start() {
		if(cameraController == null) {
			cameraController = Camera.main.GetComponent<CameraController2D>();
		}
		characterController = GetComponent<CharacterController>();

		cellWidth = 1f / spriteSheetCellsWide;
		cellHeight = 1f / spriteSheetCellsHigh;
	}

	void Update() {
		var inputVector = ((Input.GetAxis(horizontalInput) * Vector3.right) + (Input.GetAxis(verticalInput) * Vector3.forward)).normalized;
		characterController.Move(inputVector * moveSpeed * Time.deltaTime);

		var targetLookahead = inputVector * lookahead;
		currentLookahead.x = Mathf.SmoothDamp(currentLookahead.x, targetLookahead.x, ref lookaheadChangeVelocity.x, lookaheadEaseTime);
		currentLookahead.y = Mathf.SmoothDamp(currentLookahead.y, targetLookahead.y, ref lookaheadChangeVelocity.y, lookaheadEaseTime);
		currentLookahead.z = Mathf.SmoothDamp(currentLookahead.z, targetLookahead.z, ref lookaheadChangeVelocity.z, lookaheadEaseTime);
		cameraController.AddInfluence(currentLookahead);

		if(Time.time >= changeToNextFrameAt && inputVector.sqrMagnitude > 0) {
			changeToNextFrameAt = Time.time + animationFrameDelay;
			currentAnimationFrameIndex++;

			var scale = spriteRenderer.material.mainTextureScale;
			scale.x = Mathf.Abs(scale.x);
			if(flippedHorizontally) scale.x *= -1;
			spriteRenderer.material.mainTextureScale = scale;
		}



		// determine facing
		if(inputVector.x > 0) {
			spriteRenderer.material.mainTextureOffset = ConvertPositionToOffset(rightFrames[currentAnimationFrameIndex % rightFrames.Length]);
			flippedHorizontally = false;
		}
		else if(inputVector.x < 0) {
			spriteRenderer.material.mainTextureOffset = ConvertPositionToOffset(leftFrames[currentAnimationFrameIndex % leftFrames.Length]);
			if(flipLeftSide) flippedHorizontally = true;
		}
		else if(inputVector.z > 0) {
			spriteRenderer.material.mainTextureOffset = ConvertPositionToOffset(upFrames[currentAnimationFrameIndex % upFrames.Length]);
		}
		else if(inputVector.z < 0) {
			spriteRenderer.material.mainTextureOffset = ConvertPositionToOffset(downFrames[currentAnimationFrameIndex % downFrames.Length]);
		}
	}

	// Converts a position in the sprite sheet such as (2, 1) to a texture offset such as (.2222, .75)
	Vector2 ConvertPositionToOffset(Vector2 position) {
		return new Vector2(position.x * cellWidth, 1 - ((position.y + 1) * cellHeight));
	}
}