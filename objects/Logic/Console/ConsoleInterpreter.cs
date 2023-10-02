using System;
using System.Collections.Generic;
using Godot;
using ProjectBriseis.objects.AutoLoad;
using ProjectBriseis.objects.Logic.Console.Commands;

namespace ProjectBriseis.objects.Logic.Console {
    public partial class ConsoleInterpreter : Node {
        
        private readonly Type[] _commandsTypes = {
            typeof(ConfigCommand),
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
                BaseCommand command = _commandsDictionary[splitInput[0]];
                if (splitInput.Length < (command.paramNumber + 1)) {
                    Log.Info("Not enough params. Expected : " + command.paramNumber + ", got: " +
                             (splitInput.Length - 1));
                    return;
                }

                string[] args = new string[command.paramNumber];
                for (int i = 0; i < command.paramNumber; i++) {
                    args[i] = splitInput[i + 1];
                }

                command._Run(args);
            } else {
                Log.Info("Command not found: " + input);
            }
        }

        public abstract partial class BaseCommand : Node {
            public string commandKey { get; private set; }

            public int paramNumber { get; private set; }

            protected BaseCommand(string key, int paramNumber) {
                commandKey = key;
                this.paramNumber = paramNumber;
            }

            public abstract void _Run(string[] args);
        }
    }
}