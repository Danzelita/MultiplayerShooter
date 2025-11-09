using Shooter.Scripts.Settings.Skins;
using UnityEngine;

namespace Shooter.Scripts.Gameplay.Characters
{
    public class SkinDisplay : MonoBehaviour
    {
        [SerializeField] private MeshRenderer[] _meshRenderers;
        private SkinsSettings _skinsSettings;

        public void Init(SkinsSettings skinsSettings)
        {
            _skinsSettings = skinsSettings;
        }

        public void SetSkin(int index)
        {
            Debug.Log($"SetSkin {index}");
            
            foreach (MeshRenderer meshRenderer in _meshRenderers) 
                meshRenderer.material = _skinsSettings.Skins[index].SkinMaterial;
        }
    }
}