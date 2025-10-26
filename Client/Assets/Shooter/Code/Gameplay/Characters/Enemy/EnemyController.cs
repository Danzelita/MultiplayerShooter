using System.Collections.Generic;
using System.Linq;
using Colyseus.Schema;
using Shooter.Code.Data;
using Shooter.Code.Gameplay.Guns.Enemy;
using Shooter.Code.Multiplayer;
using UnityEngine;

namespace Shooter.Code.Gameplay.Characters.Enemy
{
    public class EnemyController : MonoBehaviour
    {
        [SerializeField] private EnemyCharacter _enemyCharacter;
        [SerializeField] private EnemyGun _gun;

        private float AverageInterval => _reciveTimeIntervals.Average();
    
        private  Multiplayer.generated.Player _player;
        private readonly List<float> _reciveTimeIntervals = new() { 0f, 0f, 0f, 0f, 0f };
        private float _lastReciveTime = 0f;

        public void Init(Multiplayer.generated.Player player)
        {
            _player = player;
            _enemyCharacter.SetSpeed(player.speed);
            player.OnChange += OnChange;
        }

        public void Destroy()
        {
            _player.OnChange -= OnChange;
        
            Destroy(gameObject);
        }

        public void Shoot(in ShootData shootData) => 
            _gun.Shoot(shootData.Pos.ToVector3(), shootData.Vel.ToVector3());

        private void OnChange(List<DataChange> changes)
        {
            SaveReciveTime();
        
            Vector3 position = _enemyCharacter.TargetPosition;
            Vector3 velocity = _enemyCharacter.Velocity;

            foreach (DataChange change in changes)
            {
                switch (change.Field)
                {
                    case NetworkFields.PositionX:
                        position.x = (float)change.Value;
                        break;
                    case NetworkFields.PositionY:
                        position.y = (float)change.Value;
                        break;
                    case NetworkFields.PositionZ:
                        position.z = (float)change.Value;
                        break;

                    case NetworkFields.VelocityX:
                        velocity.x = (float)change.Value;
                        break;
                    case NetworkFields.VelocityY:
                        velocity.y = (float)change.Value;
                        break;
                    case NetworkFields.VelocityZ:
                        velocity.z = (float)change.Value;
                        break;
                    case NetworkFields.RotationX:
                        _enemyCharacter.SetRotationX((float)change.Value);
                        break;
                    case NetworkFields.RotationY:
                        _enemyCharacter.SetRotationY((float)change.Value);
                        break;
                    case NetworkFields.Sneaking:
                        _enemyCharacter.SetSneak((bool)change.Value);
                        break;
                    default:
                        Debug.Log("Unknown field: " + change.Field);
                        break;
                }
            }

            _enemyCharacter.SetMovement(position, velocity, AverageInterval);
        }

        private void SaveReciveTime()
        {
            float interval = Time.time - _lastReciveTime;
            _lastReciveTime = Time.time;
        
            _reciveTimeIntervals.Add(interval);
            _reciveTimeIntervals.RemoveAt(0);
        }
    }
}