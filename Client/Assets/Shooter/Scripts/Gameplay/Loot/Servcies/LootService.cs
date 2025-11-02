using System.Collections.Generic;
using System.Linq;
using Colyseus.Schema;
using Shooter.Scripts.Multiplayer;
using Shooter.Scripts.Settings.Loots;
using UnityEngine;

namespace Shooter.Scripts.Gameplay.Loot.Servcies
{
    public class LootService
    {
        private Dictionary<string, LootView> _lootsMap = new();
        private Dictionary<string, LootSettings> _lootSettingsMap = new();
        private readonly MultiplayerManager _multiplayerManager;
        private readonly LootsSettings _lootsSettings;
        
        private Dictionary<string, object> _dropData = new();

        public LootService(MultiplayerManager multiplayerManager,LootsSettings lootsSettings)
        {
            _multiplayerManager = multiplayerManager;
            _lootsSettings = lootsSettings;
            
            foreach (var lootSettings in lootsSettings.Loots)
                _lootSettingsMap[lootSettings.Type] = lootSettings;
        }
        public void Init(MapSchema<Multiplayer.generated.Loot> stateLoots)
        {
            stateLoots.ForEach(CreateLoot);

            stateLoots.OnAdd += CreateLoot;
            stateLoots.OnRemove += RemoveLoot;
        }

        public void RemoveLoot(string key, Multiplayer.generated.Loot loot)
        {
            
            if (_lootsMap.TryGetValue(key, out LootView lootView) == false)
                return;

            lootView.Destroy();
            _lootsMap.Remove(key);
        }

        public void CreateLoot(string key, Multiplayer.generated.Loot loot)
        {
            LootSettings lootSettings = _lootSettingsMap[loot.type];
            
            LootView lootView = Object.Instantiate(lootSettings.Prefab, GetSpawnPosition(loot), Quaternion.identity);
            _lootsMap[key] = lootView;
            
            lootView.Init(loot, lootSettings, onPickup: () =>
            {
                _multiplayerManager.SendToServer("pickUp", key);
            });
        }

        public void DropByLootTypeAtPosition(string lootType, Vector3 at)
        {
            LootSettings lootSettings = _lootsSettings.Loots.First(l => l.LootType == lootType);
            
            _dropData["type"] = lootSettings.Type;
            _dropData["pX"] = at.x;
            _dropData["pY"] = at.y;
            _dropData["pZ"] = at.z;
            
            _multiplayerManager.SendToServer("drop", _dropData);
        }

        private Vector3 GetSpawnPosition(Multiplayer.generated.Loot loot) => 
            new(loot.pX, loot.pY + 1f, loot.pZ);
    }
}