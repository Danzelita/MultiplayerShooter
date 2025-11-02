using UnityEngine;

namespace Shooter.Scripts.Gameplay.Characters.Enemy
{
    public class EnemyDamageCollider : MonoBehaviour
    {
        public int DamageMultiplier => _damageMultiplier;
        public EnemyCharacter Character => _character;
        public bool IsSingle => _isSingle;
        
        
        [SerializeField] private bool _isSingle = false;
        [SerializeField] private int _damageMultiplier = 1;
        [SerializeField] private EnemyCharacter _character;
    }
}