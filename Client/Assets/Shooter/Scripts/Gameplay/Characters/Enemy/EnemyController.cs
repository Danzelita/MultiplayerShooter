using System;
using System.Collections.Generic;
using System.Linq;
using Colyseus.Schema;
using Shooter.Scripts.Data;
using Shooter.Scripts.Multiplayer;
using Shooter.Scripts.UI;
using UnityEngine;

namespace Shooter.Scripts.Gameplay.Characters.Enemy
{
    public class EnemyController : MonoBehaviour
    {
        [SerializeField] EnemyInventory _enemyInventory;
        [SerializeField] private EnemyCharacter _enemyCharacter;
        [SerializeField] private KillsDisplay _killsDisplay;

        private float AverageInterval => _reciveTimeIntervals.Average();
    
        private  Multiplayer.generated.Player _player;
        private readonly List<float> _reciveTimeIntervals = new() { 0f, 0f, 0f, 0f, 0f };
        private float _lastReciveTime = 0f;

        public void Init(Multiplayer.generated.Player player)
        {
            _player = player;
            _enemyCharacter.SetSpeed(player.speed);
            _enemyCharacter.SetMaxHealth(player.maxHP);
            player.OnChange += OnChange;

        }

        public void Destroy()
        {
            _player.OnChange -= OnChange;
        
            Destroy(gameObject);
        }

        public void Shoot(in ShootData shootData) =>
            _enemyInventory.ShootByData(shootData);

        private void OnChange(List<DataChange> changes)
        {
            SaveReciveTime();   
        
            Vector3 position = _enemyCharacter.TargetPosition;
            Vector3 velocity = _enemyCharacter.Velocity;

            foreach (DataChange change in changes)
            {
                switch (change.Field)
                {
                    case "currentGun":
                        _enemyInventory.SetCurrentGun((string)change.Value);
                        break;
                    case "kills":
                        _killsDisplay.SetKills((UInt16)change.Value);
                        break;
                    case "currentHP":
                        _enemyCharacter.RestoreHP((sbyte)change.Value);
                        break;
                    case PlayerFields.PositionX:
                        position.x = (float)change.Value;
                        break;
                    case PlayerFields.PositionY:
                        position.y = (float)change.Value;
                        break;
                    case PlayerFields.PositionZ:
                        position.z = (float)change.Value;
                        break;

                    case PlayerFields.VelocityX:
                        velocity.x = (float)change.Value;
                        break;
                    case PlayerFields.VelocityY:
                        velocity.y = (float)change.Value;
                        break;
                    case PlayerFields.VelocityZ:
                        velocity.z = (float)change.Value;
                        break;
                    case PlayerFields.RotationX:
                        _enemyCharacter.SetRotationX((float)change.Value);
                        break;
                    case PlayerFields.RotationY:
                        _enemyCharacter.SetRotationY((float)change.Value);
                        break;
                    case PlayerFields.Crouch:
                        _enemyCharacter.SetCrouch((bool)change.Value);
                        break;
                    default:
                        Debug.Log("Unknown field: " + change.Field);
                        break;
                }
            }

            _enemyCharacter.SetMovement(position, velocity, Mathf.Min(AverageInterval, 0.1f));
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