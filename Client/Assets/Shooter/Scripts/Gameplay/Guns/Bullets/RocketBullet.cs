using System;
using Shooter.Scripts.Gameplay.Characters.Enemy;
using Shooter.Scripts.Gameplay.Characters.Player;
using UnityEngine;

namespace Shooter.Scripts.Gameplay.Guns.Bullets
{
    public class RocketBullet : Bullet
    {
        [SerializeField] private float _upwardModifier;
        [SerializeField] private float _explosionForce;
        [SerializeField] private float _radius;
        [SerializeField] private GameObject _explodionEffect;
        [SerializeField] private AnimationCurve _forceCurve;
        [SerializeField] private AnimationCurve _damageCurve;
        [SerializeField] private GameObject _flyingEffect;
        [SerializeField]private float _destroyFlyingEffectDelay;
        
        private Collider[] _colliders = new Collider[5];

        protected override void OnCollision(Collision other) => 
            Explode(at: other.contacts[0].point);

        private void Explode(Vector3 at)
        {
            int size = Physics.OverlapSphereNonAlloc(at, _radius, _colliders);

            SpawnExpoleEffect(at);
            
            for (int i = 0; i < size; i++)
            {
                Collider collider = _colliders[i];

                if (collider.attachedRigidbody != null && collider.attachedRigidbody.GetComponent<PlayerController>())
                {
                    Vector3 toPlayer = Vector3.Normalize(collider.attachedRigidbody.transform.position - at);
                    float forceInverseLerp = GetDictanceInverseLerp(at, collider.attachedRigidbody.transform.position);
                    float forceMultiplayer = _forceCurve.Evaluate(forceInverseLerp);
                    Vector3 force = toPlayer * forceMultiplayer + Vector3.up * _upwardModifier;
                    
                    collider.attachedRigidbody.AddForce(force, ForceMode.VelocityChange);
                    
                    //collider.attachedRigidbody.AddExplosionForce(_explosionForce, at, _radius, _upwardModifier, ForceMode.VelocityChange);
                    continue;
                }

                if (collider.gameObject.TryGetComponent(out EnemyDamageCollider damageCollider) == false) 
                    continue;

                if (damageCollider.IsSingle == false) 
                    continue;
                
                float damageInverseLerp = GetDictanceInverseLerp(at, collider.transform.position);
                float rawDamage = _damageCurve.Evaluate(damageInverseLerp) * _damagePerBullet;
                int damage = Mathf.CeilToInt(rawDamage);
                        
                damageCollider.Character.ApplyDamage(damage * damageCollider.DamageMultiplier);
            }
        }

        private float GetDictanceInverseLerp(Vector3 position1, Vector3 position2)
        {
            float distance = Vector3.Distance(position1, position2);
            float lerp = Mathf.InverseLerp(0, _radius, distance);
            return lerp;
        }

        private void SaveDestroyFlyingEffect()
        {
            _flyingEffect.transform.parent = null;
            Destroy(_flyingEffect.gameObject, _destroyFlyingEffectDelay);
        }

        protected override void Destroy()
        {
            base.Destroy();
            SaveDestroyFlyingEffect();
        }

        private void SpawnExpoleEffect(Vector3 at)
        {
            GameObject newEffect = Instantiate(_explodionEffect, at, Quaternion.identity);
            Destroy(newEffect, 2f);
        }
    }
}