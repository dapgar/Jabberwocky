using CommandTerminal;
using System.Data.Common;
using UnityEngine;

public class Commands : MonoBehaviour {
    [RegisterCommand(Help = "Moves Player X # spaces", MinArgCount = 2, MaxArgCount = 2)]
    static void MP(CommandArg[] args) {
        int player = args[0].Int;
        int spaces = args[1].Int;

        if (Terminal.IssuedError) return; // Error will be handled by Terminal

        Terminal.Log($"Moving Player {player} {spaces} spaces");
        BoardManager.instance.DevMovePlayer(player, spaces);
    }

    // OPTIONAL: Can add 'Name = "SetGame",' before "Help = ..." to give it a custom name (separate from function name)
    [RegisterCommand(Help = "Next Game: RLGL | RT | SIS | LP", MinArgCount = 1, MaxArgCount = 1)]
    static void SetGame(CommandArg[] args) {
        string parameter = args[0].String;
        if (Terminal.IssuedError) return;

        switch (parameter.ToLower()) {
            case "rlgl":
                GameManager.instance.devMinigameNumber = 0;
                Terminal.Log("Red Light Green Light Set");
                break;
            case "rt":
                GameManager.instance.devMinigameNumber = 1;
                Terminal.Log("Reaction Time Set");
                break;
            case "sis":
                GameManager.instance.devMinigameNumber = 2;
                Terminal.Log("Sword In Stone Light Set");
                break;
            case "lp":
                GameManager.instance.devMinigameNumber = 3;
                Terminal.Log("Lock Picking Set");
                break;
            default:
                Debug.LogError($"Game {parameter} is not recognized. Use RLGL | RT | SIS | LP");
                break;
        }
    }

    [RegisterCommand(Help = "Open Game: RLGL | RT | SIS | LP", MinArgCount = 1, MaxArgCount = 1)]
    static void OpenGame(CommandArg[] args) {
        GameManager.instance.devMinigameNumber = -1;
        string parameter = args[0].String;
        if (Terminal.IssuedError) return;

        int buffer = 5;
        switch (parameter.ToLower()) {
            case "rlgl":
                SceneChanger.Instance.ChangeScene(0 + buffer);
                break;
            case "rt":
                SceneChanger.Instance.ChangeScene(1 + buffer);
                break;
            case "sis":
                SceneChanger.Instance.ChangeScene(2 + buffer);
                break;
            case "lp":
                SceneChanger.Instance.ChangeScene(3 + buffer);
                break;
            default:
                Debug.LogError($"Game {parameter} is not recognized. Use RLGL | RT | SIS | LP");
                break;
        }
    }


    [RegisterCommand(Help = "Returns to BoardScene", MinArgCount = 0, MaxArgCount = 0)]
    static void Board(CommandArg[] args) {
        SceneChanger.Instance.ChangeScene(1);
    }

}
