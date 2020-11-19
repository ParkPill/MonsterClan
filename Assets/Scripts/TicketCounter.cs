using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TicketCounter: MonoBehaviour
{
    public Text Lbl;
    public TextMeshProUGUI LblTMP;
    public TicketTypes TicketType;
    float _timer = 0;
    int _lastCounter = -1;
    public bool HoldCount = false;
    public string FormatKey;
    private float _interval = 0.1f;
    // Start is called before the first frame update
    void Start()
    {
        Lbl = GetComponent<Text>();
        LblTMP = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        _timer -= Time.deltaTime;
        if (_timer > _interval)
        {
            return;
        }
        _timer += _interval;
        if (HoldCount)
        {
            return;
        }
        int max = TicketManager.Instance.GetTicketMaxCount(TicketType);
        int ticket = TicketManager.Instance.GetTicket(TicketType);

        if (_lastCounter > ticket)
        {
            _lastCounter--;
        }
        else if (_lastCounter < ticket)
        {
            if (_lastCounter + 10 < ticket)
            {
                _lastCounter += 10;
            }
            else
            {
                _lastCounter++;
            }
        }
        if (Lbl != null)
        {
            Lbl.text = string.Format("{0}/{1}", _lastCounter, max);
        }
        else if(LblTMP != null)
        {
            LblTMP.text = string.Format("{0}/{1}", _lastCounter, max);
        }
    }
}
