using System;
using System.Collections.Generic;
using Godot;
using ProjectBriseis.objects.Logic.Console.Commands;
using ProjectBriseis.Scripts.AutoLoad;

namespace ProjectBriseis.objects.Logic.Console {
    public partial class ConsoleInterpreter : Node {
        
        private readonly Type[] _commandsTypes = {
            typeof(ConfigCommand),
            typeof(NetCommand),
            typeof(QuitCommand)
        };


        private readonly Dictionary<string, BaseCommand> _commandsDictionary = new();


        public override void _Ready() {
            foreach (Type type in _commandsTypes) {
                InstantiateCommandClass(type);
            }
        }

        private void InstantiateCommandClass(Type type) {
            BaseCommand instance = (BaseCommand) Activator.CreateInstance(type);
            if (instance == null) {
                Log.Error("Command instance is null!");
                return;
            }

            _commandsDictionary.Add(instance.commandKey, instance);
            AddChild(instance);
        }


        public void RunInput(string input) {
            string[] splitInput = input.Split(" ");
            if (_commandsDictionary.ContainsKey(splitInput[0])) {
                int argSize = splitInput.Length - 1;
                string command = splitInput[0];

                string[] args = new string[argSize];
                for (int i = 0; i < argSize; i++) {
                    args[i] = splitInput[i + 1];
                }

                RunInput(command, args);
            } else {
                Log.Info("Command not found: " + input);
            }
        }

        public void RunInput(string command, string[] args) {
            BaseCommand commandExec = _commandsDictionary[command];
            commandExec._Run(args);
        }

        public abstract partial class BaseCommand : Node {
            public string commandKey { get; private set; }

            protected BaseCommand(string key) {
                commandKey = key;
            }

            public abstract void _Run(string[] args);
        }
    }
}