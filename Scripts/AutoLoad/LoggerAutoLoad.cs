using System;
using System.Collections.Generic;
using Godot;
using ProjectBriseis.objects.Logic;

namespace ProjectBriseis.objects.AutoLoad {
    public partial class LoggerAutoLoad  : Singleton<LoggerAutoLoad> {

        private RichTextLabel _consoleLabel;
        private ScrollContainer _consoleScrollContainer;

        private int _maxLines = 100;
        
        private const int ConsoleMaxLength = 5000;
        private const int ConsoleChunkToDelete = 2500;
        private const int ConsoleOverflow = ConsoleMaxLength + ConsoleChunkToDelete;
        
        
        private readonly List<LogLine> _lines = new List<LogLine>();
        
        public override void _SingletonReady()
        {
            _consoleLabel = ConsoleAutoLoad.instance.consoleLabel;
            _consoleScrollContainer = ConsoleAutoLoad.instance.scrollContainer;
            //TODO: Get _maxLines value from config
            

            _consoleLabel.Text = string.Empty;
            AddText($"[color=\"blue\"]{DateTime.Now.ToString("HH:mm:ss.fff")} {this.GetType().Name} enabled[/color]");
        }
        
        // private void OnEnable()
        // {
        //     if (debugAreaText == null)
        //     {
        //         debugAreaText = GetComponent[TextMeshProUGUI]();
        //     }
        //     DrawLog();
        // }

        private void AddText(string text) {
            GD.PrintRich(text);
            _lines.Add(new LogLine() {
                Drawn = false,
                Data = text
            });
            
            if (_lines.Count > _maxLines) {
                _lines.RemoveAt(0);
            }
            DrawLog();
        }

        private void DrawLog() {
            if (_consoleLabel != null) {
                if (_consoleLabel.Text.Length > ConsoleOverflow ) {
                    _consoleLabel.Text = _consoleLabel.Text.Substring(ConsoleMaxLength, _consoleLabel.Text.Length - ConsoleMaxLength);
                }
            
                foreach (LogLine line in _lines) {
                    if (line.Drawn) continue;
                    _consoleLabel.AppendText(line.Data + '\n');
                    line.Drawn = true;
                }

                _consoleScrollContainer.ScrollVertical = (int) _consoleScrollContainer.GetVScrollBar().MaxValue;
            }
        }

        public void LogInfo(string message) {
            AddText($"[color=\"green\"]{DateTime.Now:HH:mm:ss.fff} {message}[/color]");
        }

        public void UserInput(string message) {
            AddText($"[color=\"white\"]{DateTime.Now:HH:mm:ss.fff} {message}[/color]");
        }

        public void LogError(string message)
        {
            AddText($"[color=\"red\"]{DateTime.Now:HH:mm:ss.fff} {message}[/color]");
        }

        public void LogWarning(string message)
        {
            AddText($"[color=\"yellow\"]{DateTime.Now:HH:mm:ss.fff} {message}[/color]");
        }

        public void LogDebug(string message)
        {
            AddText($"[color=\"orange\"]{DateTime.Now:HH:mm:ss.fff} {message}[/color]");
        }
        
        private class LogLine {
            public bool Drawn;
            public string Data;
        }
    }
    
    public abstract class Log {
        public static void Info(string message) {
            LoggerAutoLoad.instance.LogInfo(message);
        }
        public static void UserInput(string message) {
            LoggerAutoLoad.instance.UserInput("<<< " + message);
        }

        public static void Error(string message) {
            LoggerAutoLoad.instance.LogError(message);
        }

        public static void Warning(string message) {
            LoggerAutoLoad.instance.LogWarning(message);
        }

        public static void Debug(string message) {
            LoggerAutoLoad.instance.LogDebug(message);
        }
    }
}