using Shooter.Scripts.Settings.Guns;
using Shooter.Scripts.Settings.Loots;
using Shooter.Scripts.Settings.Skins;
using UnityEngine;

namespace Shooter.Scripts.Settings
{
    [CreateAssetMenu(fileName = "GameSettings", menuName = "GameSettings/New Game Settings")]
    public class GameSettings : ScriptableObject
    {
        public GunsSettings GunsSettings;
        public LootsSettings LootsSettings;
        public SkinsSettings SkinsSettings;
    }
}