using System;
using Godot;

namespace ProjectBriseis.Scripts.AutoLoad.Multiplayer;

public partial class ServerManager : Node {

    [Export]
    private string[] _mapRotation = new []{ "Scenes/Maps/Develop.tscn"};
    
    
    [ExportGroup("Dependencies")]
    [Export]
    private MapLoader _mapLoader;
    
    private int _currentRotationMap = 0;
    public void StartServer() {
        string mapToLoad = _mapRotation[_currentRotationMap];
        _mapLoader.ServerLoadMap(mapToLoad);
    }
}