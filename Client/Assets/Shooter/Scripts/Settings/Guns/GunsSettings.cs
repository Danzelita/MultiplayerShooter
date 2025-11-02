using System.Collections.Generic;
using UnityEngine;

namespace Shooter.Scripts.Settings.Guns
{
    [CreateAssetMenu(fileName = "GunsSettings", menuName = "GameSettings/Guns/New Guns Settings")]
    public class GunsSettings : ScriptableObject
    {
        public List<GunSettings> Guns;
    }
}