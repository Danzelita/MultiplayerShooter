using Shooter.Scripts.Gameplay.Characters.Enemy;
using UnityEngine;

namespace Shooter.Scripts.Gameplay.Guns.Bullets
{
    public class DefaultBullet : Bullet
    {
        protected override void OnCollision(Collision other)
        {
            if (other.gameObject.TryGetComponent<EnemyDamageCollider>(out var enemyDamageCollider)) 
                enemyDamageCollider.Character.ApplyDamage(_damagePerBullet * enemyDamageCollider.DamageMultiplier);
        }
    }
}