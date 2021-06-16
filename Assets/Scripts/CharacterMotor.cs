using System;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterMotor))]
[RequireComponent(typeof(PlayerInput))]
public class CharacterMotor : MonoBehaviour
{
	[Tooltip("Move speed of the character in m/s")]
	public float MoveSpeed = 5.335f;

	[Tooltip("How fast the character turns to face movement direction")] [Range(0.0f, 0.3f)]
	public float RotationSmoothTime = 0.12f;

	[Tooltip("Acceleration and deceleration")]
	public float SpeedChangeRate = 10.0f;

	[Space(10)] [Tooltip("The height the player can jump")]
	public float JumpHeight = 1.2f;

	[Tooltip("The character uses its own gravity value. The engine default is -9.81f")]
	public float Gravity = -15.0f;

	[Space(10)] [Tooltip("Time required to pass before being able to jump again. Set to 0f to instantly jump again")]
	public float JumpTimeout = 0.50f;

	[Tooltip("Time required to pass before entering the fall state. Useful for walking down stairs")]
	public float FallTimeout = 0.15f;

	[Header("Player Grounded")]
	[Tooltip("If the character is grounded or not. Not part of the CharacterController built in grounded check")]
	public bool Grounded = true;

	[Tooltip("Useful for rough ground")] public float GroundedOffset = -0.14f;

	[Tooltip("The radius of the grounded check. Should match the radius of the CharacterController")]
	public float GroundedRadius = 0.28f;

	[Tooltip("What layers the character uses as ground")]
	public LayerMask GroundLayers;

	[NonSerialized] public Vector2 MoveInput;
	[NonSerialized] public Vector3 LookDir;
	[NonSerialized] public bool JumpInput;

	// player
	private float _speed;
	private float _animationBlend;
	private float _targetRotation;
	private float _rotationVelocity;
	private float _verticalVelocity;
	private float _terminalVelocity = 53.0f;

	// timeout deltatime
	private float _jumpTimeoutDelta;
	private float _fallTimeoutDelta;

	// animation IDs
	private readonly int _animIDSpeed = Animator.StringToHash("Speed");
	private readonly int _animIDGrounded = Animator.StringToHash("Grounded");
	private readonly int _animIDJump = Animator.StringToHash("Jump");
	private readonly int _animIDFreeFall = Animator.StringToHash("FreeFall");
	private readonly int _animIDMotionSpeed = Animator.StringToHash("MotionSpeed");
	
	private Animator _animator;
	private Transform _transform;
	private CharacterController _controller;
	private bool _hasAnimator;
	
	private void Awake()
	{
		_hasAnimator = TryGetComponent(out _animator);
		_controller = GetComponent<CharacterController>();
		_transform = transform;
	}

	private void Start()
	{
		// reset our timeouts on start
		_jumpTimeoutDelta = JumpTimeout;
		_fallTimeoutDelta = FallTimeout;
	}
	
	private void Update()
	{
		JumpAndGravity();
		GroundedCheck();
		Move();
		Look();
	}

	private void GroundedCheck()
	{
		// set sphere position, with offset
		var transformPosition = _transform.position;
		Vector3 spherePosition = new Vector3(transformPosition.x, transformPosition.y - GroundedOffset, transformPosition.z);
		Grounded = Physics.CheckSphere(spherePosition, GroundedRadius, GroundLayers, QueryTriggerInteraction.Ignore);

		// update animator if using character
		if (_hasAnimator)
		{
			_animator.SetBool(_animIDGrounded, Grounded);
		}
	}
	
	private void Move()
		{
			// set target speed based on move speed, sprint speed and if sprint is pressed
			float targetSpeed = MoveSpeed;

			// a simplistic acceleration and deceleration designed to be easy to remove, replace, or iterate upon

			// note: Vector2's == operator uses approximation so is not floating point error prone, and is cheaper than magnitude
			// if there is no input, set the target speed to 0
			var isZeroMovementInput = MoveInput == Vector2.zero;
			if (isZeroMovementInput) targetSpeed = 0.0f;

			// a reference to the players current horizontal velocity
			var controllerVelocity = _controller.velocity;
			float currentHorizontalSpeed = new Vector3(controllerVelocity.x, 0.0f, controllerVelocity.z).magnitude;

			float speedOffset = 0.1f;
			float inputMagnitude = Mathf.Min(MoveInput.magnitude, 1);

			// accelerate or decelerate to target speed
			if (currentHorizontalSpeed < targetSpeed - speedOffset || currentHorizontalSpeed > targetSpeed + speedOffset)
			{
				// creates curved result rather than a linear one giving a more organic speed change
				// note T in Lerp is clamped, so we don't need to clamp our speed
				_speed = Mathf.Lerp(currentHorizontalSpeed, targetSpeed * inputMagnitude, Time.deltaTime * SpeedChangeRate);

				// round speed to 3 decimal places
				_speed = Mathf.Round(_speed * 1000f) / 1000f;
			}
			else
			{
				_speed = targetSpeed;
			}
			_animationBlend = Mathf.Lerp(_animationBlend, targetSpeed, Time.deltaTime * SpeedChangeRate);

			Vector3 targetDirection = new Vector3(MoveInput.x, 0.0f, MoveInput.y).normalized;

			// move the player
			_controller.Move(targetDirection.normalized * (_speed * Time.deltaTime) + new Vector3(0.0f, _verticalVelocity, 0.0f) * Time.deltaTime);

			// update animator if using character
			if (_hasAnimator)
			{
				_animator.SetFloat(_animIDSpeed, _animationBlend);
				_animator.SetFloat(_animIDMotionSpeed, inputMagnitude);
			}
		}

		private void JumpAndGravity()
		{
			if (Grounded)
			{
				// reset the fall timeout timer
				_fallTimeoutDelta = FallTimeout;

				// update animator if using character
				if (_hasAnimator)
				{
					_animator.SetBool(_animIDJump, false);
					_animator.SetBool(_animIDFreeFall, false);
				}

				// stop our velocity dropping infinitely when grounded
				if (_verticalVelocity < 0.0f)
				{
					_verticalVelocity = -2f;
				}

				// Jump
				if (JumpInput && _jumpTimeoutDelta <= 0.0f)
				{
					// the square root of H * -2 * G = how much velocity needed to reach desired height
					_verticalVelocity = Mathf.Sqrt(JumpHeight * -2f * Gravity);

					// update animator if using character
					if (_hasAnimator)
					{
						_animator.SetBool(_animIDJump, true);
					}
				}

				// jump timeout
				if (_jumpTimeoutDelta >= 0.0f)
				{
					_jumpTimeoutDelta -= Time.deltaTime;
				}
			}
			else
			{
				// reset the jump timeout timer
				_jumpTimeoutDelta = JumpTimeout;

				// fall timeout
				if (_fallTimeoutDelta >= 0.0f)
				{
					_fallTimeoutDelta -= Time.deltaTime;
				}
				else
				{
					// update animator if using character
					if (_hasAnimator)
					{
						_animator.SetBool(_animIDFreeFall, true);
					}
				}

				// if we are not grounded, do not jump
				JumpInput = false;
			}

			// apply gravity over time if under terminal (multiply by delta time twice to linearly speed up over time)
			if (_verticalVelocity < _terminalVelocity)
			{
				_verticalVelocity += Gravity * Time.deltaTime;
			}
		}
		
		private void Look()
		{
			// _targetRotation = Mathf.Atan2(LookDir.x, LookDir.z) * Mathf.Rad2Deg;
			// float resultRotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, _targetRotation, ref _rotationVelocity, RotationSmoothTime);
			// transform.rotation = Quaternion.Euler(0, resultRotation, 0); 
		}
}