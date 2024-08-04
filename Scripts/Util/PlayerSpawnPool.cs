using System.Collections.Generic;
using Godot;
using ProjectBriseis.Scripts.AutoLoad.Multiplayer;
using ProjectBriseis.Scripts.Manager.Server;
using ProjectBriseis.Scripts.Map.Spawn;

namespace ProjectBriseis.Scripts.Util;

public struct SpawnPlayer {
    public PlayerInTeam playerInTeam;
    public PlayerSpawnArea spawnArea;
    public Vector3 Position;
}

public class PlayerSpawnPool {
    private Dictionary<PlayerSpawnArea, List<PlayerInTeam>> _spawnPool = new();
    private bool _poolDirty = false;

    public void AddPlayerToPool(PlayerSpawnArea spawnArea, PlayerInTeam playerInTeam) {
        _spawnPool[spawnArea] ??= new List<PlayerInTeam>();
        _spawnPool[spawnArea].Add(playerInTeam);
        _poolDirty = true;
    }

    public List<SpawnPlayer> DumpPool() {
        List<SpawnPlayer> ret = new();

        foreach (KeyValuePair<PlayerSpawnArea, List<PlayerInTeam>> keyPair in _spawnPool) {
            PlayerSpawnArea spawn = keyPair.Key;
            List<PlayerInTeam> players = keyPair.Value;
            List<Vector3> spawnPoints = spawn.GetSpawnPoints(players.Count);
            
            for (int i = 0; i < players.Count; i++) {
                ret.Add(new SpawnPlayer() {
                    spawnArea = spawn,
                    playerInTeam = players[i],
                    Position = spawnPoints[i]
                });
            }
        }

        _poolDirty = false;

        return ret;
    }


    public bool IsDirty() {
        return _poolDirty;
    }
}