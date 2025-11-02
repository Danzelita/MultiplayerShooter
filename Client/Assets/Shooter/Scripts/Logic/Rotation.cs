using UnityEngine;

namespace Shooter.Scripts.Logic
{
    public class Rotation : MonoBehaviour
    {
        [SerializeField] private Transform _target;
        [SerializeField] private Vector3 _rotationSpeed = new Vector3(0f, 50f, 0f);

        private void Start()
        {
            if (_target == null)
                _target = transform;
        }

        private void Update()
        {
            _target.Rotate(_rotationSpeed * Time.deltaTime, Space.Self);
        }
    }
}