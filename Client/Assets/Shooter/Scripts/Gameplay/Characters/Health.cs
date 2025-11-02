using Shooter.Scripts.UI;
using UnityEngine;

namespace Shooter.Scripts.Gameplay.Characters
{
    public class Health : MonoBehaviour
    {
        [SerializeField] private HealtBar _healtBar;
        private int _max;
        private int _current;

        public void SetMax(int max)
        {
            _max = max;
            UpdateHealth();
        }

        public void SetCurrent(int current)
        {
            _current = current;
            UpdateHealth();
        }

        public void ApplyDamage(int damage)
        {
            _current -= damage;
            UpdateHealth();
        }

        private void UpdateHealth() =>
            _healtBar.UpdateHealth(_max, _current);
    }
}