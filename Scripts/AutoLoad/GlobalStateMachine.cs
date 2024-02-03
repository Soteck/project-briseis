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

public partial class GlobalStateMachine : Singleton<GlobalStateMachine> {

    private Node3D _loadedScene = null;
    private bool _started = false;
    private Node3D _startupScene;
    public void Start(Node3D startupScene, GlobalStates startingState) {
        if (_started) {
            Log.Warning("Trying to start with state " + startingState  + " but states already started");
            return;
        }

        _startupScene = startupScene;
        switch (startingState) {
            case GlobalStates.MainMenu:
                CallDeferred("LoadScene", "MainMenu.tscn");
                break;
            case GlobalStates.Launching:
            //TODO: Implement
            case GlobalStates.Intro:
            //TODO: Implement
            default:
                CallDeferred("LoadScene", "Intro.tscn");
                break;
        }
    }
    
    public void LoadScene(string scenePath) {
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

}