using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InstantMessageUI : MonoBehaviour
{
    public Text Text;
    public float TargetY = 34;
    public float HideY = -34;
    public float FloatingTime = 3;
    private float _floatTimeChecker = 0;
    public float MoveSpeed = 3;
    // Start is called before the first frame update
    void Start()
    {
        if(Text == null)
        {
            Text = transform.Find("Text").GetComponent<Text>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (_floatTimeChecker > 0)
        {
            Vector3 targetPos = new Vector3(transform.position.x, TargetY, transform.position.z);
            transform.position = Vector3.MoveTowards(transform.position, targetPos, MoveSpeed);
            if ((transform.position - targetPos).sqrMagnitude < 1)
            {
                _floatTimeChecker -= Time.deltaTime;
                if (_floatTimeChecker <= 0)
                {

                }
            }
        }
        else
        {
            Vector3 targetPos = new Vector3(transform.position.x, HideY, transform.position.z);
            transform.position = Vector3.MoveTowards(transform.position, targetPos, MoveSpeed);
            if ((transform.position - targetPos).sqrMagnitude < 1)
            {
                gameObject.SetActive(false);
            }
        }
        
    }
    
    public void ShowMessage(string msg)
    {
        Text.font = LanguageManager.Instance.GetFont();
        Text.text = msg;
        _floatTimeChecker = FloatingTime;
        gameObject.SetActive(true);
    }
}
