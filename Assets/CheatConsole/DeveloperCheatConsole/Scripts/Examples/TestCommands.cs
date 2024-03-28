using System.Collections.Generic;
using Avocado.DeveloperCheatConsole.Scripts.Core;
using Avocado.DeveloperCheatConsole.Scripts.Core.Commands;
using UnityEngine;

namespace Avocado.DeveloperCheatConsole.Scripts.Examples {
    public class TestCommands : MonoBehaviour {
        private void Awake() {
            //Add command without parameters
            DeveloperConsole.Instance.AddCommand(new DevCommand("test", "test without parameters", () => {
                Debug.Log("success execute command test without parameters");
            }));
            
            //Add command with one string parameter
            DeveloperConsole.Instance.AddCommand(new DevCommand("test", "test with one parameter", delegate(string parameter) {
                Debug.Log("success execute command test with one string parameter " + parameter);
            }));
            
            //Add command with one int parameter
            DeveloperConsole.Instance.AddCommand(new DevCommand("test", "test with one parameter", delegate(int parameter) {
                Debug.Log("success execute command test with one number parameter " + parameter);
            }));
            
            //Add command with range string parameters
            DeveloperConsole.Instance.AddCommand(new DevCommand("test", "test with range string parameters", delegate(List<string> parameters) {
                Debug.Log("success execute command test with range string parameters " + string.Join(" ", parameters));
            }));
            
            //Add command with range number parameters
            DeveloperConsole.Instance.AddCommand(new DevCommand("test", "test with range string parameters", delegate(List<int> parameters) {
                Debug.Log("success execute command test with range number parameters " + string.Join(" ", parameters));
            }));
        }
    }
}