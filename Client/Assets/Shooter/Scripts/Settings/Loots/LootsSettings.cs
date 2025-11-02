using System.Collections.Generic;
using UnityEngine;

namespace Shooter.Scripts.Settings.Loots
{
    [CreateAssetMenu(fileName = "LootsSettings", menuName = "GameSettings/Loots/New Loots Settings")]
    public class LootsSettings : ScriptableObject
    {
        public List<LootSettings> Loots;
    }
}