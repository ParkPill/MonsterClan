using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class TriggerDetector : MonoBehaviour
{
    public UnityAction<Collider2D> TriggerEnter;
    public UnityAction<Collider2D> TriggerExit;
    
    // Start is called before the first frame update
    void Start()
    {
        //TriggerEnter = new UnityAction<Collider2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Debug.Log(string.Format("{0} on trigger detector enter {1}", gameObject.name, collision.name));
        if(TriggerEnter != null)
        {
            TriggerEnter.Invoke(collision);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (TriggerExit != null)
        {
            TriggerExit.Invoke(collision);
        }
    }
}
