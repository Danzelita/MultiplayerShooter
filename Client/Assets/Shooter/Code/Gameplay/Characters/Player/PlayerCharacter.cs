using UnityEngine;

namespace Shooter.Code.Gameplay.Characters.Player
{
    public class PlayerCharacter : Character
    {
        [SerializeField] private Rigidbody _rigidbody;
        [SerializeField] private Transform _headTransform;
        [SerializeField] private Transform _cameraPoint;
        [SerializeField] private float _maxHeadAngle = 90f;
        [SerializeField] private float _minHeadAngle = -90f;
        [SerializeField] private float _jumpForce = 100f;
        [SerializeField] private float _jumpCooldown = 0.2f;
        [SerializeField] CheckFly _checkFly;

        private float _currentRotateX;
        private float _hInput;
        private float _vInput;
        private float _rotateY;
        private float _lastJumpTime;
        private float _speed;

        private void Start()
        {
            Camera.main.transform.SetParent(_cameraPoint, false);

            _speed = MaxSpeed;
        }

        private void FixedUpdate()
        {
            Move();
            RotateY();
        }

        public void SetInput(float h, float v, float rotateY)
        {
            _hInput = h;
            _vInput = v;
            _rotateY = rotateY;
        }

        public void Jump()
        {
            if (Time.time - _lastJumpTime < _jumpCooldown)
                return;
            if (_checkFly.IsFly)
                return;
            _lastJumpTime = Time.time;
            _rigidbody.AddForce(Vector3.up * _jumpForce, ForceMode.VelocityChange);
        }

        public void RotateX(float value)
        {
            _currentRotateX = Mathf.Clamp(_currentRotateX + value, _minHeadAngle, _maxHeadAngle);
            _headTransform.localEulerAngles = Vector3.right * _currentRotateX;
        }

        private void RotateY()
        {
            _rigidbody.angularVelocity = Vector3.up * _rotateY;
            _rotateY = 0f;
        }

        private void Move()
        {
            Vector3 velocity = (transform.forward * _vInput + transform.right * _hInput).normalized * _speed;
            velocity.y = _rigidbody.linearVelocity.y;
            Velocity = velocity;
            _rigidbody.linearVelocity = Velocity;
        }

        public override void SetSneak(bool isSneak)
        {
            base.SetSneak(isSneak);
            _speed = isSneak ? _sneakSpeed : MaxSpeed;
        }

        public void GetMoveInfo(
            out Vector3 position,
            out Vector3 velocity,
            out float rotationX,
            out float rotationY,
            out bool isSneak)
        {
            position = transform.position;
            velocity = _rigidbody.linearVelocity;

            rotationX = _headTransform.localEulerAngles.x;
            rotationY = transform.eulerAngles.y;
            
            isSneak = _isSneak;
        }
    }
}