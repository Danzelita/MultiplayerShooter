
using Shooter.Scripts.Data;
using Shooter.Scripts.Gameplay.Guns.Bullets;
using Shooter.Scripts.Settings.Guns;
using UnityEngine;


namespace Shooter.Scripts.Gameplay.Guns.Player
{
    public class PlayerGun : Gun
    {
        [SerializeField] private Transform _bulletSpawnPoint;

        private int _bullets;
        private float _spread;
        private int _damage;
        private float _bulletSpeed;
        private float _shootCooldown;

        private float _lastShootTime;
        private ShootData _shootData;


        public override void Init(GunSettings gunSettings)
        {
            base.Init(gunSettings);
            _bullets = gunSettings.BulletsPerShoot;
            _bulletSpeed = gunSettings.BulletSpeed;
            _damage = gunSettings.DamagePerBullet;
            _shootCooldown = gunSettings.Cooldown;
            _spread = gunSettings.Spread;
            _bulletPrefab = gunSettings.Bullet.Prefab;
        }

        public bool TryShoot(out ShootData data)
        {
            data = _shootData;

            if (Time.time - _lastShootTime < _shootCooldown)
                return false;

            _lastShootTime = Time.time;

            Shoot(ref data);
            data.GunId = _gunId;

            return true;
        }

        private void Shoot(ref ShootData data)
        {
            Vector3 shootDirection = _bulletSpawnPoint.forward;
            Vector3 spawnPosition = _bulletSpawnPoint.position;

            int seed = Random.Range(0, 99999);
            Random.InitState(seed);
            
            data.Seed = seed;
            data.Pos = spawnPosition.ToData();
            data.Dir = shootDirection.ToData();
            
            for (int i = 0; i < _bullets; i++)
            {
                Vector3 spreadDirection = GetSpreadDirection(shootDirection, _spread);
                CreateBullet(at: spawnPosition, velocity: spreadDirection * _bulletSpeed, _damage);
            }

            OnShoot?.Invoke();
        }
    }
}