using System;
using System.IO;
using ProjectBriseis.objects.Logic;
using SharpConfig;

namespace ProjectBriseis.objects.AutoLoad {
    public partial class ConfigurationAutoLoad : Singleton<ConfigurationAutoLoad> {
        private Configuration _cfg = new Configuration();
        public static Action onConfigurationChange;

        private const string DefaultPlayerName = "Briseis player";
        private const float DefaultMouseSensitivity = 15f;
        private const bool DefaultAutoSave = true;
        private const bool ServerDefaultAutoBalance = true;
        private const int ServerDefaultMaxEnergy = 100;
        private const bool DefaultAutoReload = true;
        private const bool DefaultInvertMouse = true;

        private void Start() {
            if (!File.Exists("config.cfg")) {
                Log.Info("Setting up a default config since no file was found!");
                SetupCleanCfg();
                SaveConfig();
            }

            // Load the configuration.
            _cfg = Configuration.LoadFromFile("config.cfg");
        }


        private void SaveConfig() {
            Log.Info("Saving Client config...");

            // Save the configuration.
            _cfg.SaveToFile("config.cfg");
        }

        private void DoResetConfig() {
            Log.Info("Reseting all config...");
            this.SetupCleanCfg();
            // Save the configuration.
            _cfg.SaveToFile("config.cfg");
        }

        private void SetupCleanCfg() {
            _cfg["Player"]["Name"].StringValue = DefaultPlayerName;
            _cfg["Input"]["MouseSensitivity"].FloatValue = DefaultMouseSensitivity;
            _cfg["Input"]["InvertMouse"].BoolValue = DefaultInvertMouse;
            _cfg["Weapons"]["AutoReload"].BoolValue = DefaultAutoReload;
            _cfg["Configuration"]["AutoSave"].BoolValue = DefaultAutoSave;
            _cfg["Server"]["AutoBalance"].BoolValue = ServerDefaultAutoBalance;
            _cfg["Server"]["MaxEnergy"].IntValue = ServerDefaultMaxEnergy;
        }

        private void ShouldSave() {
            if (_cfg["Core"]["AutoSave"].BoolValue) {
                SaveConfig();
            }

            onConfigurationChange?.Invoke();
        }

        public static void WriteConfig() {
            instance.SaveConfig();
        }

        public static void ResetConfig() {
            instance.DoResetConfig();
        }

        public static bool autoSave {
            get => instance._cfg["Configuration"]["AutoSave"].BoolValue;

            set {
                instance._cfg["Configuration"]["AutoSave"].BoolValue = value;
                instance.ShouldSave();
            }
        }

        public static string playerName {
            get => instance._cfg["Player"]["Name"].StringValue;

            set {
                instance._cfg["Player"]["Name"].StringValue = value;
                instance.ShouldSave();
            }
        }

        public static float mouseSensitivity {
            get => instance._cfg["Input"]["MouseSensitivity"].FloatValue;

            set {
                instance._cfg["Input"]["MouseSensitivity"].FloatValue = value;
                instance.ShouldSave();
            }
        }

        public static bool autoBalance {
            get => instance._cfg["Server"]["AutoBalance"].BoolValue;

            set {
                instance._cfg["Server"]["AutoBalance"].BoolValue = value;
                instance.ShouldSave();
            }
        }

        public static bool autoReload {
            get => instance._cfg["Weapons"]["AutoReload"].BoolValue;

            set {
                instance._cfg["Weapons"]["AutoReload"].BoolValue = value;
                instance.ShouldSave();
            }
        }

        public static bool invertMouse {
            get => instance._cfg["Input"]["InvertMouse"].BoolValue;

            set {
                instance._cfg["Input"]["InvertMouse"].BoolValue = value;
                instance.ShouldSave();
            }
        }

        public static int maxEnergy {
            get => instance._cfg["Server"]["MaxEnergy"].IntValue;

            set {
                instance._cfg["Server"]["MaxEnergy"].IntValue = value;
                instance.ShouldSave();
            }
        }

        public static float constantOfGravity => 9.81f;
        public static float constantOfTerminalVelocity => 53.0f;
        public static float constantOfSpeed => 6.0f;
        public static float constantOfSpeedSprintMultiplier => 1.334f;
        public static float constantOfKnockedDownHealth => 75f;
        public static float constantOfJumpHeight => .55f;
        public static float constantOfStaminaDecreaseRate => 25f;
        public static float constantOfStaminaIncreaseRate => 7f;
        public static float constantOfStaminaMax => 100f;
        public static float constantOfEnergyDecreaseRate => 25f;
        public static float constantOfEnergyIncreaseRate => 7f;
        public static float constantOfEnergyMax => 100f;
        public static float constantOfStaminaJumpPenalty => 22.5f;
    }
}