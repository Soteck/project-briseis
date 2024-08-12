using Godot;
using ProjectBriseis.Scripts.Manager.Server;
using ProjectBriseis.Scripts.Maps.Base;

namespace ProjectBriseis.Scripts.Manager;

public enum MatchState {
    WarmUp = 1,
    Playing = 2,
    End = 3
}

public enum Team {
    A = 1,
    B = 2,
    Spectator = 3
}

public partial class MatchManager : Node {
    private MatchState _state = MatchState.WarmUp;


    [ExportGroup("Dependencies")]
    [Export]
    private MapManager _mapManager;

    [Export]
    private ServerManager _serverManager;

    private BaseMap _mapLoaded;
    
    [Export]
    public double MapTime;

    public override void _Process(double delta) {
        if (_serverManager?.state == ServerState.Started && Multiplayer.IsServer()) {
            MapTime += delta;
        }
    }

    public override void _Ready() {
        _mapManager.OnServerMapLoaded += instance => _mapLoaded = instance;
        MapTime = 0;
    }

    public MatchState CurrentState() {
        return _state;
    }
}