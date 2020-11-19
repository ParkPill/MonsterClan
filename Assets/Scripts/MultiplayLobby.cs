using System.Collections;
using System.Collections.Generic;
using CodeStage.AntiCheat.Storage;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MultiplayLobby : MonoBehaviour
{
    private int _selectedSlot = -1;
    public GameObject[] Cards;
    public Home HomeScript;
    public GameObject HeroCardContainer;
    public GameObject EnemyCardContainer;
    List<GameObject> _cardList = new List<GameObject>();
    public TextMeshProUGUI LblMyName;
    public TextMeshProUGUI LblEnemyName;
    List<GameObject> EnmyCardList = new List<GameObject>();
    bool _isEnemyJoined = false;
    bool _isEnemyInfoHandled = false;
    bool _updateEnemyInfoRequested = false;
    bool _amIReady = false;
    bool _isEnemyReady = true;
    bool _listenerRegistered = false;
    public GameObject HeroCardPrefab;
    // Start is called before the first frame update
    void Start()
    {
        UpdateEquip();
        //HomeScript.PnlInventory.GetComponent<Inventory>().OnItemSelected.AddListener(OnItemSelected);
        MultiplayManager.Instance.OnRoomJoined += OnRoomJoined;
        //MultiplayManager.Instance.OnGameStarted += OnEnemyReady;
    }

    // Update is called once per frame
    void Update()
    {
        if (_isEnemyJoined && !_isEnemyInfoHandled)
        {
            _isEnemyInfoHandled = true;
            UpdateEnemyEquip();
        }
        if (_updateEnemyInfoRequested)
        {
            _updateEnemyInfoRequested = false;
            UpdateEnemyEquip();
        }
        if (_isEnemyReady && _amIReady)
        {
            _isEnemyReady = false;
            GameManager.Instance.IsMultiPlay = true;
            GameManager.Instance.IsVsAI = false;
            SceneManager.LoadScene("Battle");
        }
    }
    public void OnItemSelected(GameObject obj)
    {
        if (!gameObject.activeSelf)
        {
            return;
        }
        UnitInfo info = HomeScript.PnlInventory.GetComponent<Inventory>().SelectedInfo;
        //Debug.Log(string.Format("selected {0}", info.Index));
        //ObscuredPrefs.SetInt(string.Format(Consts.Key_Multiplay_Equip_Group_Foramt, _selectedSlot), group);
        //ObscuredPrefs.SetInt(string.Format(Consts.Key_Multiplay_Equip_Index_Foramt, _selectedSlot), index);
        SaveData.Instance.Data.SetDeckItem(_selectedSlot, info);

        UpdateEquip();
    }

    public void OnSlotClick(GameObject obj)
    {
        int index = int.Parse(obj.name.Substring(obj.name.Length - 1, 1));
        Debug.Log(string.Format("clicked slot: {0}", index));
        _selectedSlot = index;
        HomeScript.PnlInventory.GetComponent<Inventory>().ShowInventory(InventoryUseCases.MultiplayLobby);
    }
    public void UpdateEnemyEquip()
    {
        GameObject card;
        foreach (Transform tf in EnemyCardContainer.transform)
        {
            Destroy(tf.gameObject);
        }
        for (int i = 0; i < MultiplayManager.Instance.EnemyUnitList.Count; i++)
        {
            UnitInfo info = MultiplayManager.Instance.EnemyUnitList[i];

            card = GameManager.Instance.GetCard(info.Index, EnemyCardContainer.transform);//Instantiate(HomeScript.CardPrefabs[1 + info.Group % 4]);
            //GameObject temp = Instantiate(HomeScript.GetMonsterIconPrefab(info.Group, info.Index));
            //temp.transform.SetParent(card.transform.Find("GradeFrame"));
            //temp.transform.localScale = new Vector3(1, 1, 1);
            //temp.transform.localPosition = new Vector3(-4, 8, 0);

            card.name = string.Format("slot{0}", i);
            //card.transform.SetParent(EnemyCardContainer.transform);
            card.transform.localScale = new Vector3(1, 1, 1);
        }

        transform.Find("ReadyUI").gameObject.SetActive(true);
        transform.Find("WaitUI").gameObject.SetActive(false);
    }

    public void UpdateEquip()
    {
        for (int i = 0; i < SaveData.Instance.Data.DeckCount; i++)
        {
            if (SaveData.Instance.Data.GetDeckItem(i) == null)
            {
                Debug.Log(string.Format("deck null"));
            }
            else
            {
                Debug.Log(string.Format("deck " + SaveData.Instance.Data.GetDeckItem(i).Index));
            }
        }
        //Debug.Log("Update equip");
        int index;
        int count;
        bool isEmpty;
        GameObject card;
        foreach (Transform tf in HeroCardContainer.transform)
        {
            if (tf.name.Equals("temp"))
            {
                //Debug.Log("continue " + tf.name);
                continue;
            }
            //Debug.Log("destroy " + tf.name);
            Destroy(tf.gameObject);
        }

        UnitInfo info;
        for (int i = 0; i < 8; i++)
        {
            info = null;
            index = -1;
            if (SaveData.Instance.Data.DeckCount > i && SaveData.Instance.Data.GetDeckItem(i) != null)
            {
                info = SaveData.Instance.Data.GetDeckItem(i);
                index = info.Index;
            }

            isEmpty = false;
            if (info == null)
            {
                isEmpty = true;
                //Debug.Log("isempth 1");
            }
            else
            {
                //count = ObscuredPrefs.GetInt(string.Format(Consts.Key_Monster_Count_Format, group, index), 0);
                count = info.Count;
                //Debug.Log("isempth 2");
                if (count <= 0)
                {
                    //Debug.Log("isempth 3");
                    isEmpty = true;
                }
            }
            //Debug.Log(string.Format("is empty {0}, {1}", isEmpty, index));
            card = Instantiate(HeroCardPrefab, HeroCardContainer.transform);
            card.gameObject.SetActive(true);
            GameManager.Instance.SetCard(card, index);
            card.name = string.Format("slot{0}", i);

            //card.transform.SetParent(HeroCardContainer.transform);
            card.transform.localScale = new Vector3(1, 1, 1);
        }
        _listenerRegistered = true;
    }
    public void UpdateEquipOld()
    {
        //Debug.Log("Update equip");
        int group;
        int index = -1;
        int count;
        bool isEmpty;
        GameObject card;
        foreach(Transform tf in HeroCardContainer.transform)
        {
            Destroy(tf.gameObject);
        }
        _cardList.Clear();
        for (int i = 0; i < 8; i++)
        {
            //group = ObscuredPrefs.GetInt(string.Format(Consts.Key_Multiplay_Equip_Group_Foramt, i), -1);
            //index = ObscuredPrefs.GetInt(string.Format(Consts.Key_Multiplay_Equip_Index_Foramt, i), -1);
            //Debug.Log(string.Format("{2}. group: {0}/index:{1}", group, index, i));
            UnitInfo info = SaveData.Instance.Data.GetDeckItem(i);
            isEmpty = false;
            if (info == null)
            {
                isEmpty = true;
                //Debug.Log("isempth 1");
            }
            else
            {
                //count = ObscuredPrefs.GetInt(string.Format(Consts.Key_Monster_Count_Format, group, index), 0);
                count = info.Count;
                //Debug.Log("isempth 2");
                if (count <= 0)
                {
                    //Debug.Log("isempth 3");
                    isEmpty = true;
                }
                index = info.Index;
                Debug.Log("equip index " + index);

            }
            //Debug.Log("isempth " + isEmpty);
            if (isEmpty)
            {
                card = Instantiate(HomeScript.CardPrefabs[0], HeroCardContainer.transform);
            }
            else
            {
                //card = Instantiate(HomeScript.CardPrefabs[1 + group % 4]);
                //GameObject temp = Instantiate(HomeScript.GetMonsterIconPrefab(group, index));
                //temp.transform.SetParent(card.transform.Find("GradeFrame"));
                //temp.transform.localScale = new Vector3(1, 1, 1);
                //temp.transform.localPosition = new Vector3(-4, 8, 0);
                card = GameManager.Instance.GetCard(index, HeroCardContainer.transform);
                card.gameObject.SetActive(true);
            }
            _cardList.Add(card);
            card.name = string.Format("slot{0}", i);
            if (!_listenerRegistered)
            {
                //Debug.Log("update equip add click event listener " + i);
                if (i == 0)
                {
                    card.GetComponent<Button>().onClick.AddListener(() => OnSlotClick(_cardList[0]));
                }
                else if (i == 1)
                {
                    card.GetComponent<Button>().onClick.AddListener(() => OnSlotClick(_cardList[1]));
                }
                else if (i == 2)
                {
                    card.GetComponent<Button>().onClick.AddListener(() => OnSlotClick(_cardList[2]));
                }
                else if (i == 3)
                {
                    card.GetComponent<Button>().onClick.AddListener(() => OnSlotClick(_cardList[3]));
                }
                else if (i == 4)
                {
                    card.GetComponent<Button>().onClick.AddListener(() => OnSlotClick(_cardList[4]));
                }
                else if (i == 5)
                {
                    card.GetComponent<Button>().onClick.AddListener(() => OnSlotClick(_cardList[5]));
                }
                else if (i == 6)
                {
                    card.GetComponent<Button>().onClick.AddListener(() => OnSlotClick(_cardList[6]));
                }
                else if (i == 7)
                {
                    card.GetComponent<Button>().onClick.AddListener(() => OnSlotClick(_cardList[7]));
                }
            }

            //card.transform.SetParent(HeroCardContainer.transform);
            card.transform.localScale = new Vector3(1, 1, 1);
        }
        _listenerRegistered = true;
    }
    public void OnHeroSlotClick(GameObject obj)
    {
        OnSlotClick(obj);
    }
    public void OnQuickMatchClick()
    {
        _isEnemyInfoHandled = false;
        _isEnemyJoined = false;
        transform.Find("LobbyUI").gameObject.SetActive(false);
        transform.Find("WaitUI").gameObject.SetActive(true);
        MultiplayManager.Instance.TryQuickMatch();
    }
    public void StartBattle()
    {
        _amIReady = true;
        transform.Find("ReadyUI").gameObject.SetActive(false);
        transform.Find("WaitUI").gameObject.SetActive(true);
        MultiplayManager.Instance.RequestStartGame();
    }
    public void CloseThis()
    {
        MultiplayManager.Instance.Disconnect();
        gameObject.SetActive(false);
    }
    void OnEnemyReady()
    {
        _isEnemyReady = true;
    }
    void OnRoomJoined(string msg)
    {
        Debug.Log("Lobby room joined: " + msg);
        if (msg.Length > 1)
        {
            _isEnemyJoined = true;
        }
    }
    void OnUserQuit(string str)
    {
        Debug.Log("OnUserQuit: " + str);
        _updateEnemyInfoRequested = true;
    }
}
