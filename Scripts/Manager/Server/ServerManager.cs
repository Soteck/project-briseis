using Godot;
using ProjectBriseis.Scripts.AutoLoad.Multiplayer;

namespace ProjectBriseis.Scripts.Manager.Server;

public partial class ServerManager : Node3D {

    [Export]
    private string[] _mapRotation = new []{ "Scenes/Maps/Develop.tscn"};
    
    
    [ExportGroup("Dependencies")]
    [Export]
    private MapManager _mapManager;
    
    [Export]
    private ServerSpawnManager _serverSpawnManager;
    
    [Export]
    private ServerPlayersManager _serverPlayersManager;
    
    private int _currentRotationMap = 0;
    public void StartServer() {
        string mapToLoad = _mapRotation[_currentRotationMap];
        _mapManager.ServerLoadMap(mapToLoad);
        _serverSpawnManager.StartServer();
        _serverPlayersManager.StartServer();
    }
}