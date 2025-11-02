using System.Collections.Generic;
using Shooter.Scripts.Multiplayer;
using UnityEngine;

namespace Shooter.Scripts.Gameplay.Characters.Enemy
{
    public class EnemyCharacter : Character
    {
        public Vector3 TargetPosition { get; private set; } = Vector3.zero;
        
        [SerializeField] private Health _health;
        [SerializeField] private Transform _head;
        [SerializeField] private float _rotationLerp;
    
        private MultiplayerManager _multiplayerManager;
        private string _sessinID;
        private float _velocityMagnitude;
        private float _targetRotationX;
        private float _targetRotationY;
        private float _rotationY;
        private float _rotationX;
        private Dictionary<string, object> _data;

        public void Init(MultiplayerManager multiplayerManager, string sessionID)
        {
            _multiplayerManager = multiplayerManager;
            _sessinID = sessionID;
        }

        private void Awake() => 
            TargetPosition = transform.position;

        protected override void Update()
        {
            base.Update();
            
            if (_velocityMagnitude > 0.1f)
            {
                float maxDistance = _velocityMagnitude * Time.deltaTime;
                transform.position = Vector3.MoveTowards(transform.position, TargetPosition, maxDistance);
            }
            else
            {
                transform.position = TargetPosition;
            }

            _rotationY = Mathf.LerpAngle(_rotationY, _targetRotationY, _rotationLerp * Time.deltaTime);
            transform.localEulerAngles = Vector3.up * _rotationY;
            
            _rotationX = Mathf.LerpAngle(_rotationX, _targetRotationX, _rotationLerp * Time.deltaTime);
            _head.localEulerAngles = Vector3.right * _rotationX;
        }

        public void ApplyDamage(int damage)
        {
            _health.ApplyDamage(damage);

            _data = new()
            {
                ["id"] = _sessinID,
                ["value"] = damage,
            };
            _multiplayerManager.SendToServer("damage", _data);
        }

        public void SetSpeed(float speed) => 
            MaxSpeed = speed;

        public void RestoreHP(sbyte changeValue) => 
            _health.SetCurrent(changeValue);

        public void SetMaxHealth(int value)
        {
            MaxHealth = value;
            _health.SetMax(value);
            _health.SetCurrent(value);
        }

        public void SetRotationY(float rotationY) => 
            _targetRotationY = rotationY;

        public void SetRotationX(float rotationX) => 
            _targetRotationX = rotationX;

        public void SetMovement(in Vector3 newPosition, in Vector3 velocity, in float averageInterval)
        {
            TargetPosition = newPosition + (velocity * averageInterval);
            _velocityMagnitude = velocity.magnitude;
        
            Velocity = velocity;
        }
    }
}