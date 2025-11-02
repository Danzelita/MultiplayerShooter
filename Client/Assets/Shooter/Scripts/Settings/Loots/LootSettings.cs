using Shooter.Scripts.Gameplay.Loot;
using UnityEngine;

namespace Shooter.Scripts.Settings.Loots
{
    [CreateAssetMenu(fileName = "LootSettings", menuName = "GameSettings/Loots/New Loot Settings")]
    public class LootSettings : ScriptableObject
    {
        public string Id;
        public string LootId;
        public LootView Prefab;
    }
}