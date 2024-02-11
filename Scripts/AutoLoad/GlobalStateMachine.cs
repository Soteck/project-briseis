using System;
using Godot;
using ProjectBriseis.objects.Logic;

namespace ProjectBriseis.Scripts.AutoLoad;

public enum GlobalStates {
    Launching = 0,
    Intro = 1,
    MainMenu = 2,
    Connecting = 3,
    Connected = 4,
    Loading = 5,
    Match = 6,
    Credits = 7
}

public enum StartPoint {
    Intro = 0,
    Menu = 1
}

public partial class GlobalStateMachine : Singleton<GlobalStateMachine> {
    private bool _started = false;

    private Node3D _introScene = null;
    private Node3D _mainMenuScene = null;
    private Node3D _interfaceRoot;
    private Node3D _mapRoot;

    private GlobalStates _state = GlobalStates.Launching;

    [Signal]
    public delegate void OnStatechangeEventHandler(GlobalStates newState, GlobalStates oldState);

    public void Start(Node3D interfaceRoot, Node3D mapRoot, StartPoint startingState) {
        if (_started) {
            Log.Warning("Trying to start with state " + startingState + " but states already started");
            return;
        }

        _interfaceRoot = interfaceRoot;
        _mapRoot = mapRoot;

        if (startingState == StartPoint.Intro) {
            CallDeferred("LoadIntro");
            TransitionState(GlobalStates.Intro);
        } else {
            TransitionState(GlobalStates.MainMenu);
        }

        CallDeferred("LoadMainMenu");
    }

    private void StartDeferred(string startingStateS) {
        Enum.TryParse(startingStateS, out StartPoint startingState);
    }

    public void Connecting() {
        TransitionState(GlobalStates.Connecting);
    }

    public void Connected() {
        TransitionState(GlobalStates.Connected);
    }


    private void TransitionState(GlobalStates newState) {
        GlobalStates oldState = _state;
        _state = newState;
        IntroVisibility(newState == GlobalStates.MainMenu);
        MainMenuVisibility(newState == GlobalStates.MainMenu);

        EmitSignal(SignalName.OnStatechange, (int) newState, (int) oldState);
    }


    private void LoadIntro() {
        _introScene = LoadScene("Intro.tscn");
        TransitionState(GlobalStates.Intro);
    }

    private void IntroVisibility(bool visibility) {
        if (_introScene != null) {
            _introScene.Visible = visibility;
        }
    }

    private void MainMenuVisibility(bool visibility) {
        if (_mainMenuScene != null) {
            _mainMenuScene.Visible = visibility;
        }
    }

    private void LoadMainMenu() {
        _mainMenuScene = LoadScene("MainMenu.tscn");
        TransitionState(GlobalStates.MainMenu);
    }

    private Node3D LoadScene(string scenePath) {
        string path = "res://Scenes/" + scenePath;
        if (ResourceLoader.Load(path) is PackedScene scene) {
            Node3D sceneInstance = (Node3D) scene.Instantiate();
            _interfaceRoot.AddChild(sceneInstance);
            return sceneInstance;
        }

        return null;
    }

    public GlobalStates CurrentState() {
        return _state;
    }
}