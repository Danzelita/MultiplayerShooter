using Shooter.Scripts.Gameplay.Guns.Bullets;
using UnityEngine;

namespace Shooter.Scripts.Settings.Guns.Bullets
{
    [CreateAssetMenu(fileName = "BulletSettings", menuName = "GameSettings/Bullets/New Bullet Settings")]
    public class BulletSettings : ScriptableObject
    {
        public Bullet Prefab;
    }
}