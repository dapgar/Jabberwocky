using Avocado.DeveloperCheatConsole.Scripts.Core;
using UnityEngine;

namespace Avocado.DeveloperCheatConsole.Scripts.Visual {
    public abstract class DevConsoleGUI : MonoBehaviour {
        [SerializeField]
        [Range(0.1f, 2f)]
        protected float Scale = 1f;
        protected DeveloperConsole _console;
        
        protected string _input;
        protected Vector2 _scroll;
        protected bool _inputFocus;
        
        private void Awake() {
#if UNITY_EDITOR
            _console = DeveloperConsole.Instance;
            GenerateCommands();
#endif
        }
        
        private void GenerateCommands() {
            
        }
        
        private void OnGUI() {
#if UNITY_EDITOR
            DrawGUI();
#endif
        }
        
        protected void OnReturn() {
            if (_console.ShowConsole) {
                _console.InvokeCommand(_input);
                _input = "";
            }
        }

        protected void HandleEscape() {
            if (!_console.ShowHelp) {
                _console.ShowConsole = false;
                _inputFocus = false;
            } else {
                _console.ShowHelp = false;
                GUI.FocusControl("inputField");
            }
        }

        protected virtual void HandleShowConsole() {
        }
        
        protected virtual void HadnleKeyboardInGUI() {
        }
        
        private void DrawGUI() {
            if (!_console.ShowConsole) {
                HandleShowConsole();
                return;
            }
            
            HadnleKeyboardInGUI();

            var inputHeight = 100 * Scale;
            var y = Screen.height - inputHeight;

            if (_console.ShowHelp) {
                ShowHelp(y);
            }

            GUI.Box(new Rect(0, y, Screen.width, 100 * Scale), "");
            GUI.backgroundColor = Color.black;
            
            var labelStyle = new GUIStyle("TextField");
            labelStyle.fontStyle = FontStyle.Normal;
            var fontSize = 40 * Scale;
            labelStyle.fontSize = (int)fontSize;
            labelStyle.alignment = TextAnchor.MiddleLeft;
            GUI.SetNextControlName("inputField");
            _input = GUI.TextField(new Rect(10f, y + 10f, Screen.width - 20, 70f * Scale), _input, labelStyle);

            SetFocusTextField();
        }

        protected virtual void SetFocusTextField() {
            if (!_inputFocus) {
                _inputFocus = true;
                GUI.FocusControl("inputField");
                _input = string.Empty;
            }
        }

        private void ShowHelp(float y) {
            GUI.Box(new Rect(0, y - 500 * Scale, Screen.width, 500* Scale), "");
            var viewPort = new Rect(0, 0, Screen.width - 30, 80 * _console.Commands.Count * Scale);
            _scroll = GUI.BeginScrollView(new Rect(0, y - 480f * Scale, Screen.width, 480 * Scale), _scroll, viewPort);

            int i = 0;
            foreach (var command in _console.Commands) {
                var label = $"{command.Id} - {command.Description}";
                var labelRect = new Rect(10, 50 * i * Scale, viewPort.width-100, 50 * Scale);
                    
                var labelStyleHelp = new GUIStyle("label");
                labelStyleHelp.fontStyle = FontStyle.Normal;
                var fontSize = 30 * Scale;
                labelStyleHelp.fontSize = (int)fontSize;
                    
                GUI.Label(labelRect, label, labelStyleHelp);
                
                i++;
            }
                
            GUI.EndScrollView();
        }
    }
}
