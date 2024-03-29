using ProjectBriseis.Scripts.AutoLoad;

namespace ProjectBriseis.objects.Logic.Console.Commands {
    public partial class QuitCommand : BaseCommand {
        public QuitCommand() : base("quit") {
        }

        public override void _Run(string[] args) {
            Log.Info("Bye.");
            GetTree().Quit();
        }

        public override string[][] GetSubCommands() {
            return null;
        }
    }
}