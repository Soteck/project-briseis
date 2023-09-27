using Godot;
using ProjectBriseis.objects.Logic;

namespace ProjectBriseis.objects.AutoLoad {
    public partial class ConsoleAutoLoad : Singleton<ConsoleAutoLoad> {

        private bool _consoleDeployed = false;
        public RichTextLabel consoleLabel { get; private set; }
        public ScrollContainer consoleContainer { get; private set; }

        public override void _SingletonReady() {
            consoleContainer = GetChild<ScrollContainer>(0);
            consoleLabel = consoleContainer.GetChild<RichTextLabel>(0);
        }

        public override void _Process(double delta) {
            if (Input.IsActionJustPressed("console")) {
                Log.Info("Console pressed!!");
                _consoleDeployed = !_consoleDeployed;
                if (_consoleDeployed) {
                    consoleContainer.SetSize(new Vector2(consoleContainer.Size.X, 200));
                } else {
                    consoleContainer.SetSize(new Vector2(consoleContainer.Size.X, 0));
                }
            }
        }
    }
}