using Shooter.Scripts.Gameplay.Guns.Enemy;
using Shooter.Scripts.Gameplay.Guns.Player;
using Shooter.Scripts.Settings.Guns;
using UnityEngine;

namespace Shooter.Scripts.Gameplay.Guns.Services
{
    public class GunFactory
    {
        private Transform _gunContainer;

        public GunFactory()
        {
        }
        public GunFactory(Transform gunContainer)
        {
            _gunContainer = gunContainer;
        }

        public void Init(Transform gunContainer)
        {
            _gunContainer = gunContainer;
        }
        
        public PlayerGun CreatePlayerGun(GunSettings gunSettings)
        {
            PlayerGun newPlayerGun = Object.Instantiate(gunSettings.PlayerGunPrefab, _gunContainer.position, _gunContainer.rotation, _gunContainer);
            newPlayerGun.Init(gunSettings);

            return newPlayerGun;
        }

        public EnemyGun CreateEnemyGun(GunSettings gunSettings)
        {
            return Object.Instantiate(gunSettings.EnemyGunPrefab, _gunContainer.position, _gunContainer.rotation, _gunContainer);
        }
    }
}