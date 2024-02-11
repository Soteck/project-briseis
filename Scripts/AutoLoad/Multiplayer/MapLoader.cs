using Godot;

namespace ProjectBriseis.Scripts.AutoLoad.Multiplayer;

public partial class MapLoader : Node {
    
    public void ServerLoadMap(string mapName) {
        Log.Info("Server Loading map: " + mapName);
        Rpc("ClientLoadMapRpc", mapName);
    }


    [Rpc(
            MultiplayerApi.RpcMode.AnyPeer,
            TransferMode = MultiplayerPeer.TransferModeEnum.Reliable,
            TransferChannel = 0)
    ]
    private void ClientLoadMapRpc(string mapName) {
        Log.Info("Client loading map: " + mapName);
        //get_tree().change_scene_to_file(game_scene_path)
    }
}