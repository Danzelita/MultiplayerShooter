using Shooter.Scripts.Data;
using Shooter.Scripts.Gameplay.Guns.Bullets;
using Shooter.Scripts.Settings.Guns;
using UnityEngine;

namespace Shooter.Scripts.Gameplay.Guns.Enemy
{
    public class EnemyGun : Gun
    {
        private GunSettings _gunSettings;

        public override void Init(GunSettings gunSettings)
        {
            base.Init(gunSettings);
            _gunSettings = gunSettings;
        }

        public void Shoot(ShootData shootData)
        {
            foreach (BulletData bulletData in shootData.Bullets) 
                CreateBullet(_gunSettings.Bullet.Prefab, bulletData);
            
            OnShoot?.Invoke();
        }

        private static void CreateBullet(Bullet prefab, BulletData bulletData)
        {
            Quaternion toVelocity = Quaternion.LookRotation(bulletData.Vel.ToVector3());
            Bullet bullet = Instantiate(prefab, bulletData.Pos.ToVector3(), toVelocity);
            bullet.Init(bulletData.Vel.ToVector3());
        }
    }
}