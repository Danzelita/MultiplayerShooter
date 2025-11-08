using System.Collections;
using System.Collections.Generic;
using Shooter.Scripts.Data;
using Shooter.Scripts.Multiplayer;
using UnityEngine;

namespace Shooter.Scripts.Gameplay.Characters.Player
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private float _restartDelay;
        [SerializeField] private float _mouseSensativity = 2f;
        [SerializeField] private PlayerCharacter _playerCharacter;
        [SerializeField] private PlayerInventory _playerInventory;

        private readonly Dictionary<string, object> _data = new();
        
        private MultiplayerManager _mupliplayerManager;
        private bool _hold;

        public void Init(MultiplayerManager mupliplayerManager)
        {
            Cursor.lockState = CursorLockMode.Locked;
            _mupliplayerManager = mupliplayerManager;
        }

        private void Update()
        {
            if (_hold) 
                return;
            
            float h = Input.GetAxisRaw("Horizontal");
            float v = Input.GetAxisRaw("Vertical");

            float mouseX = Input.GetAxis("Mouse X");
            float mouseY = Input.GetAxis("Mouse Y");

            bool isSneak = Input.GetKey(KeyCode.LeftShift);
            bool isJuming = Input.GetKey(KeyCode.Space);
            bool isShoot = Input.GetMouseButton(0);

            if (Input.GetKeyDown(KeyCode.Alpha1))
                _playerInventory.SetCurrentGunByIndex(0);
            if (Input.GetKeyDown(KeyCode.Alpha2))
                _playerInventory.SetCurrentGunByIndex(1);
            if (Input.GetKeyDown(KeyCode.Alpha3))
                _playerInventory.SetCurrentGunByIndex(2);
            if (Input.GetKeyDown(KeyCode.E))
                _playerInventory.TryPickUp();
            if (Input.GetKeyDown(KeyCode.Q))
                _playerInventory.TryDrop();

            if (isJuming)
                _playerCharacter.Jump();

            if (isShoot && _playerInventory.TryShootByCurrentGun(out ShootData shootData)) 
                SendShoot(ref shootData);


            _playerCharacter.SetInput(h, v, mouseX * _mouseSensativity);
            _playerCharacter.RotateX(-mouseY * _mouseSensativity);
            _playerCharacter.SetCrouch(isSneak);
            
            SendMove();
            SendCrouch();
            SendCurrentGun(_playerInventory.GetCurrentGunId());
        }

        private void SendCrouch() => 
            _mupliplayerManager.SendToServer("crouch", _playerCharacter.IsCrouch());

        private void SendCurrentGun(string currentId) => 
            _mupliplayerManager.SendToServer("changeGun", currentId);

        private void SendShoot(ref ShootData data)
        {
            data.pKey = _mupliplayerManager.GetClientKey();
            string json = JsonUtility.ToJson(data);
            Debug.Log(JsonUtility.ToJson(data, true));
            _mupliplayerManager.SendToServer("shoot", json);
        }


        private void SendMove()
        {
            _playerCharacter.GetMoveInfo(
                out Vector3 position,
                out Vector3 velocity,
                out float rotationX,
                out float rotationY
                );

            _data[PlayerFields.PositionX] = position.x;
            _data[PlayerFields.PositionY] = position.y;
            _data[PlayerFields.PositionZ] = position.z;

            _data[PlayerFields.VelocityX] = velocity.x;
            _data[PlayerFields.VelocityY] = velocity.y;
            _data[PlayerFields.VelocityZ] = velocity.z;

            _data[PlayerFields.RotationX] = rotationX;
            _data[PlayerFields.RotationY] = rotationY;

            _mupliplayerManager.SendToServer("move", _data);
        }

        public void Restart(string json)
        {
            Vector3Data data = JsonUtility.FromJson<Vector3Data>(json);
            Vector3 resetPosition = data.ToVector3();
            StartCoroutine(HoldProcess());
            
            _playerInventory.TryDrop();
            _playerCharacter.transform.position = resetPosition;
            _playerCharacter.SetInput(0,0,0);
            
            _data[PlayerFields.PositionX] = resetPosition.x;
            _data[PlayerFields.PositionY] = resetPosition.y;
            _data[PlayerFields.PositionZ] = resetPosition.z;
            _data[PlayerFields.VelocityX] = 0;
            _data[PlayerFields.VelocityY] = 0;
            _data[PlayerFields.VelocityZ] = 0;
            _data[PlayerFields.Crouch] = false;
            
            _mupliplayerManager.SendToServer("move", _data);
        }

        private IEnumerator HoldProcess()
        {
            _hold = true;
            yield return new WaitForSecondsRealtime(_restartDelay);
            _hold = false;
        }
    }
}