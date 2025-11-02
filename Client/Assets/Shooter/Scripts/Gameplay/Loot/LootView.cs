using System;
using System.Collections.Generic;
using Colyseus.Schema;
using Shooter.Scripts.Settings.Loots;
using UnityEngine;

namespace Shooter.Scripts.Gameplay.Loot
{
    public class LootView : MonoBehaviour
    {
        private string _lootId;
        private Action _onPickup;
        private Multiplayer.generated.Loot _loot;

        public void Init(Multiplayer.generated.Loot loot, LootSettings lootSettings, Action onPickup)
        {
            _lootId = lootSettings.LootType;
            _loot = loot;
            _onPickup = onPickup;
            
            _loot.OnChange += OnChange;
        }

        private void OnChange(List<DataChange> changes)
        {
        }

        public string PickUp()
        {
            _onPickup?.Invoke();
            gameObject.SetActive(false);
            return _lootId;
        }

        public void Destroy()
        {
            _loot.OnChange -= OnChange;
            
            Destroy(gameObject);
        }
    }
}