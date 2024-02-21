using Godot;
using Godot.Collections;
using ProjectBriseis.Scripts.Map.Spawn;

namespace ProjectBriseis.Scripts.Maps.Base;

public partial class BaseMap : Node3D {
    [Export]
    private Camera3D _mapCamera;

    [ExportGroup("Spawns")]
    [Export]
    private Array<PlayerSpawnArea> _spawnAreas;

    [Export]
    private PlayerSpawnArea _defaultTeamASpawnArea;

    [Export]
    private PlayerSpawnArea _defaultTeamBSpawnArea;


    public PlayerSpawnArea DefaultTeamBSpawnArea() {
        return _defaultTeamBSpawnArea;
    }

    public PlayerSpawnArea DefaultTeamASpawnArea() {
        return _defaultTeamASpawnArea;
    }
}