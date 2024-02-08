//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.InputSystem;
//using static UnityEditor.Experimental.GraphView.GraphView;

//public class ReactionTime : MonoBehaviour
//{
//    float currentTimer;
//    float timeGoal;

//    [SerializeField] private float minTime;
//    [SerializeField] private float maxTime;

//    private float[] playerTime;

//    // Start is called before the first frame update
//    void Start()
//    {
//        timeGoal = -Random.Range(minTime, maxTime);

//        buttonPressed = new bool[players.Length];
//        for (int i = 0; i < buttonPressed.Length; i++)
//        {
//            buttonPressed[i] = false;
//        }

//        StartCoroutine(LightController());
//    }

//    // Update is called once per frame
//    void Update()
//    {
//        MovePlayer(0, Key.Q);
//        MovePlayer(1, Key.R);
//        MovePlayer(2, Key.U);
//        MovePlayer(3, Key.P);

        
//    }

//    private void MovePlayer(int index, Key key)
//    {
//        Keyboard keyboard = Keyboard.current;
//        if (keyboard == null) return; // no keyboard

//        if (keyboard[key].isPressed)
//        {
//            players[index].transform.Translate(new Vector3(0f, 0f, 1) * speed * Time.deltaTime);
//            movingPlayers[index] = true;
//        }

//        // There is no "key released" so it just checks if they're NOT pressing key but were previously moving, if so sets moving to false
//        if (!keyboard[key].isPressed && movingPlayers[index])
//        {
//            movingPlayers[index] = false;
//        }

//        if (players[index].transform.position.z >= finishLineZ)
//        {
//            // TODO: Actual win & safety for that player, disable movement & face them towards camera?
//            Debug.Log($"Player {index + 1} finished");
//        }
//    }

//    private IEnumerator LightController()
//    {
//        while (runLightCoroutine)
//        {
//            float duration = isLightRed ? Random.Range(minRedLightDuration, maxRedLightDuration) : Random.Range(minGreenLightDuration, maxGreenLightDuration);

//            //Debug.Log($"Light is {(isLightRed ? "Red" : "Green")} for {duration} seconds");

//            StartCoroutine(RotateCoroutine(isLightRed ? 0 : 180));

//            yield return new WaitForSeconds(duration);

//            isLightRed = !isLightRed;
//            lightJustSwitched = true;
//        }
//    }
//}
