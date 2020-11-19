using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using CodeStage.AntiCheat.ObscuredTypes;
using CodeStage.AntiCheat.Storage;
using UnityEngine;
using UnityEngine.Events;
using UnityQuickSheet;

public delegate void OnEventDelegate();
public delegate void OnEventDelegate<T1>(T1 arg1);
public delegate void OnEventDelegate<T1, T2>(T1 arg1, T2 arg2);
public enum EnemyTypes
{
    Normal0 = 0,
    Speedy0,
    Normal1,
    Speedy1,
    Boss,
    MiddleRange0,
    MiddleRange1
}
public enum Attributes
{
    Earth = 0,
    Water,
    Fire,
    Dark
}
public class DataManager : MonoBehaviour
{
    public UnityEvent OnCurrencyChanged = new UnityEvent();

    private static DataManager _instance;
    public static DataManager Instance
    {
        get
        {
            if (!_instance)
            {
                _instance = GameObject.FindObjectOfType(typeof(DataManager)) as DataManager;
                if (!_instance)
                {
                    GameObject container = new GameObject();
                    container.name = "DataManager";
                    _instance = container.AddComponent(typeof(DataManager)) as DataManager;

                    DontDestroyOnLoad(_instance);
                }
            }
            return _instance;
        }
    }

    public int Gem
    {
        get
        {
            return SaveData.Instance.Data.Gem;
        }
        set
        {
            SaveData.Instance.Data.Gem = value;
            OnCurrencyChanged?.Invoke();
        }
    }

    public BigNum GetEXP()
    {
        BigNum num = new BigNum();

        return num;
    }

    public ObscuredInt Gold
    {
        get
        {
            return SaveData.Instance.Data.Gold;
        }
        set
        {
            SaveData.Instance.Data.Gold = value;
            OnCurrencyChanged?.Invoke();
        }
    }
    public ObscuredInt Energy
    {
        get
        {
            return TicketManager.Instance.GetTicket(TicketTypes.Energy);
        }
        set
        {
            TicketManager.Instance.SetTicket( TicketTypes.Energy, value);
            OnCurrencyChanged?.Invoke();
        }
    }
    

    public string UnitInfoFilePath = "balance_table_CMW - Unit";
    public string EnemyUnitInfoFilePath = "balance_table_CMW - Enemy_pattern";
    private string _stageRewardInfoFilePath = "balance_table_CMW - Stage_reward";
    public float[,] RuneRewardPercentList;
    //public event OnEventDelegate<int, BigNum> OnProduceMadeMoney;
    //public event OnEventDelegate<BigNum> OnSecondCurrencyIsProduced;
    //public OnEventDelegate<int> ProduceUpgraded;
    public List<UnitInfo> UnitInfoList = new List<UnitInfo>();
    public List<List<EnemyTypes>> EnemyCountList = new List<List<EnemyTypes>>();
    public void Init()
    {
        //Debug.Log("init!");
        LoadStageRewardInfo();
        LoadUnitInfo();

        LoadProgress();
    }
    private void LoadStageRewardInfo()
    {
        List<string> _alternateList = new List<string>();
        var ta = Resources.Load(_stageRewardInfoFilePath) as TextAsset;

        string wholeText = ta.text;
        var arrayString = wholeText.Split('\n');


        RuneRewardPercentList = new float[10, 4];
        for (int i = 3; i < 12; i++)
        {
            string line = arrayString[i];
            Debug.Log(line);
            CsvParser parser = new CsvParser(line);
            int index = 0;

            foreach (string str in parser)
            {
                switch (index)
                {
                    case 8:
                        RuneRewardPercentList[i - 3, 0] = float.Parse(str.Substring(0, str.Length - 1));
                        Debug.Log(string.Format("data {0}: {1}", index, RuneRewardPercentList[i - 3, 0]));
                        break;
                    case 9:
                        RuneRewardPercentList[i - 3, 1] = float.Parse(str.Substring(0, str.Length - 1));
                        Debug.Log(string.Format("data {0}: {1}", index, RuneRewardPercentList[i - 3, 1]));
                        break;
                    case 10:
                        RuneRewardPercentList[i - 3, 2] = float.Parse(str.Substring(0, str.Length - 1));
                        Debug.Log(string.Format("data {0}: {1}", index, RuneRewardPercentList[i - 3, 2]));
                        break;
                    case 11:
                        RuneRewardPercentList[i - 3, 3] = float.Parse(str.Substring(0, str.Length - 1));
                        Debug.Log(string.Format("data {0}: {1}", index, RuneRewardPercentList[i - 3, 3]));
                        break;
                    default:
                        break;
                }
                index++;
            }
        }

    }


    public void LoadUnitInfo()
    {
        List<string> _alternateList = new List<string>();
        var ta = Resources.Load(UnitInfoFilePath) as TextAsset;
        //Debug.Log(string.Format("text field: {0}", ta.ToString()));
        string wholeText = ta.text;

        UnitInfoList.Clear();
        var arrayString = wholeText.Split('\n');

        //Debug.Log("lineCount: " + arrayString.Length);
        for (int i = 2; i < arrayString.Length; i++)
        {
            string line = arrayString[i];
            //Debug.Log(string.Format("UnitData: {0}", line));
            CsvParser parser = new CsvParser(line);
            int index = -1;
            UnitInfo data = new UnitInfo();
            foreach (string str in parser)
            {
                //Debug.Log(string.Format("str: '{0}'", str));
                switch (index)
                {
                    case 0:

                        break;
                    case 1:
                        //data.UnitType = (HeroTypes)Enum.Parse(typeof(HeroTypes), str);
                        //Debug.Log(data.ToString());
                        break;
                    default:
                        break;
                }
                index++;
            }
            //UnitDataList.Add(data);
            //Debug.Log(data.ToString());
        }
    }
    /// <summary>
    /// Load saved level and upgrades data
    /// </summary>
    public void LoadProgress()
    {
        //Gold = ObscuredPrefs.GetInt(Consts.Key_Gold, 0);
        //Gem = ObscuredPrefs.GetInt(Consts.Key_Gem, 0);
    }
    public int GetHP(int index, int level)
    {
        return 50 + index * 10 + level * 10;
    }
    public int GetATT(int index, int level)
    {
        return 9 + index * 2 + level * 1;
    }
    public float GetSpeed(int index, int level)
    {
        return 1 - index*0.05f;
    }
    public int GetRange(int index, int level)
    {
        int range = index % 2 == 0 ? 1 : 20;
        //range += level;
        //if(range > 5)
        //{
        //    range = 5;
        //}
        return range;
    }
    public Attributes GetAttributes(int index)
    {
        if (index%4 == 0)
        {
            return Attributes.Earth;
        }else if (index % 4 == 1)
        {
            return Attributes.Water;
        }
        else if (index % 4 == 2)
        {
            return Attributes.Fire;
        }
        else 
        {
            return Attributes.Dark;
        }
    }
    public int GetMPCost(int index)
    {
        //Debug.Log("mp cost: " + (2 + index / 4).ToString());
        return 2 + index/4;
    }
    public int GetLevelUpRequireCount(int level)
    {
        if (level == 0)
        {
            return 2;
        }
        else if (level == 1)
        {
            return 4;
        }
        else if (level == 2)
        {
            return 10;
        }
        else if (level == 3)
        {
            return 20;
        }
        else if (level == 4)
        {
            return 50;
        }
        else if (level == 5)
        {
            return 100;
        }
        else if (level == 6)
        {
            return 200;
        }
        else if (level == 7)
        {
            return 400;
        }
        else if (level == 8)
        {
            return 800;
        }
        else if (level == 9)
        {
            return 1000;
        }
        else if (level == 10)
        {
            return 2000;
        }
        else if (level == 11)
        {
            return 5000;
        }
        return 10;
    }
    public int MaxLevel = 12;
    public int GetLevelUpCost(int level)
    {
        if (level == 0)
        {
            return 5;
        }
        else if (level == 1)
        {
            return 20;
        }
        else if (level == 2)
        {
            return 50;
        }
        else if (level == 3)
        {
            return 150;
        }
        else if (level == 4)
        {
            return 400;
        }
        else if (level == 5)
        {
            return 1000;
        }
        else if (level == 6)
        {
            return 2000;
        }
        else if (level == 7)
        {
            return 4000;
        }
        else if (level == 8)
        {
            return 8000;
        }
        else if (level == 9)
        {
            return 10000;
        }
        else if (level == 10)
        {
            return 20000;
        }
        else if (level == 11)
        {
            return 50000;
        }
        return 10;
    }
}