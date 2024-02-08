using System.Collections.Generic;

namespace ProjectBriseis.objects.Logic.Console.Commands;
using Godot;


public abstract partial class BaseCommand : Node {
    public string commandKey { get; private set; }

    protected BaseCommand(string key) {
        commandKey = key;
    }

    public abstract void _Run(string[] args);


    public abstract string[][] GetSubCommands();

    public string AutoComplete(string[] input) {
        string[][] commands = GetSubCommands();
        if (commands == null) {
            return null;
        }

        return AutoComplete(input, commands);
    }
    public string AutoComplete(string[] input, string[][] commands) {
        int inputPosition = input.Length -1;
        if (inputPosition  <= commands.Length) {
            string s = input[inputPosition];
            string found = null;
            foreach (string subCommand in commands[inputPosition -1]) {
                if (subCommand.StartsWith(s)) {
                    if (found != null) {
                        return null;
                    } else {
                        found = subCommand;
                    }
                }
            }

            if (found != null) {
                //TODO: Extra feedback?
                input[inputPosition] = found;
            }

            string ret = input.Join(" ");
            if (found != null) {
                ret += " ";
            }
            return ret;
        }
            
        return null;
    }
}