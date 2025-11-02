using System;
using Shooter.Scripts.Gameplay.Guns.Bullets;
using Shooter.Scripts.Settings.Guns;
using UnityEngine;

namespace Shooter.Scripts.Gameplay.Guns
{
    public abstract class Gun : MonoBehaviour
    {
        protected Bullet _bulletPrefab;
        protected string _gunId;
        public Action OnShoot;

        public virtual void Init(GunSettings gunSettings)
        {
            _gunId = gunSettings.Id;
        }
    }
}