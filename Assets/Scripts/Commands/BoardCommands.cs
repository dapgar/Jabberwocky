using Avocado.DeveloperCheatConsole.Scripts.Core;
using Avocado.DeveloperCheatConsole.Scripts.Core.Commands;
using System.Collections.Generic;
using UnityEngine;

public class BoardCommands : MonoBehaviour {
    private void Awake() {
        // Move player X certain num spaces
        // ex: MovePlayer 1 3 --> moves player1 3 spaces
        DeveloperConsole.Instance.AddCommand(
            new DevCommand("MovePlayer", "Move Player {#} {X} spaces", delegate (List<int> parameters) {
                BoardManager.instance.DevMovePlayer(parameters[0], parameters[1]);
        }));
        // Same command as above, but abbreviated to MP for simplicity and typing ease
        DeveloperConsole.Instance.AddCommand(
            new DevCommand("MP", "Move Player {#} {X} spaces", delegate (List<int> parameters) {
                BoardManager.instance.DevMovePlayer(parameters[0], parameters[1]);
            }));

        /* Set Next Minigame & Skip To Minigames */
        DeveloperConsole.Instance.AddCommand(
            new DevCommand("SetGame", "Next Game: RLGL | RT | SIS | LP", delegate (string parameter) {
                switch (parameter) {
                    case "RLGL":
                        GameManager.instance.devMinigameNumber = 0;
                        Debug.Log("Red Light Green Light Set");
                        break;
                    case "RT":
                        GameManager.instance.devMinigameNumber = 1;
                        Debug.Log("Reaction Time Set");
                        break;
                    case "SIS":
                        GameManager.instance.devMinigameNumber = 2;
                        Debug.Log("Sword In Stone Light Set");
                        break;
                    case "LP":
                        GameManager.instance.devMinigameNumber = 3;
                        Debug.Log("Lock Picking Set");
                        break;
                    default:
                        Debug.Log($"Game {parameter} is not recognized. Use RLGL | RT | SIS | LP");
                        break;
                }
        }));

        DeveloperConsole.Instance.AddCommand(
            new DevCommand("OpenGame", "Open Game: RLGL | RT | SIS | LP", delegate (string parameter) {
                GameManager.instance.devMinigameNumber = -1;
                switch (parameter) {
                    case "RLGL":
                        SceneChanger.Instance.ChangeScene(0 + 4);
                        break;
                    case "RT":
                        SceneChanger.Instance.ChangeScene(1 + 4);
                        break;
                    case "SIS":
                        SceneChanger.Instance.ChangeScene(2 + 4);
                        break;
                    case "LP":
                        SceneChanger.Instance.ChangeScene(3 + 4);
                        break;
                    default:
                        Debug.Log($"Game {parameter} is not recognized. Use RLGL | RT | SIS | LP");
                        break;
                }
            }));

        DeveloperConsole.Instance.AddCommand(new DevCommand("board", "Return to board", () => {
            SceneChanger.Instance.ChangeScene(1);
        }));
    }
}
