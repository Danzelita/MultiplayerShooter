using System.Collections.Generic;
using Colyseus;
using Shooter.Scripts.Data;
using Shooter.Scripts.Gameplay.Characters;
using Shooter.Scripts.Gameplay.Characters.Enemy;
using Shooter.Scripts.Gameplay.Characters.Player;
using Shooter.Scripts.Gameplay.Guns.Services;
using Shooter.Scripts.Gameplay.Loot.Servcies;
using Shooter.Scripts.Multiplayer.generated;
using Shooter.Scripts.Settings;
using Shooter.Scripts.UI;
using UnityEngine;

namespace Shooter.Scripts.Multiplayer
{
    public class MultiplayerManager : ColyseusManager<MultiplayerManager>
    {
        [SerializeField] private PlayerCharacter _playerPrefab;
        [SerializeField] private EnemyController _enemyPrefab;

        private readonly Dictionary<string, EnemyController> _enemies = new();

        private ColyseusRoom<State> _room;
        private LootService _lootService;
        private SettingsProvider _settingsProvider;

        protected override void Awake()
        {
            base.Awake();
            
            _settingsProvider = new SettingsProvider();
            _settingsProvider.LoadGameSettings();

            Instance.InitializeClient();
            Connect();
        }

        private async void Connect()
        {
            Dictionary<string, object> data = new()
            {
                ["speed"] = _playerPrefab.MaxSpeed,
                ["hp"] =  _playerPrefab.MaxHealth,
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

        public void SendToServer(string key, bool value) =>
            _room.Send(key, value);
        public void SendToServer(string key, string data) =>
            _room.Send(key, data);

        public void SendToServer(string key, Dictionary<string, object> data) =>
            _room.Send(key, data);

        public string GetClientKey() =>
            _room.SessionId;

        private void OnChange(State state, bool isFirstState)
        {
            if (isFirstState == false)
                return;
            
            _lootService = new LootService(this, _settingsProvider.GameSettings.LootsSettings);
            _lootService.Init(state.loots);

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
            PlayerController playerController = newPlayer.GetComponent<PlayerController>();
            playerController.Init(mupliplayerManager: this);
            newPlayer.GetComponent<PlayerCharacter>().Init(player);
            newPlayer.GetComponent<PlayerInventory>().Init(_settingsProvider, new GunFactory(), _lootService);

            SkinDisplay skinDisplay = newPlayer.GetComponent<SkinDisplay>();
            skinDisplay.Init(_settingsProvider.GameSettings.SkinsSettings);
            skinDisplay.SetSkin(player.skin);
            
            _room.OnMessage<string>("Restart", playerController.Restart);
        }

        private void SpawnEnemy(string key, Player player)
        {
            EnemyController newEnemy = Instantiate(_enemyPrefab, GetPlayerSpawnPosition(player), Quaternion.identity);
            newEnemy.GetComponent<EnemyCharacter>()?.Init(multiplayerManager: this, key);
            newEnemy.Init(player);
            
            SkinDisplay skinDisplay = newEnemy.GetComponent<SkinDisplay>();
            skinDisplay.Init(_settingsProvider.GameSettings.SkinsSettings);
            skinDisplay.SetSkin(player.skin);
            
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
            new(player.pX, player.pY, player.pZ);
    }
}