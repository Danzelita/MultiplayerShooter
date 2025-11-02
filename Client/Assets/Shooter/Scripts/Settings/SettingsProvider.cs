using UnityEngine;

namespace Shooter.Scripts.Settings
{
    public class SettingsProvider
    {
        public GameSettings GameSettings { get; private set; }

        public void LoadGameSettings()
        {
            GameSettings = Resources.Load<GameSettings>("GameSettings");
        }
    }
}