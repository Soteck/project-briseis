using Godot;
using ProjectBriseis.Scripts.AutoLoad;
using ProjectBriseis.Scripts.Maps.Base;

namespace ProjectBriseis.Scripts.Manager;

public partial class MapManager : Node {
    private string _actualMapName = null;
    private BaseMap _actualMap = null;
    

    [Export]
    private Node3D _mapsRoot;

    
    [Signal]
    public delegate void OnServerMapLoadedEventHandler(BaseMap mapInstance);

    public void ServerLoadMap(string mapName) {
        // Log.Rpc("Call to ClientLoadMapRpc: " + mapName);
        // Rpc("ClientLoadMapRpc", mapName);
        // _actualMap = (BaseMap) LoadMap(mapName);
        // EmitSignal(MapManager.SignalName.OnServerMapLoaded, _actualMap);
        // _actualMapName = mapName;
        PackedScene scene = ResourceLoader.Load("res://" + mapName) as PackedScene;
        _actualMap = scene.Instantiate() as BaseMap;
        _mapsRoot.AddChild(_actualMap);

    }

    public void ServerNewPlayerLoadMap(long id) {
        Log.RpcId(id, "Call to client ClientLoadMapRpc: " + _actualMapName);
        RpcId(id, "ClientLoadMapRpc", _actualMapName);
    }


    [Rpc(
            MultiplayerApi.RpcMode.AnyPeer,
            TransferMode = MultiplayerPeer.TransferModeEnum.Reliable,
            TransferChannel = 0)
    ]
    private void ClientLoadMapRpc(string mapName) {
        Log.Info("Client loading map: " + mapName);
        LoadMap(mapName);
    }

    private Node3D LoadMap(string mapName) {
        GlobalStateMachine.instance.Loading();
        Node3D ret = AddMapToScene(mapName);
        GlobalStateMachine.instance.Match();
        return ret;
    }

    private Node3D AddMapToScene(string mapName) {
        GlobalStateMachine.instance.ClearCurrentMap();
        Node3D mapInstance = null;
        string path = "res://" + mapName;
        if (ResourceLoader.Load(path) is PackedScene scene) {
            mapInstance = (Node3D) scene.Instantiate();
            GlobalStateMachine.instance.AttachNewMap(mapInstance);
        } else {
            Log.Error("Map not found " + mapName);
        }

        return mapInstance;
    }

    public BaseMap LoadedMap() {
        return _actualMap;
    }
}