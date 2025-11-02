using UnityEngine;

namespace Shooter.Scripts.Logic
{
    public class AttachToPoint : MonoBehaviour
    {
        [SerializeField] private Transform _target;

        private void LateUpdate() => 
            transform.position = _target.position;
    }
}