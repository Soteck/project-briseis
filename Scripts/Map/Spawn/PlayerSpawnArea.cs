using System;
using System.Collections.Generic;
using Godot;
using Godot.Collections;
using ProjectBriseis.Scripts.AutoLoad;
using ProjectBriseis.Scripts.AutoLoad.Multiplayer;
using ProjectBriseis.Scripts.Util;

namespace ProjectBriseis.Scripts.Map.Spawn;

public partial class PlayerSpawnArea : Node3D {
    
    [Export]
    private Team _team;
    
    [Export]
    private string _name;


    private List<Vector3> _spawnPoints = new();
    
    
    private const float SpawnSize = 1f;
    private const float SpawnFromFloor = 0.9f;
    public override void _Ready() {
        
        Array<Node> children = GetChildren();
        
        foreach (Node child in children) {
            if (child is CollisionShape3D area3D) {
                PopulateSpawnPointsPerArea(area3D);
            }
        }

        if (_spawnPoints.Count < 16) {
            Log.Warning("Not enough spawn area for spawn " + _name + " Nº: " + _spawnPoints.Count);
        }
    }


    private void PopulateSpawnPointsPerArea(CollisionShape3D shape) {
        Vector3 componentSize = ((BoxShape3D) shape.Shape).Size;
        Vector3 position = shape.GlobalPosition;
        //Now we will split the X and Y of the size of the spawn as a grid and assign every slot as a spawn point
        long xN = (long) Math.Floor(componentSize.X / SpawnSize);
        long zN = (long) Math.Floor(componentSize.Z / SpawnSize);
        for (int xIndex = 0; xIndex < xN; xIndex++) {
            for (int zIndex = 0; zIndex < zN; zIndex++) {
                _spawnPoints.Add(new Vector3(
                                             (position.X + (xIndex - 1) + (SpawnSize / 2)),
                                             position.Y + SpawnFromFloor,
                                             (position.Z + (zIndex - 1) + (SpawnSize / 2))
                                            ));
            }
        }
    }

    public List<Vector3> GetSpawnPoints(int number) {
        List<Vector3> ret = new();
        
        foreach (int position in RandomUtil.GenerateRandomNumbers(0, _spawnPoints.Count -1, number)) {
           ret.Add(_spawnPoints[position]); 
        }

        return ret;
    }

}