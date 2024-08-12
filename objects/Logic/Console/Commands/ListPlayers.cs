using ProjectBriseis.Scripts.AutoLoad;
using ProjectBriseis.Scripts.Manager;

namespace ProjectBriseis.objects.Logic.Console.Commands {
    public partial class ListPlayers : BaseCommand {
        public ListPlayers() : base("listplayers") {
        }

        public override void _Run(string[] args) {
            Log.Info("Player ID\t\t\tPlayer Name\t\t\tTeam");
            foreach (var player in NetworkManager.instance.GetPlayers()) {
                Log.Info(player.Id + "\t\t\t" + player.Nickname + "\t\t\t" + player.team);
            }
        }

        public override string[][] GetSubCommands() {
            return null;
        }
    }
}