using System;
using System.Collections.Generic;
using System.Linq;
using Avocado.DeveloperCheatConsole.Scripts.Core.Commands;

namespace Avocado.DeveloperCheatConsole.Scripts.Core {
    public class DeveloperConsole {
        public static DeveloperConsole Instance {
            get {
                if (_instance is null) {
                    _instance = new DeveloperConsole();
                }

                return _instance;
            }
        }

        private static DeveloperConsole _instance;

        public IList<DevCommand> Commands => _commands;
        public bool ShowConsole { get; set; }
        public bool ShowHelp { get; set; }
        
        private  IList<DevCommand> _commands = new List<DevCommand>();
        private IList<string> _commandsBuffer = new List<string>();
        private int _currentIndexBufferCommand;
        
        private DeveloperConsole() {
            AddBuildInCommands();
        }

        public void AddCommand(DevCommand command) {
            _commands.Add(command);
        }

        private void AddBuildInCommands() {
            var help = new DevCommand("help", "show a list of commands", () => {
                ShowHelp = true;
            });
            
            var exit = new DevCommand("exit", "disable console", () => {
                ShowConsole = false;
            });
            
            AddCommand(help);
            AddCommand(exit);
        }

        public virtual void InvokeCommand(string commandInput) {
            var success = false;
            var properties = commandInput.Split(' ');
            var parameters = properties.ToList();
            parameters.RemoveAt(0);
            
            foreach (var command in _commands) {
                if (!commandInput.Contains(command.Id)) {
                    continue;
                }

                if (parameters.Count == 0 && command.Command != null) {
                    command.Invoke();
                    success = true;
                    break;
                }
                if (parameters.Count == 1) {
                    if (int.TryParse(parameters[0], out int resultInt)) {
                        if (command.CommandInt is null) {
                            continue;
                        } 
                        
                        command.Invoke(resultInt);
                        success = true;
                        break;
                    }
                    if(command.CommandStr != null) {
                        command.Invoke(parameters[0]);
                        success = true;
                        break;
                    }
                } 
                if (parameters.Count > 1 && parameters.TrueForAll(value => int.TryParse(value, out int intValue))) {
                    if (command.CommandListInt is null) {
                        continue;
                    }

                    command.Invoke(parameters.Select(int.Parse).ToList());
                    success = true;
                    break;
                }

                if (parameters.Count > 1 && command.CommandListStr != null) {
                    command.Invoke(parameters);
                    success = true;
                    break;
                }
            }
            
            if (success && !_commandsBuffer.Contains(commandInput)) {
                _commandsBuffer.Add(commandInput);
            }
        }

        public string GetBufferCommand(bool next) {
            if (next) {
                _currentIndexBufferCommand++;
            } else {
                _currentIndexBufferCommand--;
            }

            if (_currentIndexBufferCommand < 0) {
                _currentIndexBufferCommand = _commandsBuffer.Count - 1;
            }else if (_currentIndexBufferCommand >= _commandsBuffer.Count) {
                _currentIndexBufferCommand = 0;
            }

            return _commandsBuffer[_currentIndexBufferCommand];
        }
    }
}