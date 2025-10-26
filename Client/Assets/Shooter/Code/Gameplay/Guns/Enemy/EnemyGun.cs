using UnityEngine;

namespace Shooter.Code.Gameplay.Guns.Enemy
{
    public class EnemyGun : Gun
    {
        public void Shoot(Vector3 position, Vector3 velocity)
        {
            Instantiate(_bulletPrefab, position, Quaternion.identity).Init(velocity);
            OnShoot?.Invoke();
        }
    }
}