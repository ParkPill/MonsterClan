using System;
using System.Collections;
using System.Collections.Generic;
using CodeStage.AntiCheat.Storage;
using UnityEngine;
using UnityEngine.Events;

public enum TicketTypes
{
    Energy,
    HeartRefillVideo,
    PremiumVideo,
    NormalChest,
    RoyalChest
}
public class TicketManager : MonoBehaviour
{
    #region Singleton
    private static TicketManager _instance;
    public static TicketManager Instance
    {
        get
        {
            if (!_instance)
            {
                _instance = GameObject.FindObjectOfType(typeof(TicketManager)) as TicketManager;
                if (!_instance)
                {
                    GameObject container = new GameObject();
                    container.name = "TicketManager Instance" + UnityEngine.Random.Range(0, 1000);
                    _instance = container.AddComponent(typeof(TicketManager)) as TicketManager;
                    DontDestroyOnLoad(_instance);
                }
            }
            return _instance;
        }
    }
    #endregion
    private readonly string _strRefillStartTime = "_RST";
    private readonly string _strTicketCount = "_TC";
    public Dictionary<TicketTypes, DateTime> DicRefillStartTime = new Dictionary<TicketTypes, DateTime>();
    public Dictionary<TicketTypes, int> DicRefillTime = new Dictionary<TicketTypes, int>();
    public Dictionary<TicketTypes, int> DicTicketCount = new Dictionary<TicketTypes, int>();
    public Dictionary<TicketTypes, int> DicTicketCountMax = new Dictionary<TicketTypes, int>();
    public UnityEvent TicketCountChanged = new UnityEvent();
    float _timePassed = 1.1f;
    public bool IsInitialized = false;
    // Start is called before the first frame update
    private void Awake()
    {
        var values = Enum.GetValues(typeof(TicketTypes));
        foreach (TicketTypes type in values)
        {
            // refill start time
            string strKey = GetRefillStartTimeKey(type);
            string strRefillStartTime = ObscuredPrefs.GetString(strKey);
            if (string.IsNullOrEmpty(strRefillStartTime))
            {
                DicRefillStartTime.Add(type, DateTime.Now);
                ObscuredPrefs.SetString(strKey, DateTime.Now.ToString());
            }
            else
            {
                DicRefillStartTime.Add(type, DateTime.Parse(strRefillStartTime));
                Debug.Log(string.Format("type: {0}, start time: {1}", type, DicRefillStartTime[type]));
            }

            // refill time
            if (type == TicketTypes.Energy)
            {
                DicRefillTime.Add(type, 600);
                Debug.Log("key" + type);
                //DicRefillTime.Add(type, 6); // test 
            }
            else if (type == TicketTypes.HeartRefillVideo)
            {
                DicRefillTime.Add(type, 600);
            }
            else if (type == TicketTypes.PremiumVideo)
            {
                DicRefillTime.Add(type, 60 * 60);
                //DicRefillTime.Add(type, 60); // test 
            }
            else if (type == TicketTypes.NormalChest)
            {
                DicRefillTime.Add(type, 60 * 60 * 24);
                //DicRefillTime.Add(type, 16);// test 
            }
            else if (type == TicketTypes.RoyalChest)
            {
                DicRefillTime.Add(type, 60 * 60 * 72);
            }

            // ticket count max
            if (type == TicketTypes.Energy)
            {
                DicTicketCountMax.Add(type, 30);
            }
            else if (type == TicketTypes.HeartRefillVideo)
            {
                DicTicketCountMax.Add(type, 1);
            }
            else if (type == TicketTypes.PremiumVideo)
            {
                DicTicketCountMax.Add(type, 1);
            }
            else if (type == TicketTypes.NormalChest)
            {
                DicTicketCountMax.Add(type, 1);
            }
            else if (type == TicketTypes.RoyalChest)
            {
                DicTicketCountMax.Add(type, 1);
            }

            // ticket count
            strKey = GetTicketCountkey(type);
            DicTicketCount.Add(type, ObscuredPrefs.GetInt(strKey, DicTicketCountMax[type]));
        }
    }
    void Start()
    {
        //OscuredPref.

        ServerManager.Instance.TimeRefreshed += OnTimeRefreshed;
        IsInitialized = true;
    }

    private void OnTimeRefreshed()
    {
        var values = Enum.GetValues(typeof(TicketTypes));
        foreach (TicketTypes type in values)
        {
            for (int i = 0; i < DicTicketCountMax[type]; i++)
            {
                CheckTicketRefilled(type);
            }
        }
    }
    void CheckTicketRefilled(TicketTypes type)
    {
        if (DicTicketCountMax[type] <= DicTicketCount[type])
        {
            return;
        }
        DateTime now = ServerManager.Instance.GetCurrentTime();
        //if(type == TicketType.Heart)
        //{
        //Debug.Log(string.Format("start time: {0}, now: {1}, refill dur: {2}, passed:{3}", DicRefillStartTime[type], now, DicRefillTime[type], (now - DicRefillStartTime[type]).TotalSeconds));
        //}
        if (DicRefillTime[type] - (now - DicRefillStartTime[type]).TotalSeconds <= 0)
        {
            //Debug.Log("check refilled and it is nagarive seconds");
            AddTicket(type, 1);
            DicRefillStartTime[type] = DicRefillStartTime[type].AddSeconds(DicRefillTime[type]);
            string strKey = string.Format("{0}{1}", type.ToString(), _strRefillStartTime);
            ObscuredPrefs.SetString(strKey, DicRefillStartTime[type].ToString());
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (!ServerManager.Instance.IsTimeReady) return;
        _timePassed += Time.deltaTime;
        if (_timePassed >= 1)
        {
            _timePassed -= 1;
        }
        else
        {
            return;
        }
        var values = Enum.GetValues(typeof(TicketTypes));
        foreach (TicketTypes type in values)
        {
            //Debug.Log(string.Format("key not found? {0}", type));
            if (DicTicketCountMax[type] > DicTicketCount[type])
            {
                CheckTicketRefilled(type);
            }
        }
    }
    public void SetTicket(TicketTypes type, int count)
    {
        DicTicketCount[type] = count;
        ObscuredPrefs.SetInt(GetTicketCountkey(type), DicTicketCount[type]);
        TicketCountChanged?.Invoke();//(type, DicTicketCount[type]);
    }
    public void UseTicket(TicketTypes type, int ticketToUse)
    {
        // currently full & will not be full -> so start to refill
        if (DicTicketCountMax[type] <= DicTicketCount[type] && DicTicketCountMax[type] > DicTicketCountMax[type] - ticketToUse)
        {
            DicRefillStartTime[type] = ServerManager.Instance.GetCurrentTime();
            ObscuredPrefs.SetString(GetRefillStartTimeKey(type), DicRefillStartTime[type].ToString());
            //Debug.Log(string.Format("refill start type: {0}, start time: {1}", type, ServerManager.Instance.GetCurrentTime().ToString()));
        }
        DicTicketCount[type] -= ticketToUse;
        ObscuredPrefs.SetInt(GetTicketCountkey(type), DicTicketCount[type]);
        TicketCountChanged?.Invoke();//, DicTicketCount[type]);

        Debug.Log(string.Format("ticket count changed {0}/{1}", DicTicketCount[type], type));
    }
    public string GetTicketCountkey(TicketTypes type)
    {
        return string.Format("{0}{1}", type.ToString(), _strTicketCount);
    }
    public string GetRefillStartTimeKey(TicketTypes type)
    {
        return string.Format("{0}{1}", type.ToString(), _strRefillStartTime);
    }
    public int GetTicket(TicketTypes type)
    {
        return DicTicketCount[type];
    }
    public void AddTicket(TicketTypes type, int ticketToAdd)
    {
        DicTicketCount[type] += ticketToAdd;
        if (DicTicketCount[type] > 1000)
        {
            DicTicketCount[type] = 1000;
        }
        ObscuredPrefs.SetInt(GetTicketCountkey(type), DicTicketCount[type]);
        //Debug.Log("Heart added: " + DicTicketCount[type].ToString() + "total: {0}" + DicTicketCount[type]);
        TicketCountChanged?.Invoke();//, DicTicketCount[type]);
    }
    public int GetTimeLeftForTicketRefill(TicketTypes type)
    {
        //Debug.Log("getTimeleft " + type);
        //Debug.Log("getTimeleft " + DicRefillTime.Count);
        return (int)(DicRefillTime[type] - (ServerManager.Instance.GetCurrentTime() - DicRefillStartTime[type]).TotalSeconds);
    }
    public bool IsRefilling(TicketTypes type)
    {
        return DicTicketCountMax[type] > DicTicketCount[type];
    }
    public int GetTicketMaxCount(TicketTypes type)
    {
        return DicTicketCountMax[type];
    }
    public void SetTicketMaxCount(TicketTypes type, int max)
    {
        DicTicketCountMax[type] = max;
        TicketCountChanged?.Invoke();
    }
}
