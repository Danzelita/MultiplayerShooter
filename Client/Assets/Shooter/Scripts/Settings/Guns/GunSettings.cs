using Shooter.Scripts.Gameplay.Guns.Enemy;
using Shooter.Scripts.Gameplay.Guns.Player;
using Shooter.Scripts.Settings.Guns.Bullets;
using UnityEngine;

namespace Shooter.Scripts.Settings.Guns
{
    [CreateAssetMenu(fileName = "GunSettings", menuName = "GameSettings/Guns/New Gun Settings")]
    public class GunSettings : ScriptableObject
    {
        public string Id;

        public BulletSettings Bullet;
        public float Cooldown;
        public int BulletsPerShoot;
        public int DamagePerBullet;
        public float BulletSpeed;
        public float Spread;
        public Sprite Icon;
        
        public PlayerGun PlayerGunPrefab;
        public EnemyGun EnemyGunPrefab;
    }
}