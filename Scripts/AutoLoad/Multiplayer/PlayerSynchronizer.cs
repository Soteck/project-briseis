using System.Collections.Generic;
using Godot;
using ProjectBriseis.Scripts.Player;
using ProjectBriseis.Scripts.Util;

namespace ProjectBriseis.Scripts.AutoLoad.Multiplayer;

public partial class PlayerSynchronizer : Node {

    //All the "characters" in game (bodies) included dead and undressed
    private List<Character> _characters = new List<Character>(); 
    
    public void ServerSpawnPlayer(SpawnPlayer playerToSpawn) {
        throw new System.NotImplementedException();
    }
    
    
    


    [Rpc(
            MultiplayerApi.RpcMode.AnyPeer,
            TransferMode = MultiplayerPeer.TransferModeEnum.Reliable,
            TransferChannel = 0)
    ]
    private void ClientLoadMapRpc(string mapName) {
        Log.Info("Client loading map: " + mapName);
        // LoadMap(mapName);
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
}