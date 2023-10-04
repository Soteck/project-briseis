using ProjectBriseis.objects.AutoLoad;

namespace ProjectBriseis.objects.Logic.Console.Commands {
    public partial class ConfigCommand : ConsoleInterpreter.BaseCommand {
        public ConfigCommand() : base("config") {
        }

        public override void _Run(string[] args) {
            if (args == null || args.Length < 1) {
                return;
            }

            if (args[0] == "write") {
                ConfigurationAutoLoad.WriteConfig();
                Log.Info("Configuration saved");
                return;
            }

            if (args[0] == "reset") {
                ConfigurationAutoLoad.ResetConfig();
                Log.Info("Configuration reset!");
                return;
            }

            DoSetConfig(args);
        }

        private void DoSetConfig(string[] args) {
            bool write = args.Length == 2;
            string parameter = args[0];

            bool applied = true;
            string oldValue = "";
            switch (parameter) {
                case "sensitivity": {
                    if (write) {
                        ConfigurationAutoLoad.mouseSensitivity = float.Parse(args[1]);
                    }

                    oldValue = ConfigurationAutoLoad.mouseSensitivity.ToString();


                    break;
                }
                case "invertMouse": {
                    if (write) {
                        ConfigurationAutoLoad.invertMouse = bool.Parse(args[1]);
                    }
                    oldValue = ConfigurationAutoLoad.invertMouse.ToString();

                    break;
                }
                default:
                    applied = false;
                    break;
            }

            if (applied) {
                if (write) {
                    Log.Info("Config " + parameter + " changed from " + oldValue + " to " + args[1]);
                } else {
                    Log.Info("Config " + parameter + " is set to " + oldValue);
                }
            } else {
                Log.Warning("Config not applied");
            }
        }
    }
}