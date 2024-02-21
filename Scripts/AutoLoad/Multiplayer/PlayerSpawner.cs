using System.Collections.Generic;
using Godot;
using ProjectBriseis.Scripts.Map.Spawn;
using ProjectBriseis.Scripts.Maps.Base;
using ProjectBriseis.Scripts.Util;

namespace ProjectBriseis.Scripts.AutoLoad.Multiplayer;

public struct PlayerInTeam {
    public long Id;
    public Team Team;
    public PlayerSpawnArea SpawnArea;
    public PlayerStatus Status;
}

public enum PlayerStatus {
    Alive = 1,
    Downed = 2,
    Dead = 3
}

public partial class PlayerSpawner : Node {
    [ExportGroup("Dependencies")]
    [Export]
    private MatchManager _matchManager;

    [Export]
    private MultiplayerAutoLoad _multiplayerAutoLoad;

    [Export]
    private MapLoader _mapLoader;

    private readonly Dictionary<long, PlayerInTeam> _players = new Dictionary<long, PlayerInTeam>();
    
    private PlayerSpawnPool _spawnPool = new();

    public override void _Ready() {
        _multiplayerAutoLoad.OnPlayerDisconnect += id => _players.Remove(id);
        _multiplayerAutoLoad.OnPlayersChange += (id, encoded) => {
            _players[id] = new PlayerInTeam() {
                Id = id,
                Status = PlayerStatus.Dead
            };
        };
    }

    public override void _Process(double delta) {
        BaseMap loadedMap = _mapLoader.LoadedMap();
        if (_matchManager.CurrentState() == MatchState.WarmUp) {
            _ProcessWarmup(loadedMap);
        }

        _ProcessSpawnPool();
    }

    private void _ProcessSpawnPool() {
        if (_spawnPool.IsDirty()) {
            foreach (SpawnPlayer playerToSpawn in _spawnPool.DumpPool()) {
                _ServerSpawnPlayer(playerToSpawn);
            }
        }
    }

    private void _ServerSpawnPlayer(SpawnPlayer playerToSpawn) {
        throw new System.NotImplementedException();
    }

    private void _ProcessWarmup(BaseMap loadedMap) {
        
        foreach (KeyValuePair<long, PlayerInTeam> pair in _players) {
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