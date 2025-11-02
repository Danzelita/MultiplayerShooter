using UnityEngine;

namespace Shooter.Scripts.Multiplayer
{
    public class PointMarker : MonoBehaviour
    {
        [SerializeField] private string _tag;
        [SerializeField] private Color _color = Color.white;
        
        public string Tag => _tag;

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            Gizmos.color = _color;
            
            Gizmos.DrawSphere(transform.position, 0.25f);
            Gizmos.DrawRay(transform.position, Vector3.up * 2f);
        }
    }
#endif
}