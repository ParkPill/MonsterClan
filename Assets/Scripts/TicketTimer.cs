using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TicketTimer : MonoBehaviour
{
    public Text Lbl;
    public TicketTypes TicketType;
    float _timer = 0;
    public string FormatKey;
    public List<GameObject> AvailableShowList = new List<GameObject>();
    public List<GameObject> HideWithTimerList = new List<GameObject>();
    // Start is called before the first frame update
    void Start()
    {
        Lbl = GetComponent<Text>();
        OnTicketCountChanged();
        TicketManager.Instance.TicketCountChanged.AddListener(OnTicketCountChanged);
    }
    void OnTicketCountChanged()
    {
        int ticket = TicketManager.Instance.GetTicket(TicketType);
        //Debug.Log(string.Format("on ticket count changed {0}/{1}", ticket, TicketType));
        foreach (GameObject obj in AvailableShowList)
        {
            obj.SetActive(ticket > 0);
        }
        _timer = 0;

        int max = TicketManager.Instance.GetTicketMaxCount(TicketType);
        Debug.Log(string.Format("on ticket count changed {0}/{1}", ticket, max));
        Lbl.gameObject.SetActive(ticket < max);
        foreach (GameObject obj in HideWithTimerList)
        {
            obj.SetActive(Lbl.gameObject.activeSelf);
        }
    }

    // Update is called once per frame
    void Update()
    {
        _timer -= Time.deltaTime;
        if (_timer > 0)
        {
            return;
        }
        _timer += 0.2f;
        int max = TicketManager.Instance.GetTicketMaxCount(TicketType);
        int ticket = TicketManager.Instance.GetTicket(TicketType);
        Lbl.gameObject.SetActive(ticket < max);
        if (ticket < max)
        {
            int timeLeft = TicketManager.Instance.GetTimeLeftForTicketRefill(TicketType);
            if (string.IsNullOrEmpty(FormatKey))
            {
                Lbl.text = GameManager.Instance.GetTimeLeftString(timeLeft);
            }
            else
            {
                Lbl.text = string.Format(LanguageManager.Instance.GetText(FormatKey), GameManager.Instance.GetTimeLeftString(timeLeft));
            }
        }
    }
}
