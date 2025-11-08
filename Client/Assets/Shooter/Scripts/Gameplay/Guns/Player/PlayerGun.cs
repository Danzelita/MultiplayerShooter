using System.Collections.Generic;
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

            _shootData.Bullets = new List<BulletData>();
        }
        
        public bool TryShoot(out ShootData data)
        {
            data = _shootData;

            if (Time.time - _lastShootTime < _shootCooldown)
                return false;
            
            _lastShootTime = Time.time;

            _shootData.Bullets.Clear();
            Shoot(ref data);
            data.GunId = _gunId;
            
            return true;
        }

        private void Shoot(ref ShootData data)
        {
            for (int i = 0; i < _bullets; i++)
            {
                Vector3 shootDirection = _bulletSpawnPoint.forward;
                
                Vector2 spread = Random.insideUnitCircle * Mathf.Tan(_spread * Mathf.Deg2Rad);
                Vector3 spreadDir = (shootDirection + _bulletSpawnPoint.TransformDirection(new Vector3(spread.x, spread.y, 0f))).normalized;
                Vector3 velocity = spreadDir * _bulletSpeed;
                
                data.Bullets.Add(CreateBullet(velocity));
            }
            OnShoot?.Invoke();
        }

        private BulletData CreateBullet(Vector3 velocity)
        {
            Bullet newBullet = Instantiate(_bulletPrefab, _bulletSpawnPoint.position, _bulletSpawnPoint.rotation);
            newBullet.Init(velocity, _damage);
            
            return new BulletData(_bulletSpawnPoint.position.ToData(), velocity.ToData());
        }
    }
}