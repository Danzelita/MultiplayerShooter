using System;
using Shooter.Scripts.Gameplay.Characters.Player;
using Shooter.Scripts.Settings;
using UnityEngine;
using UnityEngine.UI;

namespace Shooter.Scripts.UI
{
    public class InventoryView : MonoBehaviour
    {
        [SerializeField] private InventorySlotView[] _inventorySlotView;
        [SerializeField] private PlayerInventory _playerInventory;
        private SettingsProvider _settingsProvider;

        private void Awake()
        {
            _settingsProvider = new SettingsProvider();
            _settingsProvider.LoadGameSettings();
            
            Init();
        }

        private void Init()
        {
            for (int i = 0; i < _inventorySlotView.Length; i++)
            {
                _inventorySlotView[i].Init(i, _playerInventory, _settingsProvider.GameSettings.GunsSettings);
            }
        }
    }
    
    
}