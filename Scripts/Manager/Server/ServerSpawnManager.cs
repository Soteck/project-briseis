using System.Collections.Generic;
using Godot;
using ProjectBriseis.Scripts.Map.Spawn;
using ProjectBriseis.Scripts.Maps.Base;
using ProjectBriseis.Scripts.Util;

namespace ProjectBriseis.Scripts.Manager.Server;

public enum PlayerStatus {
    Alive = 1,
    Downed = 2,
    Dead = 3
}

public partial class ServerSpawnManager : Node {
    
    public static ServerSpawnManager instance { get; private set; }
    
    [ExportGroup("Dependencies")]
    [Export]
    private MatchManager _matchManager;

    [Export]
    private NetworkManager _networkManager;
    
    [Export]
    private MapManager _mapManager;
    
    [Export]
    private ServerPlayersManager _serverPlayersManager;


    private readonly PlayerSpawnPool _spawnPool = new();

    public ServerSpawnManager() {
        instance = this;
    }

    
    public void StartServer() {
        
    }

    public override void _Process(double delta) {
        if (!Multiplayer.IsServer()) return;

        BaseMap loadedMap = _mapManager.LoadedMap();
        if (_matchManager.CurrentState() == MatchState.WarmUp) {
            _ProcessWarmup(loadedMap);
        }
        
        _ProcessSpawnPool();
    }

    private void _SpawnPlayer(SpawnPlayer playerToSpawn) {
        
    }

    private void _ProcessSpawnPool() {
        if (!_spawnPool.IsDirty()) return;

        foreach (SpawnPlayer playerToSpawn in _spawnPool.DumpPool()) {
            _SpawnPlayer(playerToSpawn);
        }
    }


    private void _ProcessWarmup(BaseMap loadedMap) {
        foreach (KeyValuePair<long, PlayerInTeam> pair in _serverPlayersManager._players) {
            PlayerInTeam playerInTeam = pair.Value;
            if (playerInTeam.Status != PlayerStatus.Dead) continue;
            PlayerSpawnArea spawnArea = playerInTeam.SpawnArea;

            if (spawnArea == null) {
                spawnArea = playerInTeam.Team == Team.A
                    ? loadedMap.DefaultTeamASpawnArea()
                    : loadedMap.DefaultTeamBSpawnArea();
            }


            _spawnPool.AddPlayerToPool(spawnArea, playerInTeam);
        }
    }
}