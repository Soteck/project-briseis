using ProjectBriseis.objects.AutoLoad;

namespace ProjectBriseis.objects.Logic.Console.Commands {
    public partial class ConfigCommand : ConsoleInterpreter.BaseCommand {
        public ConfigCommand() : base("config", 2) {
        }

        public override void _Run(string[] args) {
            Log.Debug(args[0] + args[1]);
        }
    }
}