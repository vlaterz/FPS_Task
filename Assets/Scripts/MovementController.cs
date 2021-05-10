using UnityEngine;
using UnityEngine.Events;
using UnityStandardAssets.CrossPlatformInput;

namespace Assets.Scripts
{
    public class MovementController : MonoBehaviour, IMovementController
    {
        [SerializeField] private float _walkSpeed = 5f;
        [SerializeField] private float _jumpSpeed = 1f;
        [SerializeField] private float _stickToGroundForce = 1f;
        [SerializeField] private float _gravityMultiplier = 1f;

        [SerializeField] private StepController _stepController = new StepController();

        public static UnityEvent StepEvent = new UnityEvent();
        public static UnityEvent JumpEvent = new UnityEvent();
        public static UnityEvent LandEvent = new UnityEvent();

        private Vector2 _input;
        private Vector3 _moveDir = Vector3.zero;
        private CharacterController _characterController;
        private CollisionFlags _collisionFlags;
        private bool _previouslyGrounded;
        private bool _jumping;
        private bool _jump;

        void Awake()
        {
            _characterController = GetComponent<CharacterController>();
            _jumping = false;
        }

        private void Update()
        {
            // the jump state needs to read here to make sure it is not missed
            if (!_jump)
            {
                _jump = CrossPlatformInputManager.GetButtonDown("Jump");
            }

            if (!_previouslyGrounded && _characterController.isGrounded)
            {
                _moveDir.y = 0f;
                _jumping = false;
                LandEvent.Invoke();
            }

            if (!_characterController.isGrounded && !_jumping && _previouslyGrounded)
            {
                _moveDir.y = 0f;
            }

            _previouslyGrounded = _characterController.isGrounded;
        }


        private void FixedUpdate()
        {
            GetInput(out float speed);
            // always move along the camera forward as it is the direction that it being aimed at
            Vector3 desiredMove = transform.forward * _input.y + transform.right * _input.x;

            // get a normal for the surface that is being touched to move along it
            Physics.SphereCast(transform.position, _characterController.radius, Vector3.down, out RaycastHit hitInfo,
                               _characterController.height / 2f, Physics.AllLayers, QueryTriggerInteraction.Ignore);
            desiredMove = Vector3.ProjectOnPlane(desiredMove, hitInfo.normal).normalized;

            _moveDir.x = desiredMove.x * speed;
            _moveDir.z = desiredMove.z * speed;

            if (_characterController.isGrounded)
            {
                _moveDir.y = -_stickToGroundForce;

                if (_jump)
                {
                    _moveDir.y = _jumpSpeed;
                    _jump = false;
                    _jumping = true;
                    JumpEvent.Invoke();
                }
            }
            else
            {
                if (_collisionFlags == CollisionFlags.Above)
                {
                    _moveDir.y = 0;
                }
                _moveDir += Physics.gravity * _gravityMultiplier * Time.fixedDeltaTime;
            }
            _collisionFlags = _characterController.Move(_moveDir * Time.fixedDeltaTime);

            if (_stepController.IsTimeToMakeAStep(speed, _characterController.velocity) && !_jumping &&
                _characterController.isGrounded)
            {
                StepEvent.Invoke();
            }
        }

        private void GetInput(out float speed)
        {
            float horizontal = CrossPlatformInputManager.GetAxis("Horizontal");
            float vertical = CrossPlatformInputManager.GetAxis("Vertical");

            speed = _walkSpeed;

            _input = new Vector2(horizontal, vertical);

            // normalize input if it exceeds 1 in combined length:
            if (_input.sqrMagnitude > 1)
            {
                _input.Normalize();
            }
        }
    }
}
