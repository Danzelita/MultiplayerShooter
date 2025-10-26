using System.Collections;
using UnityEngine;

namespace Shooter.Code.Gameplay.Guns
{
    public class Bullet : MonoBehaviour
    {
        [SerializeField] private float _lifeTime = 2f;
        [SerializeField] private Rigidbody _rigidbody;
        public void Init(Vector3 velocity)
        {
            _rigidbody.linearVelocity = velocity;
        
            StartCoroutine(DelayDestroy());
        }

        private void OnCollisionEnter(Collision other) => 
            Destroy();

        private void Destroy() => 
            Destroy(gameObject);

        private IEnumerator DelayDestroy()
        {
            yield return new WaitForSecondsRealtime(_lifeTime);
            Destroy();
        }
    }
}