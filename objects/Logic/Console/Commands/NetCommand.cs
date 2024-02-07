using MultiplayerAutoLoad = ProjectBriseis.Scripts.AutoLoad.MultiplayerAutoLoad;
using Godot;

namespace ProjectBriseis.objects.Logic.Console.Commands {
    public partial class NetCommand : ConsoleInterpreter.BaseCommand {
        private string[] _subCommands = new[] {"host", "connect", "disconnect", "map"};
        
        public NetCommand() : base("net") {
        }

        public override void _Run(string[] args) {
            string subcommand = args[0];
            string firstArg = null;
            if (args.Length > 1) {
                firstArg = args[1];
            }
            switch (subcommand) {
                case "host":
                    MultiplayerAutoLoad.instance.Host();
                    break;
                case "connect":
                    MultiplayerAutoLoad.instance.Connect(firstArg);
                    break;
                case "disconnect":
                    MultiplayerAutoLoad.instance.Disconnect();
                    break;
                case "map":
                    MultiplayerAutoLoad.instance.LoadMap(firstArg);
                    break;
                    default:
                    break;
            }
            
        }

        public override string AutoComplete(string[] input) {
            if (input.Length == 2) {
                string s = input[1];
                string found = null;
                foreach (string subCommand in _subCommands) {
                    if (subCommand.StartsWith(s)) {
                        if (found != null) {
                            return null;
                        } else {
                            found = subCommand;
                        }
                    }
                }

                input[1] = found;
                return input.Join(" ");
            }
            
            return null;
        }
    }
}