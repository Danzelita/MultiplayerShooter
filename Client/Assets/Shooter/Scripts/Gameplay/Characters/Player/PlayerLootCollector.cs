using Shooter.Scripts.Gameplay.Loot;
using Shooter.Scripts.Logic;
using UnityEngine;

namespace Shooter.Scripts.Gameplay.Characters.Player
{
    public class PlayerLootCollector : MonoBehaviour
    {
        [SerializeField] private PlayerInventory _playerInventory;
        [SerializeField] private TriggerObserver _triggerObserver;
        private LootView _currentLootForPickUp;

        private void Awake()
        {
            _triggerObserver.TriggerEnter += OnTriggerEnter;
            _triggerObserver.TriggerExit += OnTriggerExit;
        }

        public string TryPickUp()
        {
            if (_currentLootForPickUp == null)
                return null;

            return _currentLootForPickUp.PickUp();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.TryGetComponent<LootView>(out var lootView) == false)
                return;

            _currentLootForPickUp = lootView;
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.TryGetComponent<LootView>(out var lootView) == false)
                return;
            
            _currentLootForPickUp = null;
        }
    }
}