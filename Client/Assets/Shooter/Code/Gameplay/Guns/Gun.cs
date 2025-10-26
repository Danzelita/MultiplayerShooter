using System;
using UnityEngine;

namespace Shooter.Code.Gameplay.Guns
{
    public abstract class Gun : MonoBehaviour
    {
        [SerializeField] protected Bullet _bulletPrefab;
        public Action OnShoot;
    }
}