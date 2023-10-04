using ProjectBriseis.objects.AutoLoad;

namespace ProjectBriseis.objects.Logic.Console.Commands {
    public partial class NetCommand : ConsoleInterpreter.BaseCommand {
        
        public NetCommand() : base("net") {
        }

        public override void _Run(string[] args) {
            string subcommand = args[0];
            switch (subcommand) {
                case "host":
                    MultiplayerAutoLoad.instance.Host();
                    break;
                case "connect":
                    MultiplayerAutoLoad.instance.Connect(args[1]);
                    break;
                case "disconnect":
                    MultiplayerAutoLoad.instance.Disconnect();
                    break;
                    default:
                    break;
            }
            
            
        }
    }
}