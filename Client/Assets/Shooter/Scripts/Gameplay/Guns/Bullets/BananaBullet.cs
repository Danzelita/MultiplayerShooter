using Shooter.Scripts.Gameplay.Characters.Enemy;
using UnityEngine;

namespace Shooter.Scripts.Gameplay.Guns.Bullets
{
    public class BananaBullet : Bullet
    {
        [SerializeField] private GameObject _bananaEffect;
        protected override void OnCollision(Collision other)
        {
            Debug.Log(other);
            Debug.Log(other.gameObject.name);
            if (other.gameObject.TryGetComponent<EnemyDamageCollider>(out var enemyDamageCollider)) 
                enemyDamageCollider.Character.ApplyDamage(_damagePerBullet * enemyDamageCollider.DamageMultiplier);
        }

        protected override void Destroy()
        {
            base.Destroy();
            GameObject bananaEffect = Instantiate(_bananaEffect, transform.position, Quaternion.identity);
            Destroy(bananaEffect, 2f);
        }
    }
}