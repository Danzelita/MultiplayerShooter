using System;
using System.Collections.Generic;
using Colyseus.Schema;
using Shooter.Scripts.Multiplayer;
using Shooter.Scripts.UI;
using UnityEngine;

namespace Shooter.Scripts.Gameplay.Characters.Player
{
    public class PlayerCharacter : Character
    {
        [SerializeField] private Health _health;
        [SerializeField] private Rigidbody _rigidbody;
        [SerializeField] private Transform _headTransform;
        [SerializeField] private Transform _cameraPoint;
        [SerializeField] private float _maxHeadAngle = 90f;
        [SerializeField] private float _minHeadAngle = -90f;
        [SerializeField] private float _jumpForce = 100f;
        [SerializeField] private float _jumpCooldown = 0.2f;
        [SerializeField] private KillsDisplay _killsDisplay;
        [SerializeField] CheckFly _checkFly;

        private Multiplayer.generated.Player _player;
        private float _currentRotateX;
        private float _hInput;
        private float _vInput;
        private float _rotateY;
        private float _lastJumpTime;
        private float _speed;

        public void Init(Multiplayer.generated.Player player)
        {
            _player = player;
            player.OnChange += OnChange;
            
            _health.SetMax(player.maxHP);
            _health.SetCurrent(player.currentHP);
        }

        private void OnDestroy() => 
            _player.OnChange -= OnChange;

        private void OnChange(List<DataChange> changes)
        {
            foreach (DataChange change in changes)
                switch (change.Field)
                {
                    case "currentHP":
                        _health.SetCurrent((sbyte)change.Value);
                        break;
                    case "kills":
                        _killsDisplay.SetKills((UInt16)change.Value);
                        break;
                }
        }

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
            _rigidbody.MoveRotation(_rigidbody.rotation * Quaternion.Euler(Vector3.up * _rotateY));
            _rotateY = 0f;
        }

        private void Move()
        {
            Vector3 velocity = (transform.forward * _vInput + transform.right * _hInput).normalized * _speed;
            velocity.y = _rigidbody.linearVelocity.y;
            Velocity = velocity;
            _rigidbody.linearVelocity = Velocity;
        }

        public override void SetCrouch(bool isCrouch)
        {
            base.SetCrouch(isCrouch);
            _speed = isCrouch ? _sneakSpeed : MaxSpeed;
        }

        public void GetMoveInfo(
            out Vector3 position,
            out Vector3 velocity,
            out float rotationX,
            out float rotationY
            )
        {
            position = transform.position;
            velocity = _rigidbody.linearVelocity;

            rotationX = _headTransform.localEulerAngles.x;
            rotationY = transform.eulerAngles.y;
        }
        public bool IsCrouch() => _isCrouch;
    }
}