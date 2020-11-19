using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleAndRemove : MonoBehaviour
{
    public Vector3 ScaleTo = new Vector3(1, 1, 1);
    public float ScaleDur = 3;
    private float _scaleTimer = 0;
    public bool RemoveAtEnd = true;
    public GameObject ObjectToScale;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        _scaleTimer += Time.deltaTime;
        GameObject obj = this.gameObject;
        if (ObjectToScale != null)
        {
            obj = ObjectToScale;
        }
        obj.transform.localScale = Vector3.Lerp(obj.transform.localScale, ScaleTo, 0.05f);
        if (_scaleTimer >= ScaleDur && RemoveAtEnd)
        {
            Destroy(gameObject);
        }
    }
}
