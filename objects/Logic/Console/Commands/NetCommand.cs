using MultiplayerAutoLoad = ProjectBriseis.Scripts.AutoLoad.Multiplayer.MultiplayerAutoLoad;
using Godot;
using ProjectBriseis.Scripts.Manager;

namespace ProjectBriseis.objects.Logic.Console.Commands {
    public partial class NetCommand : BaseCommand {
        
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
                    NetworkManager.instance.Host();
                    break;
                case "connect":
                    NetworkManager.instance.Connect(firstArg);
                    break;
                case "disconnect":
                    NetworkManager.instance.Disconnect();
                    break;
                case "map":
                    NetworkManager.instance.LoadMap(firstArg);
                    break;
                    default:
                    break;
            }
        }

        public override string[][] GetSubCommands() {
            return new string[][] {new string[] {"host", "connect", "disconnect", "map"}};
        }
    }
}