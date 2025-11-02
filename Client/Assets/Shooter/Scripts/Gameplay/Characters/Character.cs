using UnityEngine;

namespace Shooter.Scripts.Gameplay.Characters
{
    public abstract class Character : MonoBehaviour
    {
        [field: SerializeField] public int MaxHealth { get; protected set; } = 10;
        [field: SerializeField] public float MaxSpeed { get; protected set; } = 5f;
        public Vector3 Velocity { get; protected set; }

        [SerializeField] protected float _sneakSpeed = 2.5f;
        [SerializeField] private float _sneakScale;
        [SerializeField] private Transform _body;
        [SerializeField] private Transform _collider;
        protected bool _isCrouch;

        protected virtual void Update()
        {
            Crouch();
        }

        public virtual void SetCrouch(bool isCrouch)
        {
            _isCrouch = isCrouch;
            _collider.localScale = new Vector3(1f, _isCrouch ? _sneakScale : 1f, 1f);
        }

        private void Crouch()
        {
            float targetBodyScale = _isCrouch ? _sneakScale : 1f;
            float yScale = Mathf.Lerp(_body.localScale.y, targetBodyScale, 10f * Time.deltaTime);
            _body.localScale = new Vector3(1f, yScale, 1f);
        }
    }
}