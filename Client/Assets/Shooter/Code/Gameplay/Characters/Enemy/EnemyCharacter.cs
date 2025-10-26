using UnityEngine;

namespace Shooter.Code.Gameplay.Characters.Enemy
{
    public class EnemyCharacter : Character
    {
        [SerializeField] private Transform _head;
        [SerializeField] private float _rotationLerp;
    
        public Vector3 TargetPosition { get; private set; } = Vector3.zero;
        private float _velocityMagnitude;
        
        private float _targetRotationX;
        private float _targetRotationY;
        private float _rotationY;
        private float _rotationX;

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
                transform.position = Vector3.Lerp(transform.position, TargetPosition, Time.deltaTime * 20f);
            }

            _rotationY = Mathf.LerpAngle(_rotationY, _targetRotationY, _rotationLerp * Time.deltaTime);
            transform.localEulerAngles = Vector3.up * _rotationY;
            
            _rotationX = Mathf.LerpAngle(_rotationX, _targetRotationX, _rotationLerp * Time.deltaTime);
            _head.localEulerAngles = Vector3.right * _rotationX;
        }
        public void SetSpeed(float speed) => 
            MaxSpeed = speed;

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