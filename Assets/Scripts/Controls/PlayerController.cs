using UnityEngine;

namespace WraithavenGames.FirstSight
{
	[RequireComponent(typeof(CharacterController))]
	public class PlayerController : MonoBehaviour
	{
		[Header("Movement Settings")]
		[Tooltip("How fast the player moves normally.")]
		[SerializeField] private float _moveSpeed = 6f;
		[Tooltip("How fast the player moves while running.")]
		[SerializeField] private float _runSpeed = 10f;
		[Tooltip("How much force to use when jumping.")]
		[SerializeField] private float _jumpSpeed = 8f;
		[Tooltip("How much force to apply as gravity.")]
		[SerializeField] private float _gravity = 20;

		[Header("Jump Settings")]
		[Tooltip("If enabled, allows for less 'floaty' and move controllable jumps.")]
		[SerializeField] private bool _enableHeldJump = true;
		[Tooltip("This setting allows for a very brief window to allow jumping after the "
			+"player leaves a ledge.")]
		[SerializeField] private bool _enableCoyoteTime = true;
		[Tooltip("The amount of time for the jump window to remain open.")]
		[SerializeField] private float _coyoteTimeWindow = 0.1f;
		[Tooltip("If true, a new jump will be preformed the instant the player touches the "
			+"ground if the jump button is held. Otherwise, it must be pressed for each jump.")]
		[SerializeField] private bool _continuousJumping = true;
		[SerializeField,Range(0f,1f)] private float _preserveMomentum = 0.5f;

		[Header("Screen Shake")]
		[SerializeField] private ScreenShake _shake;
		[Tooltip("How much trauma to give for each unit fallen.")]
		[SerializeField] private float _groundHitTrauma = 0.1f;
		[Tooltip("The mimumn distance the player must fall before trauma is applied.")]
		[SerializeField] private float _minFallDistance = 3f;
		[Tooltip("Lessen trauma amount on held jumps.")]
		[SerializeField] private bool _lightJumpTrauma = true;

		[Header("Dependencies")]
		[SerializeField] private InputSettings _inputSettings;

		private CharacterController _controller;
		private Vector3 _vel;
		private float _lastGrounded;
		private float _lastJump;
		private bool _isCurrentlyJump;
		private bool _wasGrounded;
		private float _jumpPeakY;

		public bool IsRunning { get{ return _inputSettings == null ? false : _inputSettings.IsRunning; } }
		public bool IsJumping { get{ return _inputSettings == null ? false : _inputSettings.IsJumping; } }
		public bool HasJumped { get{ return _inputSettings == null ? false : _inputSettings.HasJumped; } }
		public float MoveX { get{ return _inputSettings == null ? 0f : _inputSettings.MoveX; } }
		public float MoveZ { get{ return _inputSettings == null ? 0f : _inputSettings.MoveZ; } }
		public bool IsGrounded { get{ return IsGroundedRespectBuffer(); } }
		public bool IsAboutToLand { get{ return IsAboutToLandOrGrounded(); } }
		public bool IsCurrentlyJumping { get{ return _isCurrentlyJump; } }

		public float TEMP_JumpHeight { get; set; }
		public float TEMP_JumpTime { get; set; }

		private void Awake()
		{
			_controller = GetComponent<CharacterController>();
		}

		public void SetGravityAndJumpSpeed(float gravity, float jumpSpeed)
		{
			_gravity = gravity;
			_jumpSpeed = jumpSpeed;
		}

		private bool CanJump()
		{
			if (_isCurrentlyJump)
				return false;

			return IsGroundedRespectBuffer();
		}

		private bool IsGroundedRespectBuffer()
		{
			if (_controller.isGrounded)
				return true;

			if (_enableCoyoteTime)
			{
				if (Time.unscaledTime - _lastGrounded < _coyoteTimeWindow
					&& Time.unscaledTime - _lastJump >= _coyoteTimeWindow)
						return true;
			}

			return false;
		}

		private bool IsAboutToLandOrGrounded()
		{
			if (_vel.y >= 0f)
				return false;

			Vector3 pos = transform.position + new Vector3(0f, _controller.radius, 0f);
			float testZone = _controller.skinWidth + (-_vel.y + _gravity) * Time.deltaTime;

			RaycastHit hit;
			return Physics.SphereCast(pos, _controller.radius, -Vector3.up, out hit, testZone);
		}

		private void Update()
		{
			// Get speed
			float speed = _moveSpeed;
			if (IsRunning)
				speed = _runSpeed;

			// Apply movement
			Vector3 newVel;
			if (_preserveMomentum > 0f && !_controller.isGrounded)
			{
				Vector3 currentVel = transform.InverseTransformDirection(_vel);
				currentVel.y = 0f;

				newVel = new Vector3(MoveX, 0f, MoveZ).normalized * speed;

				newVel = Vector3.Lerp(currentVel, newVel, Time.deltaTime / _preserveMomentum);
			}
			else
				newVel = new Vector3(MoveX, 0f, MoveZ).normalized * speed;

			_vel = transform.TransformDirection(newVel + new Vector3(0f, _vel.y, 0f));

			// Reset vertical movement
			if (_controller.isGrounded)
			{
				if (!_wasGrounded)
				{
					float fallHeight = _jumpPeakY - transform.position.y;
					if (fallHeight >= _minFallDistance && _shake != null)
					{
						if (_isCurrentlyJump && _enableHeldJump && _lightJumpTrauma && IsJumping)
							_shake.Hit(fallHeight * _groundHitTrauma * 0.75f);
						else
							_shake.Hit(fallHeight * _groundHitTrauma);
					}
				}

				_lastGrounded = Time.unscaledTime;
				_jumpPeakY = transform.position.y;
				_isCurrentlyJump = false;
				_vel.y = 0f;
				_wasGrounded = true;
			}
			else
			{
				_jumpPeakY = Mathf.Max(_jumpPeakY, transform.position.y);
				_wasGrounded = false;
			}

			// Apply jump
			bool jump = _continuousJumping ? IsJumping : HasJumped;
			if (jump && CanJump())
			{
				_lastJump = Time.unscaledTime;
				_vel.y = _jumpSpeed;
				_isCurrentlyJump = true;
			}

			// Apply held jumps
			if (_enableHeldJump && !_controller.isGrounded)
			{
				// If jump button is not held down, gravity is applied to the player twice
				// Only apply effect if in a jump
				if (_isCurrentlyJump && !IsJumping)
					_vel.y -= _gravity * Time.deltaTime;
			}

			// Apply gravity
			_vel.y -= _gravity * Time.deltaTime;

			// Move character
			_controller.Move(_vel * Time.deltaTime);
		}
	}
}
