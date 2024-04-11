using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.UI;
using UnityEngine.UI;

public class ArrowButton : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    [SerializeField]
    GameObject charPrefab;
    [SerializeField]
    public Button prevButton;
    [SerializeField]
    UnityEvent selectEvent;
    [SerializeField]
    private MultiplayerEventSystem eventSystem;

    private void Awake()
    {
        
    }

    public void OnSelect(BaseEventData eventData)
    {
        //if (eventData.selectedObject == this.gameObject)
        //{
        //    selected = true;
        //    selectEvent.Invoke();

        //    StartCoroutine(SelectPrev());
            
        //}
        //Debug.Log(eventData.selectedObject);
    }

    IEnumerator SelectPrev()
    {
        yield return new WaitForSeconds(.1f);
        prevButton.Select();
    }

    public void OnDeselect(BaseEventData eventData)
    {
        //if (eventData.selectedObject == this.gameObject)
        //{
        //    selected = false;
        //    Debug.Log(selected);
        //}
    }

    void Test()
    {
        Debug.Log(eventSystem.currentSelectedGameObject);
    }

    public void Update()
    {
        if (eventSystem.currentSelectedGameObject == this.gameObject)
        {
            selectEvent.Invoke();
            eventSystem.SetSelectedGameObject(prevButton.gameObject);
            
        }

    }
}
