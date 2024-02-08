using System;
using System.Collections.Generic;
using Godot;
using ProjectBriseis.objects.Logic;

namespace ProjectBriseis.Scripts.AutoLoad {

    public struct PlayerConnection {
        public long Id;
        public GlobalStates Status;
        public string Nickname;
    }
    public partial class MultiplayerAutoLoad : Singleton<MultiplayerAutoLoad> {
        private const string DEFAULT_SERVER_IP = "127.0.0.1";
        private const int PORT = 27960;
        private const int MAX_CONNECTIONS = 24;

        private Dictionary<long, PlayerConnection> _players = new Dictionary<long, PlayerConnection>();

        [Signal]
        public delegate void OnPlayersChangeEventHandler(long id, string playerInfoEncoded);

        [Signal]
        public delegate void OnPlayerDisconnectEventHandler(long id);

        public override void _SingletonReady() {
            Multiplayer.PeerConnected += ServerPeerConnected;
            Multiplayer.PeerDisconnected += ServerPeerDisconnected;
            Multiplayer.ConnectedToServer += ClientConnectedToServer;
            Multiplayer.ConnectionFailed += ClientConnectionFailed;
            OnPlayersChange += ServerNotifyChange; 
            OnPlayerDisconnect += id => ServerNotifyChange(id, null);
        }

        private void ServerNotifyChange(long id, string playerInfo) {
            Rpc("ClientOnPlayersChange", id, playerInfo);
        }
        
        private void ClientConnectionFailed() {
            Log.Info("Client connection failed");
        }

        private void ClientConnectedToServer() {
            Log.Info("Client connected to the server");
        }

        private void ServerPeerDisconnected(long id) {
            Log.Info("Server: Peer with id " + id + " disconnected");
            _players.Remove(id);
            EmitSignal(SignalName.OnPlayerDisconnect, id);
        }

        private void ServerPeerConnected(long id) {
            Log.Info("Server: Peer with id " + id + " connected");
            RpcId(id, "ClientSendInformation");
        }

        public void Connect(string address) {
            if (string.IsNullOrEmpty(address)) {
                address = DEFAULT_SERVER_IP;
            }

            ENetMultiplayerPeer peer = new ENetMultiplayerPeer();
            Error error = peer.CreateClient(address, PORT);
            if (error != Error.Ok) {
                Log.Error("Error connecting to the server " + address + ":" + PORT + ". " + error);
            }

            Multiplayer.MultiplayerPeer = peer;
        }

        public void Host() {
            ENetMultiplayerPeer peer = new ENetMultiplayerPeer();
            var error = peer.CreateServer(PORT, MAX_CONNECTIONS);
            if (error != Error.Ok) {
                Log.Error("Error creating the server " + MAX_CONNECTIONS + " players at port: " + PORT + ". " + error);
            }

            Multiplayer.MultiplayerPeer = peer;
            
            const long id = 1;
            PlayerConnection connection = new() {
                Id = id,
                Nickname = ConfigurationAutoLoad.playerName,
                Status = GlobalStateMachine.instance.CurrentState()
            };

            EmitSignal(SignalName.OnPlayersChange, id, EncodePlayerConnection(connection));
        }

        public void Disconnect() {
            throw new System.NotImplementedException();
        }

        public void LoadMap(string mapName) {
            if (!Multiplayer.IsServer()) {
                Log.Error("Cannot load map. Host server is not running.");
                return;
            }
            GlobalStateMachine.instance.ServerLoadMap(mapName);

        }

        public List<PlayerConnection> GetPlayers() {
            List<PlayerConnection> ret = new();
            foreach (KeyValuePair<long,PlayerConnection> pair in _players) {
                ret.Add(pair.Value);
            }

            return ret;
        }
        
        

        [Rpc(
                MultiplayerApi.RpcMode.AnyPeer,
                TransferMode = MultiplayerPeer.TransferModeEnum.Reliable,
                TransferChannel = 0)
        ]
        private void ClientOnPlayersChange(long id, string connectionStr) {
            if (connectionStr == null) {
                _players.Remove(id);
            }else
            {
                PlayerConnection connection = DecodePlayerConnection(connectionStr);
                _players[id] = connection;
            }
            Log.Info("Client is notified about a players count change: " + id);
        }
        
        

        [Rpc(
                MultiplayerApi.RpcMode.AnyPeer,
                TransferMode = MultiplayerPeer.TransferModeEnum.Reliable,
                TransferChannel = 0)
        ]
        private void ClientSendInformation() {
            PlayerConnection connection = new() {
                Id = Multiplayer.GetUniqueId(),
                Nickname = ConfigurationAutoLoad.playerName,
                Status = GlobalStateMachine.instance.CurrentState()
            };
            Rpc("ServerReceivePlayerInformation", EncodePlayerConnection(connection));
        }
        
        [Rpc(
                MultiplayerApi.RpcMode.AnyPeer,
                TransferMode = MultiplayerPeer.TransferModeEnum.Reliable,
                TransferChannel = 0)
        ]
        private void ServerReceivePlayerInformation(string connectionStr) {
            if (connectionStr != null) {
                PlayerConnection connection = DecodePlayerConnection(connectionStr);
                _players[connection.Id] = connection;
                EmitSignal(SignalName.OnPlayersChange, EncodePlayerConnection(connection));
            }
            Log.Info("Server received update of a player info change: " + connectionStr);
        }

        private string EncodePlayerConnection(PlayerConnection connection) {
            return connection.Id + ";" + connection.Nickname + ";" + (int) connection.Status;
        }

        private PlayerConnection DecodePlayerConnection(string connectionStr) {
            string[] parts = connectionStr.Split(";");
            PlayerConnection connection = new() {
                Id = (long) Convert.ToDouble(parts[0]),
                Nickname = parts[1],
                Status = (GlobalStates) Convert.ToDouble(parts[2])
            };
            return connection;
        }
    }
}