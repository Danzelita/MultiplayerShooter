using Shooter.Scripts.Data;
using Shooter.Scripts.Gameplay.Guns.Bullets;
using Shooter.Scripts.Settings.Guns;
using UnityEngine;

namespace Shooter.Scripts.Gameplay.Guns.Enemy
{
    public class EnemyGun : Gun
    {
        private GunSettings _gunSettings;
        private float _spread;
        private int _bullets;
        private float _bulletSpeed;

        public override void Init(GunSettings gunSettings)
        {
            base.Init(gunSettings);
            _gunSettings = gunSettings;
            _spread = gunSettings.Spread;
            _bullets = gunSettings.BulletsPerShoot;
            _bulletSpeed = gunSettings.BulletSpeed;
            _bulletPrefab = gunSettings.Bullet.Prefab;
        }

        public void Shoot(ShootData shootData)
        {
            Random.InitState(shootData.Seed);
            Vector3 spawnPosition = shootData.Pos.ToVector3();
            Vector3 direction = shootData.Dir.ToVector3();
            
            for (int i = 0; i < _bullets; i++)
            {
                Vector3 spreadDirection = GetSpreadDirection(direction, _spread);
                CreateBullet(at: spawnPosition, velocity: spreadDirection * _bulletSpeed);
            }
            OnShoot?.Invoke();
        }
    }
}