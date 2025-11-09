using System;
using Shooter.Scripts.Gameplay.Guns.Bullets;
using Shooter.Scripts.Settings.Guns;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Shooter.Scripts.Gameplay.Guns
{
    public abstract class Gun : MonoBehaviour
    {
        protected const int SeedStep = 1000;
        protected Bullet _bulletPrefab;
        protected string _gunId;
        public Action OnShoot;

        public virtual void Init(GunSettings gunSettings)
        {
            _gunId = gunSettings.Type;
        }

        protected Vector3 GetSpreadDirection(Vector3 direction, float spreadDegrees)
        {
            direction = direction.normalized;
            Vector3 arbitrary = Mathf.Abs(Vector3.Dot(direction, Vector3.up)) > 0.999f ? Vector3.forward : Vector3.up;

            Vector3 right = Vector3.Cross(arbitrary, direction).normalized;
            Vector3 upLocal = Vector3.Cross(direction, right);
            
            float angle = Random.Range(0f, 1f) * 2f * Mathf.PI;
            float r = Random.Range(0f, 1f);

            float x = Mathf.Cos(angle) * r;
            float y = Mathf.Sin(angle) * r;

            float spreadFactor = Mathf.Tan(spreadDegrees * Mathf.Deg2Rad);

            Vector3 offset = right * (x * spreadFactor) + upLocal * (y * spreadFactor);

            Vector3 spreadDirection = (direction + offset).normalized;
            return spreadDirection;
        }

        protected void CreateBullet(Vector3 at, Vector3 velocity, int damage = 0)
        {
            Bullet newBullet = Instantiate(_bulletPrefab, at, Quaternion.LookRotation(velocity));
            newBullet.Init(velocity, damage);
        }
    }
}