using System;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(PlayerInput))]
public class CharacterMotor : MonoBehaviour
{
	[Tooltip("Move speed of the character in m/s")]
	public float MoveSpeed = 5.335f;
	public float AnimationVelocityInterp = 0.7f;

	[Tooltip("How fast the character turns to face movement direction")] [Range(0.0f, 0.3f)]
	public float RotationSmoothTime = 0.12f;

	[Tooltip("The character uses its own gravity value. The engine default is -9.81f")]
	public float Gravity = -15.0f;

	[Header("Player Grounded")]
	[Tooltip("If the character is grounded or not. Not part of the CharacterController built in grounded check")]
	public bool Grounded = true;

	[Tooltip("Useful for rough ground")] public float GroundedOffset = -0.14f;

	[Tooltip("The radius of the grounded check. Should match the radius of the CharacterController")]
	public float GroundedRadius = 0.28f;

	[Tooltip("What layers the character uses as ground")]
	public LayerMask GroundLayers;

	// player
	private Vector3 _velocity;
	private float _targetRotation;
	private float _rotationVelocity;
	private float _verticalVelocity;
	private float _terminalVelocity = 53.0f;

	// animation IDs
	private readonly int _animIDSpeedX = Animator.StringToHash("SpeedX");
	private readonly int _animIDSpeedY = Animator.StringToHash("SpeedY");
	private readonly int _animIDFreeFall = Animator.StringToHash("FreeFall");
	private readonly int _animIDMotionSpeed = Animator.StringToHash("MotionSpeed");
	private readonly int _animIDMeleeAttack = Animator.StringToHash("MeleeAttack");
	private readonly int _animIDStateTime = Animator.StringToHash("StateTime");

	private Animator _animator;
	private Transform _transform;
	private CharacterController _controller;
	private PlayerInput _input;
	private Vector3 _animatorVelocity;

	private void Awake()
	{
		_animator = GetComponent<Animator>();
		_input = GetComponent<PlayerInput>();
		_controller = GetComponent<CharacterController>();
		_transform = transform;
	}

	private void Start()
	{
	}

	private void Update()
	{
		GroundedCheck();
	}

	private void GroundedCheck()
	{
		// set sphere position, with offset
		var transformPosition = _transform.position;
		Vector3 spherePosition =
			new Vector3(transformPosition.x, transformPosition.y - GroundedOffset, transformPosition.z);
		Grounded = Physics.CheckSphere(spherePosition, GroundedRadius, GroundLayers, QueryTriggerInteraction.Ignore);
	}

	private void Move()
	{
		var targetVelocity = new Vector3(_input.MoveInput.x, 0, _input.MoveInput.y).normalized * MoveSpeed;
		_velocity = targetVelocity;

		// update animator if using character
		var newAnimatorVelocity = Quaternion.Inverse(_transform.rotation) * _velocity / MoveSpeed;
		_animatorVelocity = Vector3.Lerp(newAnimatorVelocity, _animatorVelocity, AnimationVelocityInterp);
		_animator.SetFloat(_animIDSpeedX, _animatorVelocity.x);
		_animator.SetFloat(_animIDSpeedY, _animatorVelocity.z);
		// _animator.SetFloat(_animIDMotionSpeed, inputMagnitude);
	}

	void OnAnimatorMove()
	{
		Vector3 movement;

		if (Grounded)
		{
			var ray = new Ray(transform.position + Vector3.up * GroundedRadius * 0.5f, -Vector3.up);
			if (Physics.Raycast(ray, out var hit, GroundedRadius, Physics.AllLayers, QueryTriggerInteraction.Ignore))
			{
				movement = Vector3.ProjectOnPlane(_animator.deltaPosition, hit.normal);
			}
			else
			{
				movement = _animator.deltaPosition;
			}
		}
		else
		{
			// movement = MoveSpeed * transform.forward * Time.deltaTime;
			movement = Vector3.zero;
		}

		_controller.transform.rotation *= _animator.deltaRotation;
		var useRootMotion = !Grounded;
		
		if(useRootMotion)
			_controller.Move(movement);
		else
			_controller.SimpleMove(_velocity);

		_animator.SetBool(_animIDFreeFall, !Grounded);
	}

	private void FixedUpdate()
	{
		_animator.SetFloat(_animIDStateTime, Mathf.Repeat(_animator.GetCurrentAnimatorStateInfo(0).normalizedTime, 1f));
		_animator.ResetTrigger(_animIDMeleeAttack);

		if (_input.Attack)
			_animator.SetTrigger(_animIDMeleeAttack);
		
		Move();
	}
}