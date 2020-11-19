using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragDropUI : MonoBehaviour, IDropHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    public enum DragTypes
    {
        Drag,
        DragOnly,
        Drop
    };
    public int Level = 0;
    public int SlotIndex = 0;
    public int Tag = 0;
    public GameScript TheGameScript;
    public DragTypes DragAction = DragTypes.Drag;
    public Vector3 DragStartPosition;
    public Vector3 DragCurrentPosition;
    
    public void OnBeginDrag(PointerEventData eventData)
    {
        //Debug.Log("drag start position: " + eventData.position);
        if (DragAction == DragTypes.Drag || DragAction == DragTypes.DragOnly)
        {
            //Debug.Log("drag start " + gameObject.name);
            TheGameScript.OnDragStarted(this);
            DragStartPosition = eventData.position / TheGameScript.TheCanvas.scaleFactor;
        }
        
    }
    public void OnDrag(PointerEventData eventData)
    {
        //Debug.Log("drag position: " + eventData.position);
        if (DragAction == DragTypes.Drag || DragAction == DragTypes.DragOnly)
        {
            DragCurrentPosition = eventData.position / TheGameScript.TheCanvas.scaleFactor;
            TheGameScript.OnDraging(this, eventData);
        }
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        //Debug.Log("Drag ended " + gameObject.name);
        if (DragAction == DragTypes.Drag || DragAction == DragTypes.DragOnly)
        {
            TheGameScript.OnDragEnded();
        }
    }
    
    public void OnDrop(PointerEventData eventData)
    {
        //Debug.Log("onDrop " + gameObject.name);
        if (DragAction == DragTypes.Drop)
        {
            TheGameScript.OnDrop(this);
            Debug.Log("drop complete" + gameObject.name);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        //Debug.Log("drad ui start");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

}
