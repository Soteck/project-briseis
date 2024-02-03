using System.Collections.Generic;
using Godot;
using ProjectBriseis.objects.Logic;

namespace ProjectBriseis.Scripts.AutoLoad {
    public partial class StartUpParamsAutoLoad : Singleton<StartUpParamsAutoLoad> {
        public override void _SingletonReady() {
            string[] args = OS.GetCmdlineArgs();
            List<string> tmpArgs = new();
            foreach (string argument in args) {
                if (argument.StartsWith("--")) {
                    if (tmpArgs.Count != 0) {
                        RunArgs(tmpArgs);
                    }

                    tmpArgs.Add(argument.Substring(2));
                } else {
                    tmpArgs.Add(argument);
                }
            }

            if (tmpArgs.Count > 0) {
                RunArgs(tmpArgs);
            }
        }

        private static void RunArgs(List<string> tmpArgs) {
            Scripts.AutoLoad.ConsoleAutoLoad.instance.consoleInterpreter.RunInput(tmpArgs.ToArray().Join(" "));
            tmpArgs.Clear();
        }
    }
}