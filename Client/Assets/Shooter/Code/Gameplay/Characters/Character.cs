using System;
using UnityEngine;

namespace Shooter.Code.Gameplay.Characters
{
    public abstract class Character : MonoBehaviour
    {
        [field: SerializeField] public float MaxSpeed { get; protected set; } = 5f;
        public Vector3 Velocity { get; protected set; }

        [SerializeField] protected float _sneakSpeed = 2.5f;
        [SerializeField] private float _sneakScale;
        [SerializeField] private Transform _body;
        [SerializeField] private Transform _collider;
        protected bool _isSneak;

        protected virtual void Update()
        {
            Sneak();
        }

        public virtual void SetSneak(bool isSneak)
        {
            _isSneak = isSneak;
            _collider.localScale = new Vector3(1f, _isSneak ? _sneakScale : 1f, 1f);
        }

        private void Sneak()
        {
            float targetBodyScale = _isSneak ? _sneakScale : 1f;
            float yScale = Mathf.Lerp(_body.localScale.y, targetBodyScale, 10f * Time.deltaTime);
            _body.localScale = new Vector3(1f, yScale, 1f);
        }
    }
}