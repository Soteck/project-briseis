using Godot;
using ProjectBriseis.objects.Logic;
using ProjectBriseis.objects.Logic.Console;

namespace ProjectBriseis.objects.AutoLoad {
    public partial class ConsoleAutoLoad : Singleton<ConsoleAutoLoad> {
        private bool _consoleDeployed = false;
        private float _originalContainerHeight = 0;
        public RichTextLabel consoleLabel { get; private set; }
        public Control consoleContainer { get; private set; }
        public ScrollContainer scrollContainer { get; private set; }
        public ConsoleInterpreter consoleInterpreter { get; private set; }

        public LineEdit lineEdit { get; private set; }

        public override void _SingletonReady() {
            consoleContainer = GetChild<Control>(0);
            consoleInterpreter = GetChild<ConsoleInterpreter>(1);
            Control vbox = consoleContainer.GetChild<Control>(0);
            scrollContainer = vbox.GetChild<ScrollContainer>(0);
            lineEdit = vbox.GetChild<LineEdit>(1);
            consoleLabel = scrollContainer.GetChild<RichTextLabel>(0);
            _originalContainerHeight = consoleContainer.Size.X;
            lineEdit.TextSubmitted += lineEditOnTextSubmitted();
            Undeploy();
        }


        public override void _Process(double delta) {
            if (Input.IsActionJustPressed("console")) {
                _consoleDeployed = !_consoleDeployed;
                if (_consoleDeployed) {
                    Deploy();
                } else {
                    Undeploy();
                }
            }
        }

        private void Undeploy() {
            consoleContainer.SetSize(new Vector2(consoleContainer.Size.X, 0));
            consoleContainer.Visible = false;
            lineEdit.Clear();
        }


        private void Deploy() {
            consoleContainer.SetSize(new Vector2(consoleContainer.Size.X, 200));
            consoleContainer.Visible = true;
            lineEdit.GrabFocus();
        }

        private LineEdit.TextSubmittedEventHandler lineEditOnTextSubmitted() {
            return text => {
                if (text.Length > 0) {
                    lineEdit.Clear();
                    Log.UserInput(text);
                    consoleInterpreter.RunInput(text);
                }
            };
        }
    }
}