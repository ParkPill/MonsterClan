using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIGeneralActions : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void HideGameObject()
    {
        this.gameObject.SetActive(false);
    }
    public void ShowGameObject()
    {
        this.gameObject.SetActive(true);
    }
    public void HideGameObject(GameObject obj)
    {
        obj.gameObject.SetActive(false);
    }
    public void ShowGameObject(GameObject obj)
    {
        obj.gameObject.SetActive(true);
    }
    public void DestroyGameObject()
    {
        Destroy(gameObject);
    }
    public void DestroyGameObject(GameObject obj)
    {
        Destroy(obj);
    }
}
