using Godot;

namespace ProjectBriseis.Scripts.AutoLoad.Multiplayer;

public partial class MapLoader : Node {
    private string _actualMap = null;

    public void ServerLoadMap(string mapName) {
        Log.Rpc("Call to ClientLoadMapRpc: " + mapName);
        Rpc("ClientLoadMapRpc", mapName);
        LoadMap(mapName);
        _actualMap = mapName;
    }

    public void ServerNewPlayerLoadMap(long id) {
        Log.RpcId(id, "Call to client ClientLoadMapRpc: " + _actualMap);
        RpcId(id, "ClientLoadMapRpc", _actualMap);
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

    private void LoadMap(string mapName) {
        GlobalStateMachine.instance.Loading();
        AddMapToScene(mapName);
        GlobalStateMachine.instance.Match();
    }

    private void AddMapToScene(string mapName) {
        GlobalStateMachine.instance.ClearCurrentMap();
        string path = "res://" + mapName;
        if (ResourceLoader.Load(path) is PackedScene scene) {
            Node3D sceneInstance = (Node3D) scene.Instantiate();
            GlobalStateMachine.instance.AttachNewMap(sceneInstance);
        } else {
            Log.Error("Map not found " + mapName);
        }
    }
}