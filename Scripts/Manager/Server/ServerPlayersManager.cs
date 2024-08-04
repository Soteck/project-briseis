using Godot;
using System.Collections.Generic;
using ProjectBriseis.Scripts.Map.Spawn;

namespace ProjectBriseis.Scripts.Manager.Server;

public struct PlayerInTeam {
    public long Id;
    public Team Team;
    public PlayerSpawnArea SpawnArea;
    public PlayerStatus Status;
}

public partial class ServerPlayersManager : Node {
    public static ServerPlayersManager instance { get; private set; }

    public Dictionary<long, PlayerInTeam> _players { get; private set; }
    
    
    [ExportGroup("Dependencies")]
    [Export]
    private NetworkManager _networkManager;

    public ServerPlayersManager() {
        instance = this;
        _players = new Dictionary<long, PlayerInTeam>();
    }

    public void StartServer() {
        _networkManager.OnPlayerDisconnect += id => _players.Remove(id);
        _networkManager.OnPlayersChange += (id, encoded) => {
            _players[id] = new PlayerInTeam() {
                Id = id,
                Status = PlayerStatus.Dead
            };
        };
    }

    public void joinTeamB() {
        // throw new System.NotImplementedException();
    }

    public void joinTeamA() {
        // throw new System.NotImplementedException();
    }
}