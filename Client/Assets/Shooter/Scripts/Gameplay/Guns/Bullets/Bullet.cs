using System.Collections;
using UnityEngine;

namespace Shooter.Scripts.Gameplay.Guns.Bullets
{
    public abstract class Bullet : MonoBehaviour
    {
        [SerializeField] private float _lifeTime = 2f;
        [SerializeField] private Rigidbody _rigidbody;
        protected int _damagePerBullet;

        public void Init(Vector3 velocity, int damage = 0)
        {
            _rigidbody.linearVelocity = velocity;
            _damagePerBullet = damage;
        
            StartCoroutine(DelayDestroy());
        }

        private void OnCollisionEnter(Collision other)
        {
            OnCollision(other);
            Destroy();
        }

        protected abstract void OnCollision(Collision other);

        protected virtual void Destroy() => 
            Destroy(gameObject);

        private IEnumerator DelayDestroy()
        {
            yield return new WaitForSecondsRealtime(_lifeTime);
            Destroy();
        }
    }
}