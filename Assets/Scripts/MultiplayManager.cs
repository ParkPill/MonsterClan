using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BestHTTP;
using BestHTTP.WebSocket;
using System;
using CodeStage.AntiCheat.Storage;

public enum NetworkCode
{
    JoinOrCreate = '0',
    Msg = '1',
    Quit = '2',
    GameStart = '3',
    UnitMove = '4',
    UnitMoveAndAttack = '5',
    Construct = '6',
    Destroy = '7',
    UnitDead = '8',
    UnitCreated = '9',
    QuickMatch = 'a',
    NotGameStart = 'b'
}
public class MultiplayManager : MonoBehaviour
{
    #region Singleton
    private static MultiplayManager _instance;
    public static MultiplayManager Instance
    {
        get
        {
            if (!_instance)
            {
                _instance = GameObject.FindObjectOfType(typeof(MultiplayManager)) as MultiplayManager;
                if (!_instance)
                {
                    GameObject container = new GameObject();
                    container.name = "MultiplayManager Instance" + UnityEngine.Random.Range(0, 1000);
                    _instance = container.AddComponent(typeof(MultiplayManager)) as MultiplayManager;
                    DontDestroyOnLoad(_instance);
                }
            }

            return _instance;
        }
    }
    #endregion
    public string ServerAddress = "http://222.120.115.95:8105";
    public WebSocket TheWebsocket;
    private bool _quickMatchRequet = false;
    public event OnEventDelegate<string> OnRoomJoined;
    public event OnEventDelegate<string> OnUserQuitRoom;
    public event OnEventDelegate OnGameStarted;
    public bool IsGameStartRequested = false;
    public event OnEventDelegate<string> OnEnemySummon;
    public event OnEventDelegate<NetworkCode, string> OnMessageArrived;
    public string EnemyInfo;
    public List<UnitInfo> EnemyUnitList = new List<UnitInfo>();
    // Start is called before the first frame update
    void Start()
    {
        TheWebsocket = new WebSocket(new Uri(ServerAddress), string.Empty, "echo-protocol");
        
        TheWebsocket.OnOpen += OnWebSocketOpenDelegate;
        TheWebsocket.OnMessage += OnWebSocketMessageDelegate;
        TheWebsocket.OnBinary += OnWebSocketBinaryDelegate;
        TheWebsocket.OnClosed += OnWebSocketClosedDelegate;
        TheWebsocket.OnError += OnWebSocketErrorDelegate;
        
    }
    void OnWebSocketOpenDelegate(WebSocket webSocket)
    {
        Debug.Log("OnWebSocketOpenDelegate");
        if (_quickMatchRequet)
        {
            RequestQuickMatch();
        }
    }
    void OnWebSocketMessageDelegate(WebSocket webSocket, string message)
    {
        Debug.Log("OnWebSocketMessageDelegate: " + message);
        if (message.StartsWith("err") || message.Length == 0)
        {
            Debug.LogError("** Network Err: " + message);
            return;
        }
        
        char code = message[0];
        string msg = message.Substring(1);
        
        if (code == (char)NetworkCode.JoinOrCreate)
        {
            EnemyInfo = msg;
            if (msg.Length > 1)
            {
                string strInfo = msg;
                var array = strInfo.Split(',');
                string name = string.Empty;
                if (array.Length > 0)
                {
                    name = array[0];
                }
                Debug.Log(string.Format("name: {0}, {1}, {2}" ,name, array.Length, array[3]));
                EnemyUnitList.Clear();
                for (int i = 1; i < array.Length; i++)
                {
                    Debug.Log(string.Format("array[{0}]: {1}", i, array[i]));
                    var value = array[i].Split('_');
                    
                    if (value.Length == 2)
                    {
                        int index = int.Parse(value[0]) % 100;
                        int level = int.Parse(value[1]);
                        UnitInfo info = new UnitInfo(0, 0, level, 0, index);
                        EnemyUnitList.Add(info);
                    }
                }
            }
            OnRoomJoined?.Invoke(msg);
        }
        else if (code == (char)NetworkCode.Quit)
        {
            EnemyInfo = string.Empty;
            EnemyUnitList.Clear();
            OnUserQuitRoom?.Invoke(msg);
        }
        else if (code == (char)NetworkCode.GameStart)
        {
            OnGameStarted?.Invoke();
            IsGameStartRequested = true;
        }
        else if( code == (char)NetworkCode.Construct)
        {
            OnEnemySummon?.Invoke(msg);
        }
        else
        {
            OnMessageArrived((NetworkCode)code, msg);
        }
    }
    void OnWebSocketBinaryDelegate(WebSocket webSocket, byte[] data)
    {
        Debug.Log("OnWebSocketBinaryDelegate");
    }
    void OnWebSocketClosedDelegate(WebSocket webSocket, UInt16 code, string message)
    {
        Debug.Log("OnWebSocketClosedDelegate: " + message);
    }
    void OnWebSocketErrorDelegate(WebSocket webSocket, string reason)
    {
        Debug.Log("OnWebSocketErrorDelegate: " + reason);
    }
    public void Connect()
    {
        Debug.Log("try connect");
        TheWebsocket.Open();
    }
    public void Disconnect()
    {
        Debug.Log("try disconnect");
        TheWebsocket.Close();
    }
    public void TryQuickMatch()
    {
        Debug.Log("try quick match;");
        if (TheWebsocket.IsOpen)
        {
            Debug.Log("already open");
            RequestQuickMatch();
        }
        else
        {
            _quickMatchRequet = true;
            Connect();
        }
    }
    private void RequestQuickMatch()
    {
        // requesting data -> 0username_unitdata(34_3:index_level),unitdata
        string name = SaveData.Instance.Data.Name;//ObscuredPrefs.GetString(Consts.Key_Name, "Noname");
        string unitData = string.Empty;
        //int group, index, level;
        UnitInfo info;
        for (int i = 0; i < 8; i++)
        {
            //group = ObscuredPrefs.GetInt(string.Format(Consts.Key_Multiplay_Equip_Group_Foramt, i), -1);
            //index = ObscuredPrefs.GetInt(string.Format(Consts.Key_Multiplay_Equip_Index_Foramt, i), -1);
            //level = ObscuredPrefs.GetInt(string.Format(Consts.Key_UnitLevelFormat, group, index), 0);
            //index = group * 100 + index;
            info = SaveData.Instance.Data.GetDeckItem(i);
            unitData += string.Format(",{0}_{1}", info.Index, info.Level);
        }
        string msg = string.Format("{0}{1}{2}", (char)NetworkCode.QuickMatch, name, unitData);
        Debug.Log("send message: " + msg);
        TheWebsocket.Send(msg);
    }
    public void SendSummon(string content)
    {
        string name = SaveData.Instance.Data.Name;//ObscuredPrefs.GetString(Consts.Key_Name, "Noname");
        string msg = string.Format("{0}{1},{2}", (char)NetworkCode.Construct, name, content);
        Debug.Log("send message: " + msg);
        TheWebsocket.Send(msg);
    }
    public void RequestStartGame()
    {
        string name = SaveData.Instance.Data.Name;//ObscuredPrefs.GetString(Consts.Key_Name, "Noname");
        string msg = string.Format("{0}{1}", (char)NetworkCode.GameStart, name);
        Debug.Log("send message: " + msg);
        TheWebsocket.Send(msg);
    }
    public void RequestCancelStartGame()
    {
        string name = SaveData.Instance.Data.Name;//ObscuredPrefs.GetString(Consts.Key_Name, "Noname");
        string msg = string.Format("{0}{1}", (char)NetworkCode.NotGameStart, name);
        Debug.Log("send message: " + msg);
        TheWebsocket.Send(msg);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
