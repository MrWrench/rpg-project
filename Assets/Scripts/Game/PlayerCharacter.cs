using System.Collections.Generic;
using Game.Input;
using UnityEngine;
using UnityEngine.VFX;

namespace Game
{
    [RequireComponent(typeof(CharacterInput))]
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(CharacterController))]
    public class PlayerCharacter : Character
    {
        [Tooltip("Move speed of the character in m/s")]
        public float MoveSpeed = 5.335f;
        public float MoveAcceleration = 20f;

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

        [SerializeField] private List<VisualEffect> _attackTrails = new();
        [SerializeField] private CharacterWeapon _weapon;

        // player
        private Vector3 _velocity;
        private Quaternion _targetRotation;
        private float _rotationVelocity;
        private float _verticalVelocity;
        private float _terminalVelocity = -53.0f;
        private bool _useRootMotion;

        // animation IDs
        private readonly int _animIDSpeedX = Animator.StringToHash("SpeedX");
        private readonly int _animIDSpeedY = Animator.StringToHash("SpeedY");
        private readonly int _animIDGrounded = Animator.StringToHash("Grounded");
        private readonly int _animIDMeleeAttack = Animator.StringToHash("MeleeAttack");
        private readonly int _animIDStateTime = Animator.StringToHash("StateTime");
        private readonly int _animIDUseRootMotion = Animator.StringToHash("UseRootMotion");
        private readonly int _animIDAttackIndex = Animator.StringToHash("AttackIndex");

        private Animator _animator;
        private Transform _transform;
        private CharacterController _controller;
        private CharacterInput _input;
        private Vector3 _animatorVelocity;
        private Camera _cam;
        private int AttackIndex => _animator.GetInteger(_animIDAttackIndex);

        private void Awake()
        {
            _cam = Camera.main;
            _animator = GetComponent<Animator>();
            _input = GetComponent<CharacterInput>();
            _controller = GetComponent<CharacterController>();
            _transform = transform;
        }

        protected override void Start()
        {
            base.Start();
            _weapon.Owner = this;
        }

        private void Update()
        {
            _animator.SetFloat(_animIDStateTime, Mathf.Repeat(_animator.GetCurrentAnimatorStateInfo(0).normalizedTime, 1f));
            _animator.ResetTrigger(_animIDMeleeAttack);

            if (_input.Attack)
                _animator.SetTrigger(_animIDMeleeAttack);

            _useRootMotion = _animator.GetBool(_animIDUseRootMotion);
            GroundedCheck();
            Move();
            Rotate();
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
            if(Grounded)
            {
                var targetVelocity = new Vector4(_input.MoveInput.x, 0, _input.MoveInput.y).normalized * MoveSpeed;
                _velocity = Vector3.MoveTowards(_velocity, targetVelocity, MoveAcceleration * Time.deltaTime);
                _velocity.y = 0;

                var newAnimatorVelocity = Quaternion.Inverse(_transform.rotation) * _velocity / MoveSpeed;
                _animatorVelocity = newAnimatorVelocity;
                _animator.SetFloat(_animIDSpeedX, _animatorVelocity.x);
                _animator.SetFloat(_animIDSpeedY, _animatorVelocity.z);
            }
            else
            {
                var verticalVelocity = Mathf.Max(_velocity.y + Gravity * Time.deltaTime, _terminalVelocity);
                _velocity.y = verticalVelocity;
            }
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

            if(_useRootMotion)
                _controller.Move(movement);
            else if(Grounded)
            {
                _controller.SimpleMove(_velocity);
                _transform.rotation = _targetRotation;
            }
            else
                _controller.Move(_velocity * Time.deltaTime);

            _animator.SetBool(_animIDGrounded, Grounded);
        }

        private void Rotate()
        {
            var point = _cam.ScreenToWorldPoint(UnityEngine.Input.mousePosition + Vector3.forward * Vector3.Distance(_cam.transform.position, transform.position));
            var dir = (point - _transform.position).normalized;
            var angle = Mathf.Atan2(dir.x, dir.z) * Mathf.Rad2Deg;
            _targetRotation = Quaternion.Euler(0, angle, 0);
        }

        public void PlayAttackTrail()
        {
            var vfx = _attackTrails[AttackIndex];
            vfx.enabled = true;
            vfx.Play();
        }

        public void MeleeAttackStart()
        {
            var healthAmount = 10 * (AttackIndex + 1);
            var poiseAmount = 10 * AttackIndex;
            _weapon.DamageInfo = DamageInfo.Default().SetHealth(healthAmount).SetPoise(poiseAmount).SetAttacker(this).SetInflictor(_weapon);
            _weapon.SetActive(true);
        }

        public void MeleeAttackEnd()
        {
            _weapon.SetActive(false);
        }
    }
}
