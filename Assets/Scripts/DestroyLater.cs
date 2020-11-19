using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyLater : MonoBehaviour
{
    private float _timeLeft = 1;
    // Start is called before the first frame update
    void Start()
    {
        
    }

public void SetTime(float sec)
    {
        _timeLeft = sec;
    }

    // Update is called once per frame
    void Update()
    {
        _timeLeft -= Time.deltaTime;
        if (_timeLeft <= 0)
        {
            Destroy(gameObject);
        }
    }
}
