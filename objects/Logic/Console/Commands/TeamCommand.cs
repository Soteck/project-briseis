using ProjectBriseis.Scripts.Manager.Server;

namespace ProjectBriseis.objects.Logic.Console.Commands;

public partial class TeamCommand : BaseCommand {
        
    public TeamCommand() : base("team") {
    }

    public override void _Run(string[] args) {
        string subcommand = args[0];

        switch (subcommand) {
            case "a":
                ServerPlayersManager.instance.joinTeamA();
                break;
            case "b":
                ServerPlayersManager.instance.joinTeamB();
                break;
            default:
                break;
        }
    }

    public override string[][] GetSubCommands() {
        return new string[][] {new string[] {"a", "b"}};
    }
}