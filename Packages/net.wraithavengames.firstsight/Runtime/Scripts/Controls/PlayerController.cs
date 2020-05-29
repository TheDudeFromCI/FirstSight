using UnityEngine;

#pragma warning disable CS0649

namespace WraithavenGames.FirstSight
{
    [RequireComponent(typeof(CharacterController))]
    public class PlayerController : MonoBehaviour
    {
        [Header("Movement Settings")]
        [Tooltip("How fast the player moves normally.")]
        [SerializeField] private float moveSpeed = 6f;
        [Tooltip("How fast the player moves while running.")]
        [SerializeField] private float runSpeed = 10f;
        [Tooltip("How much force to use when jumping.")]
        [SerializeField] private float jumpSpeed = 8f;
        [Tooltip("How much force to apply as gravity.")]
        [SerializeField] private float gravity = 20;

        [Header("Jump Settings")]
        [Tooltip("If enabled, allows for less 'floaty' and move controllable jumps.")]
        [SerializeField] private bool enableHeldJump = true;
        [Tooltip("This setting allows for a very brief window to allow jumping after the "
            + "player leaves a ledge.")]
        [SerializeField] private bool enableCoyoteTime = true;
        [Tooltip("The amount of time for the jump window to remain open.")]
        [SerializeField] private float coyoteTimeWindow = 0.1f;
        [Tooltip("If true, a new jump will be preformed the instant the player touches the "
            + "ground if the jump button is held. Otherwise, it must be pressed for each jump.")]
        [SerializeField] private bool continuousJumping = true;
        [SerializeField, Range(0f, 1f)] private float preserveMomentum = 0.5f;

        [Header("Screen Shake")]
        [SerializeField] private ScreenShake shake;
        [Tooltip("How much trauma to give for each unit fallen.")]
        [SerializeField] private float groundHitTrauma = 0.1f;
        [Tooltip("The mimumn distance the player must fall before trauma is applied.")]
        [SerializeField] private float minFallDistance = 3f;
        [Tooltip("Lessen trauma amount on held jumps.")]
        [SerializeField] private bool lightJumpTrauma = true;

        [Header("Dependencies")]
        [SerializeField] private InputSettings inputSettings;

        private CharacterController controller;
        private Vector3 vel;
        private float lastGrounded;
        private float lastJump;
        private bool isCurrentlyJump;
        private bool wasGrounded;
        private float jumpPeakY;

        public bool IsRunning { get { return inputSettings == null ? false : inputSettings.IsRunning; } }
        public bool IsJumping { get { return inputSettings == null ? false : inputSettings.IsJumping; } }
        public bool HasJumped { get { return inputSettings == null ? false : inputSettings.HasJumped; } }
        public float MoveX { get { return inputSettings == null ? 0f : inputSettings.MoveX; } }
        public float MoveZ { get { return inputSettings == null ? 0f : inputSettings.MoveZ; } }
        public bool IsGrounded { get { return IsGroundedRespectBuffer(); } }
        public bool IsAboutToLand { get { return IsAboutToLandOrGrounded(); } }
        public bool IsCurrentlyJumping { get { return isCurrentlyJump; } }

        public float TEMPJumpHeight { get; set; }
        public float TEMPJumpTime { get; set; }

        private void Awake()
        {
            controller = GetComponent<CharacterController>();
        }

        public void SetGravityAndJumpSpeed(float gravity, float jumpSpeed)
        {
            this.gravity = gravity;
            this.jumpSpeed = jumpSpeed;
        }

        private bool CanJump()
        {
            if (isCurrentlyJump)
                return false;

            return IsGroundedRespectBuffer();
        }

        private bool IsGroundedRespectBuffer()
        {
            if (controller.isGrounded)
                return true;

            if (enableCoyoteTime)
            {
                if (Time.unscaledTime - lastGrounded < coyoteTimeWindow
                    && Time.unscaledTime - lastJump >= coyoteTimeWindow)
                    return true;
            }

            return false;
        }

        private bool IsAboutToLandOrGrounded()
        {
            if (vel.y >= 0f)
                return false;

            Vector3 pos = transform.position + new Vector3(0f, controller.radius, 0f);
            float testZone = controller.skinWidth + (-vel.y + gravity) * Time.deltaTime;

            RaycastHit hit;
            return Physics.SphereCast(pos, controller.radius, -Vector3.up, out hit, testZone);
        }

        private void Update()
        {
            // Get speed
            float speed = moveSpeed;
            if (IsRunning)
                speed = runSpeed;

            // Apply movement
            Vector3 newVel;
            if (preserveMomentum > 0f && !controller.isGrounded)
            {
                Vector3 currentVel = transform.InverseTransformDirection(vel);
                currentVel.y = 0f;

                newVel = new Vector3(MoveX, 0f, MoveZ).normalized * speed;

                newVel = Vector3.Lerp(currentVel, newVel, Time.deltaTime / preserveMomentum);
            }
            else
                newVel = new Vector3(MoveX, 0f, MoveZ).normalized * speed;

            vel = transform.TransformDirection(newVel + new Vector3(0f, vel.y, 0f));

            // Reset vertical movement
            if (controller.isGrounded)
            {
                if (!wasGrounded)
                {
                    float fallHeight = jumpPeakY - transform.position.y;
                    if (fallHeight >= minFallDistance && shake != null)
                    {
                        if (isCurrentlyJump && enableHeldJump && lightJumpTrauma && IsJumping)
                            shake.Hit(fallHeight * groundHitTrauma * 0.75f);
                        else
                            shake.Hit(fallHeight * groundHitTrauma);
                    }
                }

                lastGrounded = Time.unscaledTime;
                jumpPeakY = transform.position.y;
                isCurrentlyJump = false;
                vel.y = 0f;
                wasGrounded = true;
            }
            else
            {
                jumpPeakY = Mathf.Max(jumpPeakY, transform.position.y);
                wasGrounded = false;
            }

            // Apply jump
            bool jump = continuousJumping ? IsJumping : HasJumped;
            if (jump && CanJump())
            {
                lastJump = Time.unscaledTime;
                vel.y = jumpSpeed;
                isCurrentlyJump = true;
            }

            // Apply held jumps
            if (enableHeldJump && !controller.isGrounded)
            {
                // If jump button is not held down, gravity is applied to the player twice
                // Only apply effect if in a jump
                if (isCurrentlyJump && !IsJumping)
                    vel.y -= gravity * Time.deltaTime;
            }

            // Apply gravity
            vel.y -= gravity * Time.deltaTime;

            // Move character
            controller.Move(vel * Time.deltaTime);
        }
    }
}
