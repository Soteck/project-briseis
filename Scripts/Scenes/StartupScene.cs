using System;
using System.Collections.Generic;
using System.Linq;
using Godot;
using ProjectBriseis.Scripts.AutoLoad;

namespace ProjectBriseis.Scripts.Scenes;

public partial class StartupScene : Node3D {

    private GlobalStates _startingState = GlobalStates.MainMenu;
    
    private const string StartupStateKey = "StartupState";
    public override void _Ready() {
        string[] args = OS.GetCmdlineArgs();
        Dictionary<string, List<string>> tmpArgs = new();

        string argument = null;
        foreach (string data in args) {
            if (data.StartsWith("--")) {
                argument = data.Substring(2);
                tmpArgs[argument] = new List<string>();
            } else {
                if (argument != null) {
                    tmpArgs[argument].Add(data);
                } else {
                    Log.Warning("Ignoring arg " + data);
                }
            }
        }

        if (tmpArgs.ContainsKey(StartupStateKey)) {
            var newState = tmpArgs[StartupStateKey].First();
            Enum.TryParse(newState, out _startingState);
            tmpArgs.Remove(StartupStateKey);
        }

        if (tmpArgs.Count > 0) {
            foreach (KeyValuePair<string,List<string>> arg in tmpArgs) {
                RunArgs(arg.Key, arg.Value.ToArray());
                tmpArgs.Remove(arg.Key);
            }
        }

        GlobalStateMachine.instance.Start(this, _startingState);
    }

    private static void RunArgs(string arg, string[] argParams) {
        ConsoleAutoLoad.instance.consoleInterpreter.RunInput(arg, argParams);
    }
}