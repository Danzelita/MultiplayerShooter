using Shooter.Scripts.Gameplay.Characters.Enemy;
using UnityEngine;

namespace Shooter.Scripts.Gameplay.Guns.Bullets
{
    public class GrenadeBullet : Bullet
    {
        [SerializeField] private float _radius;
        [SerializeField] private GameObject _explodionEffect;
        [SerializeField] private AnimationCurve _damageCurve;
        private Collider[] _colliders = new Collider[5];

        protected override void OnCollision(Collision other) => 
            Explode(at: other.contacts[0].point);

        private void Explode(Vector3 at)
        {
            int size = Physics.OverlapSphereNonAlloc(at, _radius, _colliders);
            
            for (int i = 0; i < size; i++)
            {
                Collider collider = _colliders[i];

                if (collider.gameObject.TryGetComponent(out EnemyDamageCollider damageCollider) == false) 
                    continue;

                if (damageCollider.IsSingle == false) 
                    continue;
                
                float distance = Vector3.Distance(at, collider.transform.position);
                float lerp = Mathf.InverseLerp(0, _radius, distance);
                float rawDamage = _damageCurve.Evaluate(lerp) * _damagePerBullet;
                int damage = Mathf.CeilToInt(rawDamage);
                        
                damageCollider.Character.ApplyDamage(damage * damageCollider.DamageMultiplier);
            }

            SpawnExpoleEffect(at);
        }

        private void SpawnExpoleEffect(Vector3 at)
        {
            GameObject newEffect = Instantiate(_explodionEffect, at, Quaternion.identity);
            Destroy(newEffect, 2f);
        }
    }
}