using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Display : MonoBehaviour
{
    [SerializeField]
    private GameObject[] displayObjectPrefab;
    private GameObject[] displayObjects;

    GameObject objectOnDisplay;
    public int displayIndex = 0;

    private void Awake()
    {
        displayObjectPrefab = PlayerConfigurationManager.Instance.GetPlayerPrefabs();
        if (displayObjectPrefab.Length != 0)
        {
            
            displayObjects = new GameObject[displayObjectPrefab.Length];
            SpawnObjDisplays();

            PutOnDisplay(displayIndex);
        }
    }

    private void SpawnObjDisplays()
    {
        for (int i = 0; i < displayObjectPrefab.Length; i++)
        {
            displayObjects[i] = Instantiate(displayObjectPrefab[i], transform);
            displayObjects[i].transform.localPosition = Vector3.zero;
            SetUILayer(displayObjects[i]);
            displayObjects[i].SetActive(false);
        }
        transform.localScale = Vector3.one * 200;
    }

    public void DisplayPrev()
    {
        displayIndex += displayObjects.Length - 1;
        displayIndex %= displayObjects.Length;
        SwapDisplay(displayIndex);
        if (PlayerConfigurationManager.Instance.CheckCharPrefabUsed(displayIndex))
        {
            DisplayPrev();
        }
    }

    public void DisplayNext()
    {
        displayIndex++;
        displayIndex %= displayObjects.Length;
        SwapDisplay(displayIndex);
        if (PlayerConfigurationManager.Instance.CheckCharPrefabUsed(displayIndex))
        {
            DisplayNext();
        }
    }

    void SwapDisplay(int index)
    {
        RemoveFromDisplay();
        PutOnDisplay(index);
    }

    void PutOnDisplay(int index)
    {
        objectOnDisplay = displayObjects[index];
        objectOnDisplay.SetActive(true);
    }

    void RemoveFromDisplay()
    {
        if (objectOnDisplay != null)
        {
            objectOnDisplay.SetActive(false);
        }
    }

    void SetUILayer(GameObject obj)
    {
        obj.layer = LayerMask.NameToLayer("UI");
        foreach (Transform child in obj.transform)
        {
            SetUILayer(child.gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        transform.localPosition = new Vector3(transform.localPosition.x, Mathf.Sin(Time.time * 2) * 20, transform.localPosition.z);
    }

    public void SetDisplayIndex(int index)
    {
        if (index < 0)
        {
            return;
        }
        displayIndex = index;
        displayIndex %= displayObjectPrefab.Length;
        SwapDisplay(displayIndex);
    }

    public int GetDisplayIndex()
    {
        return displayIndex;
    }
}
