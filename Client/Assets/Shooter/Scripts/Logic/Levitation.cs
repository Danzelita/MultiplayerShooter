using UnityEngine;

namespace Shooter.Scripts.Logic
{
    public class Levitation : MonoBehaviour
    {
        [SerializeField] private Transform _target;
        [SerializeField] private float _amplitude = 0.5f;
        [SerializeField] private float _frequency = 1.5f;
        [SerializeField] private float _offset = 0f;

        private float _startY;

        private void Start()
        {
            if (_target == null)
                _target = transform;

            _startY = _target.localPosition.y;
        }

        private void Update()
        {
            float newY = _startY + Mathf.Sin(Time.time * _frequency + _offset) * _amplitude;
            Vector3 pos = _target.localPosition;
            pos.y = newY;
            _target.localPosition = pos;
        }
    }
}