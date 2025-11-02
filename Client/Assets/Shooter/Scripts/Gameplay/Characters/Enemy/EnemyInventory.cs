using System.Collections.Generic;
using System.Linq;
using Shooter.Scripts.Data;
using Shooter.Scripts.Gameplay.Guns.Enemy;
using Shooter.Scripts.Gameplay.Guns.Services;
using Shooter.Scripts.Settings;
using Shooter.Scripts.Settings.Guns;
using UnityEngine;

namespace Shooter.Scripts.Gameplay.Characters.Enemy
{
    public class EnemyInventory : MonoBehaviour
    {
        [SerializeField] private Transform _gunContainer;
        private readonly Dictionary<string, EnemyGun> _enemyGunsMap = new();
        private SettingsProvider _settingsProvider;
        private GunFactory _gunFactory;
        private EnemyGun _currentGun;

        private void Awake()
        {
            Init(new SettingsProvider(), new GunFactory(_gunContainer));
        }

        public void Init(SettingsProvider settingsProvider, GunFactory gunFactory)
        {
            _settingsProvider = settingsProvider;
            _gunFactory = gunFactory;

            _settingsProvider.LoadGameSettings();
        }

        public void SetCurrentGun(string id)
        {
            DisableAllGuns();
            
            GunSettings gunSettings = _settingsProvider.GameSettings.GunsSettings.Guns.FirstOrDefault(g => g.Id == id);

            if (gunSettings == null)
                return;

            if (_enemyGunsMap.TryGetValue(id, out EnemyGun enemyGun))
            {
                _currentGun = enemyGun;
                _currentGun.gameObject.SetActive(true);
                return;
            }

            
            _enemyGunsMap.Add(id, _gunFactory.CreateEnemyGun(gunSettings));
            _currentGun = _enemyGunsMap[id];
            _currentGun.gameObject.SetActive(true);
            _currentGun.Init(gunSettings);
        }

        public void ShootByData(ShootData shootData)
        {
            SetCurrentGun(shootData.GunId);
            _currentGun.Shoot(shootData);
        }

        private void DisableAllGuns()
        {
            for (int i = 0; i < _gunContainer.childCount; i++)
                _gunContainer.GetChild(i).gameObject.SetActive(false);
        }
        
    }
}