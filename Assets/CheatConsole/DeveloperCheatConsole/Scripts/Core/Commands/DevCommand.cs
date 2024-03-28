using System;
using System.Collections.Generic;

namespace Avocado.DeveloperCheatConsole.Scripts.Core.Commands {
    public class DevCommand {
        public string Id { get; }
        public string Description { get; }

        public Action Command { get; private set; }
        public Action<string> CommandStr { get; private set; }
        public Action<List<string>> CommandListStr { get; private set; }
        public Action<List<int>> CommandListInt { get; private set; }
        public Action<int> CommandInt { get; private set; }

        private DevCommand(string commandId, string commandDescription) {
            Id = commandId;
            Description = commandDescription;
        }

        public DevCommand(string commandId, string commandDescription, Action command) : this(commandId, commandDescription) {
            Command = command;
        }
        
        public DevCommand(string commandId, string commandDescription, Action<string> command) : this(commandId, commandDescription) {
            CommandStr = command;
        }
        
        public DevCommand(string commandId, string commandDescription, Action<int> command) : this(commandId, commandDescription) {
            CommandInt = command;
        }
        
        public DevCommand(string commandId, string commandDescription, Action<List<string>> command) : this(commandId, commandDescription) {
            CommandListStr = command;
        }
        
        public DevCommand(string commandId, string commandDescription, Action<List<int>> command) : this(commandId, commandDescription) {
            CommandListInt = command;
        }

        public void Invoke() {
            Command.Invoke();
        }
        
        public void Invoke(string parameter) {
            CommandStr.Invoke(parameter);
        }
        
        public void Invoke(int parameter) {
            CommandInt.Invoke(parameter);
        }
        
        public void Invoke(List<string> parameters) {
            CommandListStr.Invoke(parameters);
        }
        
        public void Invoke(List<int> parameters) {
            CommandListInt.Invoke(parameters);
        }
    }
}