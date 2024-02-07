using MultiplayerAutoLoad = ProjectBriseis.Scripts.AutoLoad.MultiplayerAutoLoad;

namespace ProjectBriseis.objects.Logic.Console.Commands {
    public partial class NetCommand : ConsoleInterpreter.BaseCommand {
        
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
    }
}