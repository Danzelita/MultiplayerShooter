using UnityEngine;

namespace Shooter.Scripts.UI
{
    public class HealtBar : MonoBehaviour
    {
        [SerializeField] private RectTransform _filledImage;
        [SerializeField] private float _defaultWidth;

        private void OnValidate()
        {
            _defaultWidth = _filledImage.sizeDelta.x;
        }

        public void UpdateHealth(int max, int current)
        {
            float percent = current / (float)max;
            _filledImage.sizeDelta = new Vector2(_defaultWidth * percent, _filledImage.sizeDelta.y);
        }
    }
}