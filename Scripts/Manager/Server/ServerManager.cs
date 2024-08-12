using Godot;
using ProjectBriseis.Scripts.AutoLoad.Multiplayer;

namespace ProjectBriseis.Scripts.Manager.Server;

public enum ServerState {
    Idle = 0,
    Started = 1
}

public partial class ServerManager : Node3D {
    
    public ServerState state { get; private set; } = ServerState.Idle;

    [Export]
    private string[] _mapRotation = new[] {"Scenes/Maps/Develop.tscn"};


    [ExportGroup("Dependencies")]
    [Export]
    private MapManager _mapManager;

    [Export]
    private ServerSpawnManager _serverSpawnManager;

    [Export]
    private ServerPlayersManager _serverPlayersManager;

    private int _currentRotationMap = 0;

    [Signal]
    public delegate void OnStatechangeEventHandler(ServerState newState, ServerState oldState);
    public void StartServer() {
        string mapToLoad = _mapRotation[_currentRotationMap];
        _mapManager.ServerLoadMap(mapToLoad);
        _serverSpawnManager.StartServer();
        _serverPlayersManager.StartServer();
        TransitionState(ServerState.Started);
    }
    
    
    private void TransitionState(ServerState newState) {
        ServerState oldState = state;
        state = newState;

        EmitSignal(SignalName.OnStatechange, (int) newState, (int) oldState);
    }

}