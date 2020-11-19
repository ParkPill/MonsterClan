using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragDrop : MonoBehaviour, IBeginDragHandler
{
    private Vector2 _startPos;
    BoxCollider2D _collider;
    public GameScript TheGameScript;
    public int Index = 0;
    private bool _isDraggingMe = false;
    private bool IsDragging
    {
        get
        {
            return TheGameScript.IsDragging;
        }
        set
        {
            TheGameScript.IsDragging = value;
            _isDraggingMe = value;
            Debug.Log("draging " + value.ToString() + "/" + gameObject.name);
        }
    }

    private void Awake()
    {
        _collider = GetComponent<BoxCollider2D>();
    }
    void Start()
    {

    }

    private void OnMouseDown()
    {
        Debug.Log("mouse down " + gameObject.name);
        if (TheGameScript.IsPopupOn())
        {
            return;
        }
        Vector3 mousePos = Input.mousePosition;
        mousePos = Camera.main.ScreenToWorldPoint(mousePos);

        _startPos = new Vector2(mousePos.x - transform.localPosition.x, mousePos.y - transform.localPosition.y);

        TheGameScript.OnDragStarted(this);
        IsDragging = true;
    }
    private void OnMouseUp()
    {
        IsDragging = false;
        if (TheGameScript.IsPopupOn())
        {
            return;
        }
        TheGameScript.OnDragObjectEnded();
    }
    // Update is called once per frame
    void Update()
    {
        //if (IsDragging)
        {
            Vector3 mousePos = Input.mousePosition;
            mousePos = Camera.main.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, 10));
            if (_isDraggingMe)
            {
                //Debug.Log(string.Format("dragging me {0}/{1}", _startPos, mousePos));
                gameObject.transform.localPosition = new Vector3(mousePos.x - _startPos.x, mousePos.y - _startPos.y, 0);
                
            }
            else
            {
                //if (this.GetComponent<UnitBase>().BelongTo == Rooms.MergeRoom && Input.GetMouseButtonUp(0))
                if (Input.GetMouseButtonUp(0))
                {
                    if (_collider.bounds.Contains(new Vector3(mousePos.x, mousePos.y, _collider.transform.position.z)))
                    {
                        TheGameScript.OnDrop(this);
                        
                        //Debug.Log("draging on " + gameObject.name);
                    }
                }
            }
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        Debug.Log("drag begin");
    }
}
