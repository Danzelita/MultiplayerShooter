using System.Collections.Generic;
using Colyseus;
using Shooter.Code.Data;
using Shooter.Code.Gameplay.Characters;
using Shooter.Code.Gameplay.Characters.Enemy;
using Shooter.Code.Gameplay.Characters.Player;
using Shooter.Code.Multiplayer.generated;
using UnityEngine;

namespace Shooter.Code.Multiplayer
{
    public class MultiplayerManager : ColyseusManager<MultiplayerManager>
    {
        [SerializeField] private PlayerCharacter _playerPrefab;
        [SerializeField] private EnemyController _enemyPrefab;

        private readonly Dictionary<string, EnemyController> _enemies = new();

        private ColyseusRoom<State> _room;

        protected override void Awake()
        {
            base.Awake();

            Instance.InitializeClient();
            Connect();
        }

        private async void Connect()
        {
            Dictionary<string, object> data = new()
            {
                ["speed"] = _playerPrefab.MaxSpeed,
            };

            _room = await Instance.client.JoinOrCreate<State>("state_handler", data);

            _room.OnStateChange += OnChange;

            _room.OnMessage<string>("Shoot", ApplyShoot);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();

            if (_room == null)
                return;

            _room.OnStateChange -= OnChange;
            _room.State.players.OnAdd -= SpawnEnemy;
            _room.State.players.OnRemove -= DespawnEnemy;

            _room.Leave();
        }

        public void SendMessage(string key, string data) =>
            _room.Send(key, data);

        public void SendMessage(string key, Dictionary<string, object> data) =>
            _room.Send(key, data);

        public string GetClientKey() =>
            _room.SessionId;

        private void OnChange(State state, bool isFirstState)
        {
            if (isFirstState == false)
                return;

            state.players.ForEach((key, player) =>
            {
                if (key == _room.SessionId)
                    SpawnPlayer(key, player);
                else
                    SpawnEnemy(key, player);
            });

            _room.State.players.OnAdd += SpawnEnemy;
            _room.State.players.OnRemove += DespawnEnemy;
        }

        private void DespawnEnemy(string key, Player player)
        {
            if (_enemies.Remove(key, out EnemyController enemyController) == false)
                return;
            enemyController.Destroy();
        }

        private void SpawnPlayer(string key, Player player)
        {
            Character newPlayer = Instantiate(_playerPrefab, GetPlayerSpawnPosition(player), Quaternion.identity);
            newPlayer.GetComponent<PlayerController>()?.Init(mupliplayerManager: this);
        }

        private void SpawnEnemy(string key, Player player)
        {
            EnemyController newEnemy = Instantiate(_enemyPrefab, GetPlayerSpawnPosition(player), Quaternion.identity);
            newEnemy.Init(player);
            
            _enemies.Add(key, newEnemy);
        }

        private void ApplyShoot(string jsonShoot)
        {
            ShootData shootData = JsonUtility.FromJson<ShootData>(jsonShoot);

            if (_enemies.TryGetValue(shootData.pKey, out var enemyController) == false)
            {
                Debug.LogError("EnemeyController not found");
                return;
            }
            enemyController.Shoot(shootData);
        }

        private Vector3 GetPlayerSpawnPosition(Player player) =>
            new(player.pX, 0f, player.pZ);
    }
}