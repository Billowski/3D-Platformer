using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Animations.Rigging;

namespace Platformer
{
    [RequireComponent(typeof(CharacterController))]
    [RequireComponent(typeof(PlayerInput))]

    public class ThirdPersonController : MonoBehaviour
    {
        [Header("Postać")]
        [SerializeField]
        private float _moveSpeed = 2.0f; // prędkość postaci
        [SerializeField]
        private float _sprintSpeed = 5.335f; // prędkość sprintu
        [Range(0.0f, 0.3f)]
        [SerializeField]
        private float _rotationSmoothTime = 0.18f; // prędkość obrotu
        [SerializeField]
        private float _speedChangeRate = 10.0f; // zmiana tempa prędkości

        [Space(10)]
        [SerializeField]
        private float _jumpHeight = 1.2f; // wysokość skoku
        [SerializeField]
        private float _gravity = -15.0f; // grawitacja (oryginalnie -9.81f)

        [Space(10)]
        [SerializeField]
        private float _jumpTimeout = 0.50f; // czas między kolejnym skokiem
        [SerializeField]
        private float _fallTimeout = 0.15f; // czas między przejściem do !Grounded

        [Header("Uziemiona postać")]
        [SerializeField]
        private bool _grounded = true; // czy postać jest na ziemi
        [SerializeField]
        private float _groundedOffset = -0.2f; // offset dla nierównej powierzchni
        [SerializeField]
        private float _groundedRadius = 0.4f; // promień do sprawdzenia czy postać jest na ziemi
        [SerializeField]
        private LayerMask _groundLayers; // jakie warstwy postać traktuje za ziemię

        [Header("Cinemachine")]
        [SerializeField]
        private GameObject _cinemachineCameraTarget; // obiekt, za którym virtualna kamera cinemachine podąża
        [SerializeField]
        private GameObject _followCamera;
        [SerializeField]
        private GameObject _aimCamera;
        [SerializeField]
        private float _sensitivity = 1.0f;
        [SerializeField]
        private float _aimSensitivity = 0.2f;
        [SerializeField]
        private int _invertY;
        [SerializeField]
        private float TopClamp = 70.0f; // górny limit stopni kamery
        [SerializeField]
        private float BottomClamp = -30.0f; // dolny limit stopni kamery
        [SerializeField]
        private bool LockCameraPosition = false; // czy kamera jest zablokowana

        [Header("Celowanie")]
        [SerializeField]
        private GameObject _crosshair;
        [SerializeField]
        private Transform _prefabBulletProjectile;
        [SerializeField]
        private Transform _spawnBulletPosition;
        [SerializeField]
        private Transform _headTarget;
        [SerializeField]
        private Transform _armTarget;
        [SerializeField]
        private Rig _headRig;
        [SerializeField]
        private Rig _armRig;

        // cinemachine
        private float _cinemachineTargetYaw;
        private float _cinemachineTargetPitch;

        // gracz
        private float _speed;
        private float _animationBlend;
        private float _targetRotation = 0.0f;
        private float _rotationVelocity;
        private float _verticalVelocity;
        private float _terminalVelocity = 53.0f;

        // timeout deltatime
        private float _jumpTimeoutDelta;
        private float _fallTimeoutDelta;

        // animation IDs
        private int _animIDSpeed;
        private int _animIDGrounded;
        private int _animIDJump;
        private int _animIDFreeFall;
        private int _animIDMotionSpeed;

        // odniesienia
        private Animator _animator;
        private CharacterController _controller;
        private PlayerInput _input;
        private Camera _mainCamera;

        // editor
        Color transparentGreen = new Color(0.0f, 1.0f, 0.0f, 0.35f);
        Color transparentRed = new Color(1.0f, 0.0f, 0.0f, 0.35f);

        private const float _threshold = 0.01f;

        private void Awake()
        {
            // odniesienie do głównej kamery
            _mainCamera = Camera.main;
        }

        private void Start()
        {
            _sensitivity = PlayerPrefs.HasKey("masterSensitivity") ? PlayerPrefs.GetFloat("masterSensitivity") : 1.0f;
            _invertY = PlayerPrefs.GetInt("masterInvertY") == 0 ? 1 : -1;

            _animator = GetComponentInChildren<Animator>();
            _controller = GetComponent<CharacterController>();
            _input = GetComponent<PlayerInput>();

            AssignAnimationIDs();

            _jumpTimeoutDelta = _jumpTimeout;
            _fallTimeoutDelta = _fallTimeout;
        }

        private void Update()
        {
            GroundedCheck();
            JumpAndGravity();
            Move();
        }

        private void LateUpdate()
        {
            Aiming();
            CameraRotation();
        }

        /// <summary>
        /// Przypisanie ID każdej animacji
        /// </summary>
        private void AssignAnimationIDs()
        {
            _animIDSpeed = Animator.StringToHash("Speed");
            _animIDGrounded = Animator.StringToHash("Grounded");
            _animIDJump = Animator.StringToHash("Jump");
            _animIDFreeFall = Animator.StringToHash("FreeFall");
            _animIDMotionSpeed = Animator.StringToHash("MotionSpeed");
        }

        private void GroundedCheck()
        {
            // set sphere position, with offset
            Vector3 spherePosition = new Vector3(transform.position.x, transform.position.y - _groundedOffset, transform.position.z);
            _grounded = Physics.CheckSphere(spherePosition, _groundedRadius, _groundLayers, QueryTriggerInteraction.Ignore);

            // update animator if using character
            _animator.SetBool(_animIDGrounded, _grounded);
        }

        /// <summary>
        /// Sprawdzanie czy gracz jest uziemiony lub czy skacze
        /// </summary>
        private void JumpAndGravity()
        {
            if (_grounded)
            {
                // reset falltimeout
                _fallTimeoutDelta = _fallTimeout;

                _animator.SetBool(_animIDJump, false);
                _animator.SetBool(_animIDFreeFall, false);

                // ustawienie naszej prędkości do -2f
                if (_verticalVelocity < 0.0f)
                {
                    _verticalVelocity = -2f;
                }

                // Skok
                if (_input.Jump && _jumpTimeoutDelta <= 0.0f)
                {
                    // pierwiastek z H * -2f * G = jaka prędkość potrzebna do osiągnięcia pożądanej wysokości
                    _verticalVelocity = Mathf.Sqrt(_jumpHeight * -2f * _gravity);

                    _animator.SetBool(_animIDJump, true);
                }

                // jump timeout
                if (_jumpTimeoutDelta >= 0.0f)
                {
                    _jumpTimeoutDelta -= Time.deltaTime;
                }
            }
            else
            {
                // reset jump timeout
                _jumpTimeoutDelta = _jumpTimeout;

                // fall timeout
                if (_fallTimeoutDelta >= 0.0f)
                {
                    _fallTimeoutDelta -= Time.deltaTime;
                }
                else
                {
                    _animator.SetBool(_animIDFreeFall, true);
                }

                _input.Jump = false;
            }

            // dodawanie prędkość, póki nie wyniesie prędkości granicznej
            if (_verticalVelocity < _terminalVelocity)
            {
                _verticalVelocity += _gravity * Time.deltaTime;
            }
        }

        /// <summary>
        /// Ruch postaci
        /// </summary>
        private void Move()
        {
            float targetSpeed = _input.Sprint && !_input.Aim ? _sprintSpeed : _moveSpeed;

            if (_input.Move == Vector2.zero) targetSpeed = 0.0f;

            // odniesienie do aktualnej prędkości poziomej graczy
            float currentHorizontalSpeed = new Vector3(_controller.velocity.x, 0.0f, _controller.velocity.z).magnitude;

            float speedOffset = 0.1f;
            float inputMagnitude = _input.AnalogMovement ? _input.Move.magnitude : 1f;

            // przyspieszenie lub zwolnienie do prędkości docelowej
            if (currentHorizontalSpeed < targetSpeed - speedOffset || currentHorizontalSpeed > targetSpeed + speedOffset)
            {
                // tworzy zakrzywiony wynik, a nie liniowy, dając bardziej organiczną zmianę prędkości 
                _speed = Mathf.Lerp(currentHorizontalSpeed, targetSpeed * inputMagnitude, Time.deltaTime * _speedChangeRate);

                // prędkość zaokrąglona do 3 miejsc po przecinku
                _speed = Mathf.Round(_speed * 1000f) / 1000f;
            }
            else
            {
                _speed = targetSpeed;
            }
            _animationBlend = Mathf.Lerp(_animationBlend, targetSpeed, Time.deltaTime * _speedChangeRate);

            // normalizacja inputu
            Vector3 inputDirection = new Vector3(_input.Move.x, 0.0f, _input.Move.y).normalized;

            // obracanie postacią, jeśli postać się porusza
            if (_input.Move != Vector2.zero || _input.Aim)
            {
                _targetRotation = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg + _mainCamera.transform.eulerAngles.y;
                float rotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, _targetRotation, ref _rotationVelocity, _rotationSmoothTime);

                // obrót w zależności od pozycji kamery
                transform.rotation = _input.Aim ? transform.rotation = Quaternion.Euler(0.0f, _mainCamera.transform.eulerAngles.y, 0.0f) : transform.rotation = Quaternion.Euler(0.0f, rotation, 0.0f);
            }


            Vector3 targetDirection = Quaternion.Euler(0.0f, _targetRotation, 0.0f) * Vector3.forward;

            // poruszanie postacią
            _controller.Move(targetDirection.normalized * (_speed * Time.deltaTime) + new Vector3(0.0f, _verticalVelocity, 0.0f) * Time.deltaTime);

            _animator.SetFloat(_animIDSpeed, _animationBlend);
            _animator.SetFloat(_animIDMotionSpeed, inputMagnitude);
        }

        private void Aiming()
        {
            if (_input.Aim && !_aimCamera.activeInHierarchy)
            {
                _aimCamera.SetActive(true);
                _crosshair.SetActive(true);
                _followCamera.SetActive(false);
            }
            else if (!_input.Aim && _aimCamera.activeInHierarchy)
            {
                _aimCamera.SetActive(false);
                _crosshair.SetActive(false);
                _followCamera.SetActive(true);
            }

            // ustawienie vector2 na środku ekranu
            Vector2 screenCenterPoint = new Vector2(Screen.width / 2f, Screen.height / 2f);
            // wysyła promień od kamery do punktu znajdującego się na środku ekranu
            Ray ray = Camera.main.ScreenPointToRay(screenCenterPoint);
            if (Physics.Raycast(ray, out RaycastHit raycastHit, 999f, _groundLayers))
            {
                _headTarget.position = raycastHit.point;
                _armTarget.position = raycastHit.point;
            }

            // sprawdzenie czy postać patrzy się do przodu
            Vector3 heading = _headTarget.position - transform.position;
            float dot = Vector3.Dot(heading, transform.forward);

            _headRig.weight = dot / 3;

            if (_input.Aim)
            {
                _armRig.weight = 1.0f;

                if (_input.Shoot)
                {
                    // wyliczenie kierunku strzału
                    Vector3 aimDir = (_armTarget.position - _spawnBulletPosition.position).normalized;
                    // utworzenie pocisku
                    Instantiate(_prefabBulletProjectile, _spawnBulletPosition.position, Quaternion.LookRotation(aimDir, Vector3.up));
                    _input.Shoot = false;
                }
            }
            else
            {
                _armRig.weight = 0.0f;
            }
        }

        /// <summary>
        /// Obracanie kamerą
        /// </summary>
        private void CameraRotation()
        {
            // jeśli jest poruszanie kamerą i kamera nie jest zablokowana
            if (_input.Look.sqrMagnitude >= _threshold && !LockCameraPosition)
            {
                float sensitivity = _input.Aim ? _aimSensitivity : _sensitivity;
                _cinemachineTargetYaw += _input.Look.x * Time.deltaTime * sensitivity;
                _cinemachineTargetPitch += _input.Look.y * Time.deltaTime * sensitivity * _invertY;
            }

            //  ograniczyć obrót do 360 stopni poziomo oraz pionowo
            _cinemachineTargetYaw = ClampAngle(_cinemachineTargetYaw, float.MinValue, float.MaxValue);
            _cinemachineTargetPitch = ClampAngle(_cinemachineTargetPitch, BottomClamp, TopClamp);

            // cinemachine będzie podążała za tym elementem
            _cinemachineCameraTarget.transform.rotation = Quaternion.Euler(_cinemachineTargetPitch, _cinemachineTargetYaw, 0.0f);
        }

        private static float ClampAngle(float lfAngle, float lfMin, float lfMax)
        {
            if (lfAngle < -360f) lfAngle += 360f;
            if (lfAngle > 360f) lfAngle -= 360f;
            return Mathf.Clamp(lfAngle, lfMin, lfMax);
        }

        private void OnDrawGizmosSelected()
        {
            if (_grounded) Gizmos.color = transparentGreen;
            else Gizmos.color = transparentRed;

            Gizmos.DrawSphere(new Vector3(transform.position.x, transform.position.y - _groundedOffset, transform.position.z), _groundedRadius);
        }
    }
}