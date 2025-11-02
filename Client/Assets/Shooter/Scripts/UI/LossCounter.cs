using TMPro;
using UnityEngine;

namespace Shooter.Scripts.UI
{
    public class LossCounter : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _text;
        private int _playerLoss;
        private int _enemyLoss;

        public void SetPlayerLoss(int value)
        {
            _playerLoss = value;
            UpdateText();
        }

        public void SetEnemyLoss(int value)
        {
            _enemyLoss = value;
            UpdateText();
        }

        private void UpdateText()
        {
            _text.text = $"{_enemyLoss} : {_playerLoss}";
        }
    }
}