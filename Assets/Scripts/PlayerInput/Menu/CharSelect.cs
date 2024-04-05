using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CharSelect : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    [SerializeField]
    GameObject charPrefab;

    public void OnSelect(BaseEventData eventData)
    {
        this.gameObject.SetActive(true);
        Debug.Log("selected");
    }

    public void OnDeselect(BaseEventData eventData)
    {
        this.gameObject.SetActive(false);
        Debug.Log("deselected");
    }
}
