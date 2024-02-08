using ProjectBriseis.Scripts.AutoLoad;

namespace ProjectBriseis.objects.Logic.Console.Commands {
    public partial class ListPlayers : BaseCommand {
        public ListPlayers() : base("listplayers") {
        }

        public override void _Run(string[] args) {
            foreach (var player in MultiplayerAutoLoad.instance.GetPlayers()) {
                Log.Info(player.Id + "\t" + player.Nickname);
            }
        }

        public override string[][] GetSubCommands() {
            return null;
        }
    }
}