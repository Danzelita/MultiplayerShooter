using System.Collections.Generic;
using Shooter.Code.Data;
using Shooter.Code.Gameplay.Guns.Player;
using Shooter.Code.Multiplayer;
using UnityEngine;

namespace Shooter.Code.Gameplay.Characters.Player
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private float _mouseSensativity = 2f;
        [SerializeField] private PlayerCharacter _playerCharacter;
        [SerializeField] private PlayerGun _gun;

        private readonly Dictionary<string, object> _data = new();
        
        private MultiplayerManager _mupliplayerManager;

        public void Init(MultiplayerManager mupliplayerManager) => 
            _mupliplayerManager = mupliplayerManager;

        private void Update()
        {
            float h = Input.GetAxisRaw("Horizontal");
            float v = Input.GetAxisRaw("Vertical");

            float mouseX = Input.GetAxis("Mouse X");
            float mouseY = Input.GetAxis("Mouse Y");

            bool isSneak = Input.GetKey(KeyCode.LeftControl);
            bool isJuming = Input.GetKey(KeyCode.Space);
            bool isShoot = Input.GetMouseButton(0);


            if (isJuming)
                _playerCharacter.Jump();

            if (isShoot && _gun.TryShoot(out ShootData shootData)) 
                SendShoot(ref shootData);


            _playerCharacter.SetInput(h, v, mouseX * _mouseSensativity);
            _playerCharacter.RotateX(-mouseY * _mouseSensativity);
            _playerCharacter.SetSneak(isSneak);
            
            SendMove();
        }

        private void SendShoot(ref ShootData data)
        {
            data.pKey = _mupliplayerManager.GetClientKey();
            string json = JsonUtility.ToJson(data);
            _mupliplayerManager.SendMessage("shoot", json);
        }


        private void SendMove()
        {
            _playerCharacter.GetMoveInfo(
                out Vector3 position,
                out Vector3 velocity,
                out float rotationX,
                out float rotationY,
                out bool isSneak
                );

            _data[NetworkFields.PositionX] = position.x;
            _data[NetworkFields.PositionY] = position.y;
            _data[NetworkFields.PositionZ] = position.z;

            _data[NetworkFields.VelocityX] = velocity.x;
            _data[NetworkFields.VelocityY] = velocity.y;
            _data[NetworkFields.VelocityZ] = velocity.z;

            _data[NetworkFields.RotationX] = rotationX;
            _data[NetworkFields.RotationY] = rotationY;

            _data[NetworkFields.Sneaking] = isSneak;

            _mupliplayerManager.SendMessage("move", _data);
        }
    }
}