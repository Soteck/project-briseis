using System.Collections.Generic;
using Godot;
using ProjectBriseis.objects.Logic;

namespace ProjectBriseis.Scripts.AutoLoad {
    public partial class MultiplayerAutoLoad : Singleton<MultiplayerAutoLoad> {
        private const string DEFAULT_SERVER_IP = "127.0.0.1";
        private const int PORT = 27960;
        private const int MAX_CONNECTIONS = 24;

        private Dictionary<int, string> players = new Dictionary<int, string>();

        [Signal]
        public delegate void OnPlayerConnectedEventHandler(long peerId, string playerInfo);

        public override void _SingletonReady() {
            Multiplayer.PeerConnected += PeerConnected;
            Multiplayer.PeerDisconnected += PeerDisconnected;
            Multiplayer.ConnectedToServer += ConnectedToServer;
            Multiplayer.ConnectionFailed += ConnectionFailed;
        }

        private void ConnectionFailed() {
            Log.Info("Connection failed");
        }

        private void ConnectedToServer() {
            Log.Info("Connected to the server");
        }

        private void PeerDisconnected(long id) {
            Log.Info("Peer with id " + id + " disconnected");
        }

        private void PeerConnected(long id) {
            Log.Info("Peer with id " + id + " conneected");
        }

        public void Connect(string address) {
            if (address == null || address.Length < 1) {
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


            players[1] = "test1";
            EmitSignal(SignalName.OnPlayerConnected, 1, players[1]);
        }

        public void Disconnect() {
            throw new System.NotImplementedException();
        }
    }
}