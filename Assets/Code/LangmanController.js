// This is a modified version of the PlatformerController from 
// the 2D Lerpz tutorial (http://unity3d.com/support/resources/tutorials/2d-gameplay-tutorial.html).
// Primary changes:
// * In-air movement is calculated more like running movement. When you release the arrow keys,
//   horizontal movement gradually slows to a stop. The old logic was suitable for a jetpack,
//   but not a traditional platformer.
// * EDIT: The jumping logic now allows jumps slightly after the character leaves a ledge.
// * I've stripped out a bunch of things I didn't use, such as having different walk vs. run speeds,
//   the particle effect stuff, and more.
//
// Bug fixes:
// * Respawning from a moving platform sometimes caused the player to be affected by the
//   platform's position in the respawn frame. To remedy this, activePlatform is set to null in Spawn.
// * When standing on a platform, the character could be pushed through walls and other obstacles. To
//   remedy this, the effect of a platform's movement on the character is not directly applied to the transform.
//   Instead, it is combined with the other movement input and passed into CharacterController.Move, which 
//   handles collision detection.
// * EDIT: It turns out that for moving platforms, transform.position must be updated directly.
//   If this isn't done, the character will eventually fall through the platform.
//   The code now modifies transform.position directly when the character is standing on a
//   kinematic rigidbody that has a non-zero velocity.
// * If the character brushed a block diagonally when jumping, he could end up floating slowly upward.
//   This unintentional flying ability was remedied by detecting ceiling collision as part of determining
//   whether the apex of the jump has been reached.
// * In certain cases, when jumping while standing on a falling block, the character could end up 
//   jumping/landing without ever leaving the block. This was fixed by getting rid of inAirVelocity 
//   and not making the jump velocity dependent on the velocity of the active platform.
//
// Changes by Ehren von Lehe. Feel free to use this code in your own projects.
// http://vonlehecreative.com
#pragma strict

// Does this script currently respond to Input?
var canControl = true;

// The character will spawn at spawnPoint's position when needed.  This could be changed via a script at runtime to implement, e.g. waypoints/savepoints.
var spawnPoint : Transform;

class LangmanControllerMovement {
	// The speed when running 
	var runSpeed = 1.0;

	// The speed when sliding up and around corners 
	var slideFactor = 0.05;
	@System.NonSerialized
	var slideX = 0.0;
	
	// The gravity for the character
	var gravity = 0.0;
	var maxFallSpeed = 0.0;

	// How fast does the character change speeds?  Higher is faster.
	var speedSmoothing = 1.0;

	// This controls how fast the graphics of the character "turn around" when the player turns around using the controls.
	var rotationSmoothing = 10.0;

	// The current move direction in x-y.  This will always been (1,0,0) or (-1,0,0)
	// The next line, @System.NonSerialized , tells Unity to not serialize the variable or show it in the inspector view.  Very handy for organization!
	@System.NonSerialized
	var direction = Vector3.zero;

	// The current vertical speed
	@System.NonSerialized
	var verticalSpeed = 0.0;

	// The current movement speed.  This gets smoothed by speedSmoothing.
	@System.NonSerialized
	var speed = 0.0;

	// Is the user pressing the left or right movement keys?
	@System.NonSerialized
	var isMoving = false;

	// The last collision flags returned from controller.Move
	@System.NonSerialized
	var collisionFlags : CollisionFlags; 

	// We will keep track of an approximation of the character's current velocity, so that we return it from GetVelocity () for our camera to use for prediction.
	@System.NonSerialized
	var velocity : Vector3;
	
	// This will keep track of how long we have we been in the air (not grounded)
	@System.NonSerialized
	var hangTime = 0.0;
}

var movement : LangmanControllerMovement;

// We will contain all the jumping related variables in one helper class for clarity.
class LangmanControllerJumping {
	// Can the character jump?
	var enabled = true;

	// How high do we jump when pressing jump and letting go immediately
	var height = 0.5;
	// We add extraHeight units (meters) on top when holding the button down longer while jumping
	var extraHeight = 1.6;
	
	// How fast does the character change speeds?  Higher is faster.
	var speedSmoothing = 1.0;

	// How fast does the character move horizontally when in the air.
	var jumpSpeed = 1.0;
	
	// This prevents inordinarily too quick jumping
	// The next line, @System.NonSerialized , tells Unity to not serialize the variable or show it in the inspector view.  Very handy for organization!
	@System.NonSerialized
	var repeatTime = 0.05;

	@System.NonSerialized
	var timeout = 0.15;

	// Are we jumping? (Initiated with jump button and not grounded yet)
	@System.NonSerialized
	var jumping = false;
	
	@System.NonSerialized
	var reachedApex = false;
  
	// Last time the jump button was clicked down
	@System.NonSerialized
	var lastButtonTime = -10.0;
	
	// Last time we were grounded
	@System.NonSerialized
	var lastGroundedTime = -10.0;

	@System.NonSerialized
	var groundingTimeout = 0.1;

	// Last time we performed a jump
	@System.NonSerialized
	var lastTime = -1.0;

	// the height we jumped from (Used to determine for how long to apply extra jump power after jumping.)
	@System.NonSerialized
	var lastStartHeight = 0.0;

	@System.NonSerialized
	var touchedCeiling = false;

	@System.NonSerialized
	var buttonReleased = true;
}

var jump : LangmanControllerJumping;

private var controller : CharacterController;

// Moving platform support.
private var activePlatform : Transform;
private var activeLocalPlatformPoint : Vector3;
private var activeGlobalPlatformPoint : Vector3;
private var lastPlatformVelocity : Vector3;

private var sprite : GameObject;

function Awake () {
	movement.direction = transform.TransformDirection (Vector3.forward);
	controller = GetComponent (CharacterController) as CharacterController;
	sprite = transform.Find("Sprite").gameObject;
}

function Spawn () {
	// reset the character's speed
	movement.verticalSpeed = 0.0;
	movement.speed = 0.0;
	
	// make sure we're not attached to a platform
	activePlatform = null;
	
	// reset the character's position to the spawnPoint
	transform.position = spawnPoint.position;
	
	sprite.active = true;
	canControl = true;
}

function Unspawn() {
	canControl = false;
	sprite.active = false;
}

function UpdateSmoothedMovementDirection () {	
	var h = Input.GetAxisRaw ("Horizontal");
	
	if (!canControl)
		h = 0.0;
	
	movement.isMoving = Mathf.Abs (h) > 0.1;
	
	if (movement.isMoving){
		// run
		movement.direction = Vector3 (h, 0, 0);
	}
	
	// Smooth the speed based on the current target direction
	var curSmooth = 0.0;
	// Choose target speed
	var targetSpeed = Mathf.Min (Mathf.Abs(h), 1.0);

	if(controller.isGrounded){
		curSmooth = movement.speedSmoothing * Time.smoothDeltaTime;
		targetSpeed *= movement.runSpeed;
		movement.hangTime = 0.0;
	}else{
		curSmooth = jump.speedSmoothing * Time.smoothDeltaTime;
		targetSpeed *= jump.jumpSpeed;
		movement.hangTime += Time.smoothDeltaTime;
	}
	
	movement.speed = Mathf.Lerp (movement.speed, targetSpeed, curSmooth);
}

function AnimateCharacter() {
	// For an example of animating a sprite sheet, see:
	// http://www.unifycommunity.com/wiki/index.php?title=Animating_Tiled_texture
	if (movement.isMoving){
		// run
	}else if(controller.isGrounded){
		// stand
	}
}

function JustBecameUngrounded() {
	return (Time.time < (jump.lastGroundedTime + jump.groundingTimeout) && jump.lastGroundedTime > jump.lastTime);
}

function ApplyJumping () {
	if (Input.GetButtonDown ("Jump") && canControl) {
		jump.lastButtonTime = Time.time;
	}

	// Prevent jumping too fast after each other
	if (jump.lastTime + jump.repeatTime > Time.time){
		return;
	}

	var isGrounded = controller.isGrounded;
	
	// Allow jumping slightly after the character leaves a ledge,
	// as long as a jump hasn't occurred since we became ungrounded.
	if (isGrounded || JustBecameUngrounded()) {
		if(isGrounded){
			jump.lastGroundedTime = Time.time;
		}
		
		// Jump
		// - Only when pressing the button down
		// - With a timeout so you can press the button slightly before landing		
		if (jump.enabled && Time.time < jump.lastButtonTime + jump.timeout) {
			movement.verticalSpeed = CalculateJumpVerticalSpeed (jump.height);
			// If we're on a platform, add the platform's velocity (times 1.4)
			// to the character's velocity. We only do this if the platform
			// is traveling upward.
			if(activePlatform){
				var apRb = activePlatform.rigidbody;
				if(apRb){
					var apRbY = activePlatform.rigidbody.velocity.y;
					if(apRbY > 0.0){
						apRbY *= 1.4;
						movement.verticalSpeed += apRbY;
					}
				}
			}
			SendMessage ("DidJump", SendMessageOptions.DontRequireReceiver);
		}
	}
}

function ApplyGravity () {
	// Apply gravity
	var jumpButton = Input.GetButton ("Jump");
	
	if (!canControl)
		jumpButton = false;
		
	// When we reach the apex of the jump we send out a message
	if (jump.jumping && !jump.reachedApex && movement.verticalSpeed <= 0.0) {
		jump.reachedApex = true;
		SendMessage ("DidJumpReachApex", SendMessageOptions.DontRequireReceiver);
	}
	
	// * When jumping up we don't apply gravity for some time when the user is holding the jump button
	//   This gives more control over jump height by pressing the button longer
	if (!jump.touchedCeiling && IsTouchingCeiling()){
		jump.touchedCeiling = true; // store this so we don't allow extra power jump to continue after character hits ceiling.
	}
	if (!jumpButton){
		jump.buttonReleased = true;
	}
	
	var extraPowerJump = jump.jumping && movement.verticalSpeed > 0.0 && jumpButton && !jump.buttonReleased && transform.position.y < jump.lastStartHeight + jump.extraHeight && !jump.touchedCeiling;
	
	if (extraPowerJump){
		return;
	}else if (controller.isGrounded){
		movement.verticalSpeed = -movement.gravity * Time.smoothDeltaTime;
	}else{
		movement.verticalSpeed -= movement.gravity * Time.smoothDeltaTime;
	}
		
	// Make sure we don't fall any faster than maxFallSpeed.  This gives our character a terminal velocity.
	movement.verticalSpeed = Mathf.Max (movement.verticalSpeed, -movement.maxFallSpeed);
}

function CalculateJumpVerticalSpeed (targetJumpHeight : float) {
	// From the jump height and gravity we deduce the upwards speed 
	// for the character to reach at the apex.
	return Mathf.Sqrt (2 * targetJumpHeight * movement.gravity);
}

function DidJump () {
	jump.jumping = true;
	jump.reachedApex = false;
	jump.lastTime = Time.time;
	jump.lastStartHeight = transform.position.y;
	jump.lastButtonTime = -10;
	jump.touchedCeiling = false;
	jump.buttonReleased = false;
}

function Update () {
	// Make sure we are always in the 2D plane.
	transform.position.z = -0.1;

	UpdateSmoothedMovementDirection();
	
	AnimateCharacter();		

	// Apply gravity
	// - extra power jump modifies gravity
	ApplyGravity ();

	// Apply jumping logic
	ApplyJumping ();
	
	var platformMovementOffset = Vector3.zero;
	
	// Moving platform support
	if (activePlatform != null && !jump.jumping) {
		var newGlobalPlatformPoint = activePlatform.TransformPoint(activeLocalPlatformPoint);
		var moveDistance = (newGlobalPlatformPoint - activeGlobalPlatformPoint);
		// Setting transform.position directly causes us to go through walls if we're on a rotating block.
		// But it's necessary to make moving platforms work.
		if(activePlatform.rigidbody.isKinematic && activePlatform.rigidbody.velocity.sqrMagnitude > 0.0){
			// Moving platform. Change the position directly so the character
			// won't fall through the platform.
			transform.position = transform.position + moveDistance;
		}else{
			// Store the desired movement for use in CharacterController.Move.
			platformMovementOffset = moveDistance;
		}
		lastPlatformVelocity = (newGlobalPlatformPoint - activeGlobalPlatformPoint) / Time.smoothDeltaTime;
	} else {
		lastPlatformVelocity = Vector3.zero;	
	}
	
	activePlatform = null;
	
	// Save lastPosition for velocity calculation.
	var lastPosition = transform.position;
	
	// Calculate actual motion
	var currentMovementOffset = (movement.direction * movement.speed) + Vector3 (0.0, movement.verticalSpeed, 0.0);
	
	// We always want the movement to be framerate independent.  Multiplying by Time.smoothDeltaTime does this.
	currentMovementOffset *= Time.smoothDeltaTime;
	currentMovementOffset += platformMovementOffset;
	currentMovementOffset.x += movement.slideX * movement.slideFactor;
	// Reset sliding to zero. It will be set in controller.Move
	movement.slideX = 0.0;
	
   	// Move our character!
   	// We can get null refs here
   	movement.collisionFlags = controller.Move (currentMovementOffset);
	
	// Calculate the velocity based on the current and previous position.  
	// This means our velocity will only be the amount the character actually moved as a result of collisions.
	movement.velocity = (transform.position - lastPosition) / Time.smoothDeltaTime;
	
	// Moving platforms support
	if (activePlatform != null) {
		activeGlobalPlatformPoint = transform.position;
		activeLocalPlatformPoint = activePlatform.InverseTransformPoint (transform.position);
	}
	
	// Set rotation to the move direction	
	if (movement.direction.sqrMagnitude > 0.01)
		transform.rotation = Quaternion.Slerp (transform.rotation, Quaternion.LookRotation (movement.direction), Time.smoothDeltaTime * movement.rotationSmoothing);
	
	// We are in jump mode but just became grounded
	if (controller.isGrounded) {
		if (jump.jumping) {
			jump.jumping = false;
			SendMessage ("DidLand", SendMessageOptions.DontRequireReceiver);

			var jumpMoveDirection = movement.direction * movement.speed;
			if (jumpMoveDirection.sqrMagnitude > 0.01)
				movement.direction = jumpMoveDirection.normalized;
		}
	}
}

function OnControllerColliderHit (hit : ControllerColliderHit)
{
	// Make sure we are really standing on a straight platform
	// Not on the underside of one and not falling down from it either!
	if (hit.moveDirection.y < -0.9 && hit.normal.y > 0.9 
		&& hit.rigidbody 
		&& (!hit.rigidbody.isKinematic || hit.rigidbody.velocity.sqrMagnitude > 0.0)) {
		activePlatform = hit.collider.transform;
	}else if (jump.jumping && hit.moveDirection.y > 0.0 && hit.normal.y < 0.0 && Mathf.Abs(hit.normal.x) > 0.01){
		movement.slideX = hit.normal.x;
	}
}

// Various helper functions below:
function GetSpeed () {
	return movement.speed;
}

function GetVelocity () {
	return movement.velocity;
}


function IsMoving () {
	return movement.isMoving;
}

function IsJumping () {
	return jump.jumping;
}

function IsTouchingCeiling () {
	return (movement.collisionFlags & CollisionFlags.CollidedAbove) != 0;
}

function GetDirection () {
	return movement.direction;
}

function GetHangTime() {
	return movement.hangTime;
}

function Reset () {
	gameObject.tag = "Player";
}

function SetControllable (controllable : boolean) {
	canControl = controllable;
}

// Require a character controller to be attached to the same game object
@script RequireComponent (CharacterController)
@script AddComponentMenu ("2D Platformer/Langman Controller")
