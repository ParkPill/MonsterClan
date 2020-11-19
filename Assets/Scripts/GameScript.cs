using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GameScript : MonoBehaviour
{
    public Canvas TheCanvas;
    public bool IsDragging = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public virtual void OnDragStarted(DragDropUI drag)
    {

    }

    public virtual void OnDraging(DragDropUI drag, PointerEventData eventData)
    {

    }

    public virtual void OnDrop(DragDropUI drag)
    {

    }
    public virtual void OnDragEnded()
    {
        
    }
    public void OnDragObjectEnded()
    {

    }
    public void OnDragStarted(DragDrop drag)
    {

    }
    public void OnDrop(DragDrop drag)
    {

    }
    public bool IsPopupOn()
    {
        return false;
    }
}

