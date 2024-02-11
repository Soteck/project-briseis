using System;
using Godot;

namespace ProjectBriseis.Scripts.AutoLoad.Multiplayer;

public partial class ServerManager : Node {

    [Export]
    private string[] _mapRotation = new []{ "DevMap"};
    
    
    [ExportGroup("Dependencies")]
    [Export]
    private MapLoader _mapLoader;
    
    private int _currentRotationMap = 0;
    public void Start() {
        string mapToLoad = _mapRotation[_currentRotationMap];
        _mapLoader.ServerLoadMap(mapToLoad);
    }
}