using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Draggable : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public GameObject pf_dragComponent;

    GameObject spawnedObject;
    // Start is called before the first frame update
    public void OnDrag(PointerEventData eventData)
    {
        Vector2 current = Camera.main.ScreenToWorldPoint(eventData.position);
        spawnedObject.transform.position = current;

    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        spawnedObject = Instantiate(pf_dragComponent, FindObjectOfType<TimeMaster>().transform);
        FindObjectOfType<TimeMaster>().addToDefenses(spawnedObject);
        spawnedObject.GetComponent<Defense>().dragged = true;
    } 
    public void OnEndDrag(PointerEventData eventData)
    {

    }
    public void OnDrop(PointerEventData eventData)
    {

    }
}
