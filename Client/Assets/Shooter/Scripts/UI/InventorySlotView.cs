using System.Linq;
using Shooter.Scripts.Gameplay.Characters.Player;
using Shooter.Scripts.Settings.Guns;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Shooter.Scripts.UI
{
    public class InventorySlotView : MonoBehaviour
    {
        [SerializeField] private Image _icon;
        [SerializeField] private Image _currentImage;
        [SerializeField] private Color _currentColor;
        [SerializeField] private TextMeshProUGUI _text;
        private int _index;
        private GunsSettings _gunsSettings;

        public void Init(int index, PlayerInventory playerInventory, GunsSettings gameSettingsGunsSettings)
        {
            _index = index;
            _gunsSettings = gameSettingsGunsSettings;

            playerInventory.CurrentSlotChanged += PlayerInventoryOnCurrentSlotChanged;
            playerInventory.InventoryChanged += PlayerInventoryOnInventoryChanged;

            _icon.gameObject.SetActive(false);
        }

        private void PlayerInventoryOnInventoryChanged(int index, string gunId)
        {
            if (_index != index)
                return;

            GunSettings gunSettings = _gunsSettings.Guns.FirstOrDefault(g => g.Id == gunId);
            _icon.gameObject.SetActive(gunSettings);
            _icon.sprite = gunSettings?.Icon;
            _text.text = gunId;
        }

        private void PlayerInventoryOnCurrentSlotChanged(int index) =>
            SetCurrent(index == _index);

        private void SetCurrent(bool flag) =>
            _currentImage.color = flag ? _currentColor : Color.white;
    }
}