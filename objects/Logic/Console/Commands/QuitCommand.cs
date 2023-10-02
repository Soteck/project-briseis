using ProjectBriseis.objects.AutoLoad;

namespace ProjectBriseis.objects.Logic.Console.Commands {
    public partial class QuitCommand : ConsoleInterpreter.BaseCommand {
        public QuitCommand() : base("quit", 0) {
        }

        public override void _Run(string[] args) {
            Log.Info("Bye.");
            GetTree().Quit();
        }
    }
}