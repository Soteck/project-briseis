using System;
using System.Collections.Generic;
using Godot;
using ProjectBriseis.objects.Logic;
using ProjectBriseis.Scripts.AutoLoad;
using ProjectBriseis.Scripts.AutoLoad.Multiplayer;

namespace ProjectBriseis.Scripts.Manager;

public partial class NetworkManager : Node3D {
    //private Player = ResourceLoader.Load(path) is PackedScene scene
    private const string DEFAULT_SERVER_IP = "127.0.0.1";
    private const int PORT = 27960;
    private const int MAX_CONNECTIONS = 24;

    private readonly Dictionary<long, PlayerConnection> _players = new Dictionary<long, PlayerConnection>();
    private long _myMultiplayerId;

    public static NetworkManager instance { get; private set; }
    
    [Signal]
    public delegate void OnPlayersChangeEventHandler(long id, string playerInfoEncoded);

    [Signal]
    public delegate void OnPlayerDisconnectEventHandler(long id);

    [Signal]
    public delegate void OnClientStatusChangeEventHandler(bool connected);

    [ExportGroup("Dependencies")]
    // [Export]
    // private MatchManager _matchManager;
    //
    // [Export]
    // private ServerSpawnManager _serverSpawnManager;
    //
    // [Export]
    // private MapLoader _mapLoader;
    
    [Export]
    private Server.ServerManager _serverManager;

    [Export]
    private Node3D _playersRoot;

    //---
    private PackedScene _playerScene;
    public bool clientConnectionStatus { private set; get; } = false;

    public override void _Ready() {
        Multiplayer.PeerConnected += PeerConnected;
        Multiplayer.PeerDisconnected += ServerPeerDisconnected;
        Multiplayer.ConnectedToServer += ClientConnectedToServer;
        Multiplayer.ConnectionFailed += ClientConnectionFailed;
        Multiplayer.ServerDisconnected += ClientDisconnectedFromServer;
        OnPlayersChange += ServerRegisterNotifyChange;
        OnPlayerDisconnect += id => ServerRegisterNotifyChange(id, null);
        ConfigurationAutoLoad.instance.OnConfigurationChange += OnMyConfigurationChanged;
        GlobalStateMachine.instance.OnStatechange += OnClientStateChanged;
        _playerScene = ResourceLoader.Load("res://Prefabs/Player/Player.tscn") as PackedScene;
        NetworkManager.instance = this;
    }

    private void PeerConnected(long id) {
        if (Multiplayer.IsServer() && id != 1) {
            ServerPeerConnected(id);
        }
    }

    private void OnMyConfigurationChanged(string[] key, Variant value) {
        if (key is {Length: 2}) {
            if (key[0] == "Player" && key[1] == "Name") {
                if (Multiplayer.IsServer()) {
                    PlayerConnection playerConnection = _players[_myMultiplayerId];
                    playerConnection.Nickname = ConfigurationAutoLoad.playerName;
                    _players[_myMultiplayerId] = playerConnection;
                    EmitSignal(NetworkManager.SignalName.OnPlayersChange, _myMultiplayerId,
                               EncodePlayerConnection(playerConnection));
                } else {
                    ClientSendInformation();
                }
            }
        }
    }

    private void OnClientStateChanged(GlobalStates newstate, GlobalStates oldstate) {
        if (GlobalStateMachine.instance.NetworkRunning()) {
            ClientSendInformation();
        }
    }

    private void ServerRegisterNotifyChange(long id, string playerInfo) {
        _players[id] = DecodePlayerConnection(playerInfo);
        Log.RpcId(id, "ClientOnPlayersChange" + playerInfo);
        Rpc("ClientOnPlayersChange", id, playerInfo);
    }

    private void ClientConnectionFailed() {
        Log.Info("Client connection failed");
    }

    private void ClientConnectedToServer() {
        Log.Info("Client connected to the server");
        clientConnectionStatus = true;
        EmitSignal(NetworkManager.SignalName.OnClientStatusChange, true);
    }

    private void ClientDisconnectedFromServer() {
        Log.Info("Client connected from the server");
        clientConnectionStatus = false;
        EmitSignal(NetworkManager.SignalName.OnClientStatusChange, false);
    }

    private void ServerPeerDisconnected(long id) {
        Log.Info("Server: Peer with id " + id + " disconnected");
        _players.Remove(id);
        EmitSignal(NetworkManager.SignalName.OnPlayerDisconnect, id);
    }

    private void ServerPeerConnected(long id) {
        Log.Info("Server: Peer with id " + id + " connected");

        AddPlayer(id);

        // PlayerConnection connection = new() {
        //     Id = id,
        //     Status = GlobalStates.Connecting
        // };
        //
        // EmitSignal(NetworkManager.SignalName.OnPlayersChange, id, EncodePlayerConnection(connection));
        // Log.RpcId(id, "Call to client ClientSendInformation");
        // RpcId(id, "ClientSendInformation");
        // _mapLoader.ServerNewPlayerLoadMap(id);
        //
        // foreach (KeyValuePair<long, PlayerConnection> playerConnection in _players) {
        //     Log.RpcId(id, "Call to client ClientOnPlayersChange" + playerConnection.Key + EncodePlayerConnection(playerConnection.Value));
        //     RpcId(id, "ClientOnPlayersChange",
        //           playerConnection.Key, EncodePlayerConnection(playerConnection.Value));
        // }
    }

    public void Connect(string address) {
        if (string.IsNullOrEmpty(address)) {
            address = DEFAULT_SERVER_IP;
        }

        ENetMultiplayerPeer peer = new ENetMultiplayerPeer();
        Error error = peer.CreateClient(address, PORT);
        GlobalStateMachine.instance.Connecting();
        if (error != Error.Ok) {
            Log.Error("Error connecting to the server " + address + ":" + PORT + ". " + error);
            return;
        }

        GlobalStateMachine.instance.Connected();

        Multiplayer.MultiplayerPeer = peer;
        _myMultiplayerId = Multiplayer.GetUniqueId();
    }

    public void Host() {
        ENetMultiplayerPeer peer = new ENetMultiplayerPeer();
        var error = peer.CreateServer(PORT, MAX_CONNECTIONS);
        if (error != Error.Ok) {
            Log.Error("Error creating the server " + MAX_CONNECTIONS + " players at port: " + PORT + ". " + error);
            return;
        }

        Multiplayer.MultiplayerPeer = peer;

        AddPlayer(Multiplayer.GetUniqueId());

        // const long id = 1;
        // _myMultiplayerId = id;
        // PlayerConnection connection = new() {
        //     Id = id,
        //     Nickname = ConfigurationAutoLoad.playerName,
        //     Status = GlobalStateMachine.instance.CurrentState()
        // };
        //
        // EmitSignal(NetworkManager.SignalName.OnPlayersChange, id, EncodePlayerConnection(connection));
        //
        _serverManager.Visible = true;
        _serverManager.StartServer();
        
        GlobalStateMachine.instance.Connected();
        Log.Info("Server started on port: " + PORT);
    }

    private void AddPlayer(long id) {
        Node player = _playerScene.Instantiate();
        player.Name = id.ToString();
        _playersRoot.AddChild(player);
    }

    public void Disconnect() {
        Multiplayer.MultiplayerPeer.Close();
    }

    public void LoadMap(string mapName) {
        if (!Multiplayer.IsServer()) {
            Log.Error("Cannot load map. Host server is not running.");
            return;
        }

        // _mapLoader.ServerLoadMap(mapName);
    }

    public List<PlayerConnection> GetPlayers() {
        List<PlayerConnection> ret = new();
        foreach (KeyValuePair<long, PlayerConnection> pair in _players) {
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
        } else {
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
        _players[connection.Id] = connection;
        Log.Rpc("ServerReceivePlayerInformation" + EncodePlayerConnection(connection));
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
            EmitSignal(NetworkManager.SignalName.OnPlayersChange, EncodePlayerConnection(connection));
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