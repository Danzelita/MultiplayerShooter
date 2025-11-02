using System;
using TMPro;
using UnityEngine;

namespace Shooter.Scripts.UI
{
    public class KillsDisplay : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _text;

        public void SetKills(UInt16 kills) => 
            _text.text = kills.ToString();
    }
}