using System;
using Godot;
using ProjectBriseis.objects.Logic;

namespace ProjectBriseis.Scripts.AutoLoad;

public enum GlobalStates {
    Launching = 0,
    Intro = 1,
    MainMenu = 2,
    Connecting = 3,
    Loading = 4,
    Credits = 5
}

public enum StartPoint {
    Intro = 0,
    Menu = 1
}

public partial class GlobalStateMachine : Singleton<GlobalStateMachine> {
    private Node3D _loadedScene = null;
    private bool _started = false;
    private Node3D _startupScene;

    private GlobalStates _state = GlobalStates.Launching;

    [Signal]
    public delegate void OnStatechangeEventHandler(GlobalStates newState, GlobalStates oldState);

    public void Start(Node3D startupScene, StartPoint startingState) {
        if (_started) {
            Log.Warning("Trying to start with state " + startingState + " but states already started");
            return;
        }

        _startupScene = startupScene;
        CallDeferred("StartDeferred", (int) startingState);
    }

    private void StartDeferred(string startingStateS) {
        Enum.TryParse(startingStateS, out StartPoint startingState);
        switch (startingState) {
            case StartPoint.Menu:
                LoadScene("MainMenu.tscn");
                break;
            case StartPoint.Intro:
            default:
                LoadScene("Intro.tscn");
                break;
        }
    }

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

    private void TransitionState(GlobalStates newState) {
        GlobalStates oldState = _state;
        _state = newState;
        EmitSignal(SignalName.OnStatechange, (int) newState, (int) oldState);
    }

    private void LoadScene(string scenePath) {
        if (_loadedScene != null) {
            _loadedScene.GetParent().RemoveChild(_loadedScene);
            return;
        }

        string path = "res://Scenes/" + scenePath;

        if (ResourceLoader.Load(path) is PackedScene scene) {
            Node sceneInstance = scene.Instantiate();
            _startupScene.AddChild(sceneInstance);
        }
    }

    public GlobalStates CurrentState() {
        return _state;
    }
}