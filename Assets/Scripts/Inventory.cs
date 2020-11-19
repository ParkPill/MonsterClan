using System.Collections;
using System.Collections.Generic;
using CodeStage.AntiCheat.Storage;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
public enum InventoryUseCases
{
    MultiplayLobby = 0,
    StageLobby,
    Browse
}
public class Inventory : MonoBehaviour
{
    public Transform InventoryContents;
    public Home HomeScript;
    public Transform TabIndicator;
    //public bool IsToPick = false;
    public UnitInfo SelectedInfo;
    public InventoryUseCases UseCase;
    //public UnityEvent OnItemSelected = new UnityEvent();
    public StageLobby StageLobbyScript;
    public MultiplayLobby MultiplayLobbyScript;
    private bool _isInvalidated = false;
    // Start is called before the first frame update
    void Start()
    {
        GameManager.Instance.CardPrefab = InventoryContents.Find("tempCommon").gameObject;
        UpdateInventory(0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private GameObject SelectedGameObject;
    public void OnItemClick(GameObject obj)
    {
        string name = obj.name;
        SelectedGameObject = obj;
        SelectedInfo = obj.GetComponent<InventoryItem>().Info;
        if (UseCase == InventoryUseCases.StageLobby)
        {
            //int underbar = name.IndexOf('_');
            //Debug.Log(string.Format("name: {0}/{1}/{2}", name, underbar, name.Length));
            //SelectedGroup = int.Parse(name.Substring(0, underbar));
            //SelectedIndex = int.Parse(name.Substring(underbar + 1, name.Length - underbar - 1));
            //OnItemSelected?.Invoke();
            StageLobbyScript.OnItemSelected(obj);
            gameObject.SetActive(false);
        }
        else if (UseCase == InventoryUseCases.MultiplayLobby)
        {
            MultiplayLobbyScript.OnItemSelected(obj);
            gameObject.SetActive(false);
        }
        else
        {
            transform.parent.Find("pnlInfo").gameObject.SetActive(true);
            Transform pnl = transform.parent.Find("pnlInfo").Find("Popup");
            Transform item = pnl.Find("Item");
            GameObject card = GameManager.Instance.GetCard(SelectedInfo.Index, item.parent);
            card.transform.position = item.position;
            card.gameObject.SetActive(true);
            int max = DataManager.Instance.GetLevelUpRequireCount(SelectedInfo.Level);
            pnl.parent.Find("imgUpgrade").Find("lblCount").GetComponent<TextMeshProUGUI>().text = string.Format("{0}/{1}", SelectedInfo.Count, max);
            pnl.parent.Find("imgUpgrade").Find("imgFill").GetComponent<Image>().fillAmount = SelectedInfo.Count*1f/ max;
            if (SelectedInfo.Level >= DataManager.Instance.MaxLevel)
            {
                pnl.Find("btnUpgrade").Find("Text_Cost").GetComponent<TextMeshProUGUI>().text = "Lv.MAX";
                pnl.Find("btnUpgrade").GetComponent<Button>().interactable = false;
            }
            else
            {
                int price = DataManager.Instance.GetLevelUpCost(SelectedInfo.Level);
                pnl.Find("btnUpgrade").Find("Text_Cost").GetComponent<TextMeshProUGUI>().text = price.ToString();
                pnl.Find("btnUpgrade").GetComponent<Button>().interactable = price <= DataManager.Instance.Gold;
            }
            pnl.Find("Text_ItemName").GetComponent<TextMeshProUGUI>().text = LanguageManager.Instance.GetText(string.Format("monster name {0}", SelectedInfo.Index));
            pnl.Find("Item_Stats").Find("level").Find("Text (TMP) (1)").GetComponent<TextMeshProUGUI>().text = (SelectedInfo.Level + 1).ToString();
            pnl.Find("Item_Stats").Find("hp").Find("Text (TMP) (1)").GetComponent<TextMeshProUGUI>().text = DataManager.Instance.GetHP(SelectedInfo.Index, SelectedInfo.Level).ToString();
            pnl.Find("Item_Stats").Find("att").Find("Text (TMP) (1)").GetComponent<TextMeshProUGUI>().text = DataManager.Instance.GetATT(SelectedInfo.Index, SelectedInfo.Level).ToString();
            pnl.Find("Item_Stats").Find("attri").Find("Text (TMP) (1)").GetComponent<TextMeshProUGUI>().text = DataManager.Instance.GetAttributes(SelectedInfo.Index).ToString();
            pnl.Find("Item_Stats").Find("cost").Find("Text (TMP) (1)").GetComponent<TextMeshProUGUI>().text = DataManager.Instance.GetMPCost(SelectedInfo.Index).ToString();

            //pnl.Find("lblDesc").GetComponent<TextMeshProUGUI>().text =string.Format(LanguageManager.Instance.GetText("attribute damage desc"), )
        }
    }
    public void OnUpgradeClick()
    {
        int price = DataManager.Instance.GetLevelUpCost(SelectedInfo.Level);
        int count = DataManager.Instance.GetLevelUpRequireCount(SelectedInfo.Level);
        if (count > SelectedInfo.Count)
        {
            GameManager.Instance.TheHome.ShowInstanceMessage(LanguageManager.Instance.GetText("not enough card"));
            return;
        }
        if (DataManager.Instance.Gold >= price)
        {
            DataManager.Instance.Gold -= price;
            SelectedInfo.Count -= count;
            SelectedInfo.Level++;
            OnItemClick(SelectedGameObject);
            _isInvalidated = true;
        }
        else
        {
            GameManager.Instance.TheHome.ShowInstanceMessage(LanguageManager.Instance.GetText("not enough gold"));
        }
    }
    public void OnCloseInfo()
    {
        GameManager.Instance.TheHome.OnBackButtonClick();
        if (_isInvalidated)
        {
            GameManager.Instance.TheHome.OnInventoryClick();
        }
    }
    
    public void OnTabClick(int index)
    {
        UpdateInventory(index);
        Transform pnl = transform.Find("Equipment").Find("Right_Panel");
        TabIndicator.DOMove(new Vector3(pnl.Find("TapMenu").Find(string.Format("Tab{0}", index)).transform.position.x, TabIndicator.position.y, TabIndicator.position.z), 0.3f).SetEase(Ease.OutSine);
        
    }
    public void ShowInventory(InventoryUseCases useCase)
    {
        UseCase = useCase;
        //IsToPick = isToPick;
        transform.Find("Equipment").Find("TopBar").Find("Button_Home").gameObject.SetActive(useCase == InventoryUseCases.Browse);
        transform.Find("Equipment").Find("btnOk").gameObject.SetActive(useCase == InventoryUseCases.Browse);
        gameObject.SetActive(true);
        UpdateInventory(0);
    }
    /// <summary>
    /// index 0:all, 1: earth, 2: water, 3: fire, 4: dark
    /// </summary>
    /// <param name="index"></param>
    public void UpdateInventory(int index)
    {
        foreach (Transform child in InventoryContents)
        {
            if (child.gameObject.name.StartsWith("temp"))
            {
                child.gameObject.SetActive(false);
                //child.Find("GradeFrame").Find("Item").gameObject.SetActive(false);
            }
            else
            {
                Destroy(child.gameObject);
            }
        }

        //for (int i = 0; i < SaveData.Instance.Data.InventoryCount; i++)
        //{
            //UnitInfo info = SaveData.Instance.Data.GetInventoryItem(i);
            //Debug.Log(string.Format("item: {0}/{1}", info.Index, info.Count));
        //}
        //Debug.Log("index: " + index);
        for (int i = 0; i < SaveData.Instance.Data.InventoryCount; i++)
        {
            UnitInfo info = SaveData.Instance.Data.GetInventoryItem(i);
            
            if (info == null || info.Level < 0)
            {
                continue;
            }

            if (UseCase == InventoryUseCases.MultiplayLobby || UseCase == InventoryUseCases.StageLobby)
            {
                PlayerData data = SaveData.Instance.Data;
                bool existInDeck = false;
                for (int j = 0; j < data.DeckCount; j++)
                {
                    if (data.GetDeckItem(j).Index == info.Index)
                    {
                        existInDeck = true;
                        break;
                    }
                }
                if (existInDeck)
                {
                    continue;
                }
            }

            GameObject card = GameManager.Instance.GetCard(info.Index, InventoryContents);
            card.gameObject.SetActive(true);
            card.transform.Find("imgPotion").gameObject.SetActive(false);
            card.transform.Find("lblCount").gameObject.SetActive(false);
            card.transform.Find("imgCountBack").gameObject.SetActive(true);
            card.transform.Find("imgCount").gameObject.SetActive(true);
            int max = DataManager.Instance.GetLevelUpRequireCount(info.Level);
            card.transform.Find("imgCount").GetComponent<Image>().fillAmount = info.Count * 1f / max;
            card.transform.Find("lblCardCount").gameObject.SetActive(true);
            card.transform.Find("lblCardCount").GetComponent<TextMeshProUGUI>().text = string.Format("{0}/{1}", info.Count, max);
            card.AddComponent<InventoryItem>().Info = SaveData.Instance.Data.GetInventoryItem(i);
        }
    }
}