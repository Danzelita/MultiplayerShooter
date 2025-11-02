using UnityEngine;

namespace Shooter.Scripts.Logic
{
    public class Rotation : MonoBehaviour
    {
        [SerializeField] private Transform _target;
        [SerializeField] private Vector3 _rotationSpeed = new Vector3(0f, 50f, 0f);
        [SerializeField] private bool _randomStartRotation = false;

        private void Start()
        {
            if (_target == null)
                _target = transform;

            if (_randomStartRotation)
                _target.localEulerAngles = new Vector3(
                    Random.Range(0f, 360f),
                    Random.Range(0f, 360f),
                    Random.Range(0f, 360f)
                );
        }

        private void Update()
        {
            _target.Rotate(_rotationSpeed * Time.deltaTime, Space.Self);
        }
    }
}