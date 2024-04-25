using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.UI;
using UnityEngine.UI;

public class ItemButtonScript : MonoBehaviour {

    [SerializeField]
    private MultiplayerEventSystem eventSystem;

    void Update() {
        if (eventSystem.currentSelectedGameObject == gameObject) {
            gameObject.transform.localScale = new Vector3(2f, 2f, 2f);
        }
        else {
            gameObject.transform.localScale = new Vector3(1f, 1f, 1f);
        }
    }
}
