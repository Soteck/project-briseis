using Godot;
using ProjectBriseis.objects.Logic;

namespace ProjectBriseis.objects.AutoLoad {
    public partial class ConsoleAutoLoad : Singleton<ConsoleAutoLoad> {
        
        public RichTextLabel consoleLabel { get; private set; }

        public override void _SingletonReady() {
            consoleLabel = GetChild<Node>(0).GetChild<RichTextLabel>(0);
        }

        public override void _Process(double delta) {
            if (Input.IsActionJustPressed("console")) {
                Log.Info("Console pressed!!");
            }
        }
    }
}