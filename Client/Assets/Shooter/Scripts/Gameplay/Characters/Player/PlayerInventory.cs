using System;
using System.Collections.Generic;
using System.Linq;
using Shooter.Scripts.Data;
using Shooter.Scripts.Gameplay.Guns.Player;
using Shooter.Scripts.Gameplay.Guns.Services;
using Shooter.Scripts.Gameplay.Loot.Servcies;
using Shooter.Scripts.Settings;
using Shooter.Scripts.Settings.Guns;
using UnityEngine;

namespace Shooter.Scripts.Gameplay.Characters.Player
{
    public class PlayerInventory : MonoBehaviour
    {
        [SerializeField] private Transform _gunContainer;
        [SerializeField] PlayerLootCollector _playerLootCollector;

        public event Action<int> CurrentSlotChanged; 
        public event Action<int, string> InventoryChanged;
        private readonly Dictionary<int, string> _inventory = new();
        private readonly Dictionary<string, PlayerGun> _playerGunsMap = new();
        private int _currentSlot = 0;
        private GunFactory _gunFactory;
        private SettingsProvider _settingsProvider;
        private LootService _lootService;

        public void Init(SettingsProvider settingsProvider, GunFactory gunFactory, LootService lootService)
        {
            _settingsProvider = settingsProvider;
            _gunFactory = gunFactory;
            _lootService = lootService;
            
            _gunFactory.Init(_gunContainer);
            SetCurrentGunByIndex(0);
        }

        public void SetCurrentGunByIndex(int index)
        {
            DisableAllGuns();

            _currentSlot = index;
            
            UpdateInventory();

            if (_inventory.TryGetValue(_currentSlot, out string id) == false)
                return;

            GunSettings gunSettings = _settingsProvider
                .GameSettings
                .GunsSettings
                .Guns
                .FirstOrDefault(g => g.Id == id);

            UpdateInventory();
            
            if (gunSettings == null)
                return;

            if (_playerGunsMap.TryGetValue(id, out var playerGun))
            {
                playerGun.gameObject.SetActive(true);
            }
            else
            {
                _playerGunsMap[id] = _gunFactory.CreatePlayerGun(gunSettings);
                _playerGunsMap[id].gameObject.SetActive(true);
            }
            
            UpdateInventory();
        }

        public bool TryShootByCurrentGun(out ShootData shootData)
        {
            shootData = new ShootData();

            if (_inventory.TryGetValue(_currentSlot, out string gunId) == false)
                return false;

            if (_playerGunsMap.TryGetValue(gunId, out PlayerGun playerGun))
            {
                bool result = playerGun.TryShoot(out ShootData data);
                shootData = data;
                return result;
            }

            return false;
        }
        
        public void TryPickUp()
        {
            string lootId = _playerLootCollector.TryPickUp();

            if (string.IsNullOrEmpty(lootId))
                return;
            
            if (_inventory.ContainsKey(_currentSlot)) 
                TryDrop();
            
            _inventory[_currentSlot] = lootId;
            SetCurrentGunByIndex(_currentSlot);
            
            UpdateInventory();
        }

        public string GetCurrentGunId() => 
            _inventory.TryGetValue(_currentSlot, out string gunId) ? gunId : string.Empty;

        public void TryDrop()
        {
            if (_inventory.TryGetValue(_currentSlot, out string id) == false) 
                return;
            
            _lootService.DropByLootIdAtPosition(id, transform.position);
            _inventory.Remove(_currentSlot);
            SetCurrentGunByIndex(_currentSlot);

            UpdateInventory();
        }

        private void DisableAllGuns()
        {
            for (int i = 0; i < _gunContainer.childCount; i++)
                _gunContainer.GetChild(i).gameObject.SetActive(false);
        }

        private void UpdateInventory()
        {
            bool avalibleItem = _inventory.TryGetValue(_currentSlot, out string id);
            InventoryChanged?.Invoke(_currentSlot, avalibleItem ? id : string.Empty);
            CurrentSlotChanged?.Invoke(_currentSlot);
        }
    }
}