using Godot;
using ProjectBriseis.Scripts.Maps.Base;

namespace ProjectBriseis.Scripts.Manager;

public enum MatchState {
    WarmUp = 1,
    Playing = 2,
    End = 3
}

public enum Team {
    A = 1,
    B = 2
}

public partial class MatchManager : Node {
    private MatchState _state = MatchState.WarmUp;


    [ExportGroup("Dependencies")]
    [Export]
    private MapManager _mapManager;

    private BaseMap _mapLoaded;

    public override void _Ready() {
        _mapManager.OnServerMapLoaded += instance => _mapLoaded = instance;
    }

    public MatchState CurrentState() {
        return _state;
    }
}