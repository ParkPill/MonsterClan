using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using CodeStage.AntiCheat.ObscuredTypes;
using CodeStage.AntiCheat.Storage;
using UnityEngine;

public class SaveData : MonoBehaviour
{
    private static SaveData _instance;
    public static SaveData Instance
    {
        get
        {
            if (!_instance)
            {
                _instance = GameObject.FindObjectOfType(typeof(SaveData)) as SaveData;
                if (!_instance)
                {
                    GameObject container = new GameObject();
                    container.name = "SaveData Instance" + UnityEngine.Random.Range(0, 1000);
                    _instance = container.AddComponent(typeof(SaveData)) as SaveData;
                    _instance.Load();
                    DontDestroyOnLoad(_instance);
                }
            }
            return _instance;
        }
    }
    float _timer = 0;
    public PlayerData Data;
    // Start is called before the first frame update
    void Start()
    {


    }
    public static byte[] SerializeObject<T>(T serializableObject)
    {
        T obj = serializableObject;

        IFormatter formatter = new BinaryFormatter();
        using (MemoryStream stream = new MemoryStream())
        {
            formatter.Serialize(stream, obj);
            return stream.ToArray();
        }
    }

    public static T DeserializeObject<T>(byte[] serilizedBytes)
    {
        IFormatter formatter = new BinaryFormatter();
        using (MemoryStream stream = new MemoryStream(serilizedBytes))
        {
            return (T)formatter.Deserialize(stream);
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
        _timer = 3;
        if (Data.Changed)
        {
            Save();
        }
    }
    public void Save()
    {
        byte[] bytes = SerializeObject(Data);
        ObscuredPrefs.SetByteArray(Consts.Key_PlayerData, bytes);
        Data.Changed = false;
    }
    public void Load()
    {
        byte[] bytes = ObscuredPrefs.GetByteArray(Consts.Key_PlayerData);
        if (bytes.Length == 0)
        {
            Data = new PlayerData();
        }
        else
        {
            Data = DeserializeObject<PlayerData>(bytes);
        }
        //Debug.Log("BladeUnlock: " + Data.BladeUnlock[9]);
        Data.Changed = false;
    }
    public void Reset()
    {
        Data = new PlayerData();
        Save();
    }
}
[Serializable]
public class PlayerData
{
    public string Name = "NoName";
    public bool Changed = false;
    public int NextID = 0;
    public bool IsVIP = false;
    private ObscuredInt _language;
    public ObscuredInt Language
    {
        get { return _language; }
        set
        {
            _language = value;
            Changed = true;
        }
    }
    private ObscuredInt _gem;
    public ObscuredInt Gem
    {
        get { return _gem; }
        set { _gem = value; Changed = true; }
    }

    private ObscuredInt _gold;
    public ObscuredInt Gold
    {
        get { return _gold; }
        set { _gold = value; Changed = true; }
    }

    private ObscuredInt _energy;
    public ObscuredInt Energy
    {
        get { return _energy; }
        set { _energy = value; Changed = true; }
    }
    private ObscuredInt _stone;
    public ObscuredInt Stone
    {
        get { return _stone; }
        set { _stone = value; Changed = true; }
    }
    private ObscuredInt _currentStage;
    public ObscuredInt CurrentStage
    {
        get { return _currentStage; }
        set { _currentStage = value; Changed = true; }
    }
    private ObscuredInt _slotUnlockedCount;
    public ObscuredInt SlotUnlockedCount
    {
        get { return _slotUnlockedCount; }
        set { _slotUnlockedCount = value; Changed = true; }
    }
    private ObscuredInt _produceLevel;
    public ObscuredInt ProduceLevel
    {
        get { return _produceLevel; }
        set { _produceLevel = value; Changed = true; }
    }

    private ObscuredInt _growthHPLevel;
    public ObscuredInt GrowthHPLevel
    {
        get { return _growthHPLevel; }
        set { _growthHPLevel = value; Changed = true; }
    }
    private ObscuredInt _growthFeverLevel;
    public ObscuredInt GrowthFeverLevel
    {
        get { return _growthFeverLevel; }
        set { _growthFeverLevel = value; Changed = true; }
    }
    private ObscuredInt _growthAutoMergeLevel;
    public ObscuredInt GrowthAutoMergeLevel
    {
        get { return _growthAutoMergeLevel; }
        set { _growthAutoMergeLevel = value; Changed = true; }
    }
    private ObscuredInt _growthEnergyMaxLevel;
    public ObscuredInt GrowthEnergyMaxLevel
    {
        get { return _growthEnergyMaxLevel; }
        set { _growthEnergyMaxLevel = value; Changed = true; }
    }
    private ObscuredInt _growthOfflineRewardMaxLevel;
    public ObscuredInt GrowthOfflineRewardMaxLevel
    {
        get { return _growthOfflineRewardMaxLevel; }
        set { _growthOfflineRewardMaxLevel = value; Changed = true; }
    }
    private ObscuredInt _growthProduceBladeLevelLevel;
    public ObscuredInt GrowthProduceBladeLevelLevel
    {
        get { return _growthProduceBladeLevelLevel; }
        set { _growthProduceBladeLevelLevel = value; Changed = true; }
    }

    public void CheckBoolListCount(List<ObscuredBool> list, int index)
    {
        int loopCounter = 0;
        while (list.Count <= index)
        {
            list.Add(false);
            loopCounter++;
            if (loopCounter > 10000)
            {
                break;
            }
        }
    }
    public List<int> StarCountList = new List<int>();

    public void CheckIntListCount(List<int> list, int index)
    {
        int loopCounter = 0;
        while (list.Count <= index)
        {
            list.Add(0);
            loopCounter++;
            if (loopCounter > 10000)
            {
                break;
            }
        }
    }
    public int GetStarCount(int index)
    {
        CheckIntListCount(StarCountList, index);
        return StarCountList[index];
    }
    public void SetStageStar(int stage, int star)
    {
        while (stage >= StarCountList.Count)
        {
            StarCountList.Add(0);

            Changed = true;
        }
        if (StarCountList[stage] < star)
        {
            StarCountList[stage] = star;
            Changed = true;
        }
    }
    private List<ObscuredBool> _bladeUnlock = new List<ObscuredBool>();
    public ObscuredBool GetBladeUnlock(int index)
    {
        CheckBoolListCount(_bladeUnlock, index);
        return _bladeUnlock[index];
    }
    public void SetBladeUnlock(int index, bool unlock)
    {
        CheckBoolListCount(_bladeUnlock, index);
        _bladeUnlock[index] = unlock;
    }

    private List<ObscuredBool> _bladeNewUnlock = new List<ObscuredBool>();
    public ObscuredBool GetBladeNewUnlock(int index)
    {
        CheckBoolListCount(_bladeNewUnlock, index);
        return _bladeNewUnlock[index];
    }
    public void SetBladeNewUnlock(int index, bool unlock)
    {
        CheckBoolListCount(_bladeNewUnlock, index);
        _bladeNewUnlock[index] = unlock;
    }
    private ObscuredInt _newUnlockedCount;
    public ObscuredInt NewUnlockedCount
    {
        get { return _newUnlockedCount; }
        set { _newUnlockedCount = value; Changed = true; }
    }
    private ObscuredBool _autoMergeOn;
    public ObscuredBool AutoMergeOn
    {
        get { return _autoMergeOn; }
        set { _autoMergeOn = value; Changed = true; }
    }
    public void CheckUnitInfoListCount(List<UnitInfo> list, int index)
    {
        int loopCounter = 0;
        while (list.Count <= index)
        {
            list.Add(null);
            loopCounter++;
            if (loopCounter > 10000)
            {
                break;
            }
        }
    }
    private List<UnitInfo> _inventory = new List<UnitInfo>();
    public int InventoryCount
    {
        get { return _inventory.Count; }
    }
    public void AddInventoryItem(UnitInfo info)
    {
        UnitInfo existInfo = null;
        foreach (UnitInfo i in _inventory)
        {
            if (i.Index == info.Index)
            {
                existInfo = i;
                break;
            }
        }
        if (existInfo != null)
        {
            existInfo.Count++;
        }
        else
        {
            if (info.Count == 0)
            {
                info.Count = 1;
            }
            _inventory.Add(info);
        }
        Changed = true;
    }
    public void AddInventoryItem(int index)
    {
        AddInventoryItem(new UnitInfo(index));
    }
    public void RemoveInventoryItem(UnitInfo info)
    {
        _inventory.Remove(info);
    }
    public UnitInfo GetInventoryItem(int index)
    {
        //CheckUnitInfoListCount(_inventory, index);
        if (_inventory.Count > index)
        {
            return _inventory[index]; 
        }
        else
        {
            return null;
        }
    }
    public int GetInventoryItemCount(int index)
    {
        foreach (UnitInfo info in _inventory)
        {
            if (info.Index == index)
            {
                return info.Count;
            }
        }
        return -1;
    }
    public void SetInventoryItem(int index, UnitInfo info)
    {
        //CheckUnitInfoListCount(_inventory, index);
        _inventory[index] = info;
    }

    private List<UnitInfo> _deck = new List<UnitInfo>();
    public int DeckCount
    {
        get { return _deck.Count; }
    }
    public void AddDeckItem(UnitInfo info)
    {
        _deck.Add(info);
        Changed = true;
    }
    public void RemoveDeckItem(UnitInfo info)
    {
        _deck.Remove(info);
    }
    public UnitInfo GetDeckItem(int index)
    {
        //CheckUnitInfoListCount(_deck, index);
        if (_deck.Count > index)
        {
            return _deck[index];
        }
        else
        {
            return null;
        }
    }
    public void SetDeckItem(int index, UnitInfo info)
    {
        CheckUnitInfoListCount(_deck, index);
        _deck[index] = info;
        Changed = true;
    }
    public int GetNextIDAndPlus()
    {
        int id = NextID;
        NextID++;
        return id;
    }
}
