using UnityEngine;

namespace Shooter.Scripts.Gameplay.Guns
{
    public class GunAnimation : MonoBehaviour
    {
        [SerializeField] private Gun _gun;
        [SerializeField] private Animator _animator;
    
        private const string Shoot = "Shoot";

        private void Start() => 
            _gun.OnShoot += OnShoot;

        private void OnDestroy() => 
            _gun.OnShoot -= OnShoot;

        private void OnShoot() => 
            _animator.SetTrigger(Shoot);
    }
}
