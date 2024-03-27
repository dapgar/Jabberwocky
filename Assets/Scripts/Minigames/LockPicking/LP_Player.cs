using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LP_Player : MonoBehaviour
{
    enum State
    {
        idle,
        wrong,
        waiting
    }

    float timer = 0;
    float timeBeforeNewCode = .4f;
    float timeForWrongAnswer = .6f;

    State state;

    int playerIndex;

    [SerializeField]
    GameObject lockPrefab;
    Queue<GameObject> locks = new Queue<GameObject>();
    GameObject lockObject;

    Queue<LP_CodeChar> lockCode = new Queue<LP_CodeChar>();
    GameObject codeObject;

    int codeLengthBase = 3;
    int increasePerCompletedCode = 1;
    int completedCodes = 0;
    int codesToWin = 5;

    Animator animator;

    [SerializeField]
    private GameObject testArrowPrefab;
    //private GameObject testArrow;

    private bool buttonPressed = false;
    //private float jumpPower = 0.0f;


    private int weightBase = 2;
    private int weightIncrease = 1;
    private int weightDecrease = -3;
    private int[] weights = new int[4];

    //private int[] dir = {0,1,2,3};
    private Vector2[] dir = { new Vector2(0, 1), new Vector2(0, -1), new Vector2(-1, 0), new Vector2(1, 0) };

    private Vector3 moveDirection = Vector3.zero;
    private Vector2 inputVector = Vector2.zero;

    private Vector2 testVector = Vector2.zero;

    private void Start()
    {
        PlayerSetup();
        //dir[0] = new Vector2(0, 1); //up
        //dir[1] = new Vector2(0, -1); //down
        //dir[2] = new Vector2(-1, 0); //left
        //dir[3] = new Vector2(1, 0); //right

        //testArrow = Instantiate(testArrowPrefab);
        SetupLocks();
        WaitForNewCode();
    }

    void WaitForNewCode()
    {
        timer = timeBeforeNewCode;
        state = State.waiting;
    }

    void WrongAnswer()
    {
        timer = timeForWrongAnswer;
        state = State.wrong;
        if (animator) animator.SetTrigger("Dead");
        List<LP_CodeChar> arrows = lockCode.ToList();

        for (int i = 0; i < arrows.Count; i++)
        {
            arrows[i].Wrong();
        }
    }

    void PlayerSetup()
    {
        playerIndex = GetComponent<PlayerInputHandler>().GetIndex();

        Transform playerTransform = transform.Find($"Player{playerIndex + 1}(Clone)");
        animator = playerTransform.GetComponent<Animator>();
    }

    private void Update()
    {
        switch (state)
        {
            case State.idle:
                break;
            case State.wrong:
                if (timer <= 0)
                {
                    state = State.idle;
                    if (animator) animator.SetTrigger("Dead");

                    List<LP_CodeChar> arrows = lockCode.ToList();

                    for (int i = 0; i < arrows.Count; i++)
                    {
                        arrows[i].Unwrong();
                    }
                }

                timer -= Time.deltaTime;
                break;
            case State.waiting:
                if (timer <= 0)
                {
                    state = State.idle;
                    NewCode(codeLengthBase + completedCodes * increasePerCompletedCode);
                }

                timer -= Time.deltaTime;
                break;
        }
    }

    void SetupLocks()
    {
        lockObject = new GameObject();
        lockObject.transform.position = new Vector3(this.transform.position.x, this.transform.position.y + 1, this.transform.position.z + 1.0f);

        float spacing = .5f;
        for (int i = 0; i < codesToWin; i++)
        {
            float xPos = -((spacing * (codesToWin - 1)) / 2) + spacing * i;

            GameObject newLock = Instantiate(lockPrefab, new Vector3(lockObject.transform.position.x + xPos, lockObject.transform.position.y, lockObject.transform.position.z), Quaternion.identity, lockObject.transform);
            locks.Enqueue(newLock);
        }
    }

    void NewCode(int codeLength)
    {
        ResetWeights();
        Destroy(codeObject);
        codeObject = new GameObject();
        codeObject.transform.position = new Vector3(this.transform.position.x, this.transform.position.y + 2, this.transform.position.z + .8f);

        float spacing = 1f;
        for (int i = 0; i < codeLength; i++)
        {
            float xPos = -((spacing * (codeLength - 1)) / 2) + spacing * i;

            GameObject arrow = Instantiate(testArrowPrefab, new Vector3(codeObject.transform.position.x + xPos, codeObject.transform.position.y, codeObject.transform.position.z), Quaternion.identity, codeObject.transform);
            arrow.GetComponent<LP_CodeChar>().SetDir(NewDir());
            lockCode.Enqueue(arrow.GetComponent<LP_CodeChar>());
        }
    }

    void ResetWeights()
    {
        for (int i = 0; i < weights.Length; i++)
        {
            weights[i] = weightBase;
        }
    }

    Vector2 NewDir()
    {
        Vector2 dirToReturn = new Vector2(0, 1);
        int randomIndex = -1;

        int randomWeight = Random.Range(0, weights.Sum());
        for (int i = 0;i < weights.Length; i++)
        {
            randomWeight -= weights[i];
            if (randomWeight < 0)
            {
                dirToReturn = dir[i];

                randomIndex = i;
                break;
            }
        }

        // update weights
        for (int i = 0; i < weights.Length; i++)
        {
            if (i == randomIndex)
            {
                //weights[i] += weightDecrease;
                weights[i] = weightBase - weightIncrease;
            }
            else
            {
                weights[i] += weightIncrease;
            }
        }

        return dirToReturn;
        //testVector = Vector2.zero;

        //testVector[Random.Range(0, 2)] = Random.Range(0, 2) * 2 - 1;
        //Debug.Log(testVector);
    }

    void RemoveLock()
    {
        GameObject lockToRemove;
        locks.TryPeek(out lockToRemove);
        if (lockToRemove != null)
        {
            locks.Dequeue();
            lockToRemove.transform.parent = null;
            lockToRemove.AddComponent<Rigidbody>();
            lockToRemove.GetComponent<Rigidbody>().AddForce(new Vector3(Random.Range(-50, 50), Random.Range(-30, 30) + 180, Random.Range(-20, 20)));
            lockToRemove.GetComponent<Rigidbody>().AddTorque(new Vector3(Random.Range(-90, 90), Random.Range(-90, 90), Random.Range(-90, 90)));
        }
    }

    public void OnInputMove(Vector2 direction)
    {
        if (state != State.idle)
        {
            return;
        }
        inputVector = direction;

        LP_CodeChar codeChar;
        lockCode.TryPeek(out codeChar);
        if (codeChar == null)
        {
            return;
        }

        if (codeChar.CheckSolve(direction))
        {
            lockCode.Dequeue();
            if (lockCode.Count <= 0)
            {
                RemoveLock();
                completedCodes++;
                if (completedCodes >= codesToWin)
                {
                    if (animator) animator.SetTrigger("Backflip");
                    return;
                }
                WaitForNewCode();
            }
            if (animator) animator.SetTrigger("SwordPull");
        }
        else if (direction != Vector2.zero)
        {
            WrongAnswer();
        }
    }
}
