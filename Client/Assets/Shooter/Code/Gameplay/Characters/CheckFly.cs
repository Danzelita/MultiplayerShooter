using UnityEngine;

namespace Shooter.Code.Gameplay.Characters
{
    public class CheckFly : MonoBehaviour
    {
        [SerializeField] private float _radius = 0.1f;
        [SerializeField] private LayerMask _layerMask;
        [SerializeField] private float _coyotTime = 0.15f;

        private float _flyTimer = 0f;

        public bool IsFly { get; private set; }

        private void Update()
        {
            if (Physics.CheckSphere(transform.position, _radius, _layerMask))
            {
                IsFly = false;
                _flyTimer = 0f;
            }
            else
            {
                _flyTimer += Time.deltaTime;
                if (_flyTimer > _coyotTime) 
                    IsFly = true;
            }
        }

#if UNITY_EDITOR
        private void OnDrawGizmos() =>
            Gizmos.DrawSphere(transform.position, _radius);
#endif
    }
}