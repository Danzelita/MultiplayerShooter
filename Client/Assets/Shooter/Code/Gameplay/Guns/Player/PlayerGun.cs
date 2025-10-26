using Shooter.Code.Data;
using UnityEngine;

namespace Shooter.Code.Gameplay.Guns.Player
{
    public class PlayerGun : Gun
    {
        [SerializeField] private Transform _bulletSpawnPoint;
        [SerializeField] float _bulletSpeed;
        [SerializeField] private float _shootCooldown;

        private float _lastShootTime;


        public bool TryShoot(out ShootData data)
        {
            data = new ShootData();
        
            if (Time.time - _lastShootTime < _shootCooldown)
                return false;
        
            _lastShootTime = Time.time;
        
            Vector3 position = _bulletSpawnPoint.position;
            Vector3 velocity = _bulletSpawnPoint.forward * _bulletSpeed;
        
            Instantiate(_bulletPrefab, _bulletSpawnPoint.position, _bulletSpawnPoint.rotation)
                .Init(velocity);

            OnShoot?.Invoke();

            data.Pos = position.ToData();
            data.Vel = velocity.ToData();
        
            return true;
        }
    }
}