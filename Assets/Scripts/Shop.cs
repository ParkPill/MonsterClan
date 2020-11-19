using System.Collections;
using System.Collections.Generic;
using CodeStage.AntiCheat.Storage;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{
    public GameObject PnlPick;
    public GameObject PnlChest;
    public GameObject PnlGold;
    public GameObject PnlGem;
    public Transform TfTabIndicator;
    public Transform PickContents;
    private int _monsterKinds = 4;
    public List<Vector3> TabIndicatorPositions;
    public Home HomeScript;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void OnPlayAdForChestClick()
    {
        if (IsVideoForFreeChestReady())
        {
            ShowVideo();
        }
    }
    public void OnSecondChestClick(TextMeshProUGUI lbl)
    {
        int price = int.Parse(lbl.text);
        int gem = DataManager.Instance.Gem;
        if (gem >= price)
        {
            DataManager.Instance.Gem -= price;
            Gacha(5);
        }
        else
        {
            ShowMessage(LanguageManager.Instance.GetText("not enough gem"));
        }
    }
    public void OnThirdChestClick(TextMeshProUGUI lbl)
    {
        int price = int.Parse(lbl.text);
        int gem = DataManager.Instance.Gem;
        if (gem >= price)
        {
            DataManager.Instance.Gem -= price;
            Gacha(30);
        }
        else
        {
            ShowMessage(LanguageManager.Instance.GetText("not enough gem"));
        }
    }
    public void Gacha(int count)
    {
        PnlPick.SetActive(true);
        
        foreach (Transform child in PickContents)
        {
            if (child.gameObject.name.StartsWith("temp"))
            {
                child.gameObject.SetActive(false);
                child.Find("GradeFrame").Find("Item").gameObject.SetActive(false);
            }
            else
            {
                Destroy(child.gameObject);
            }
        }
        
        for (int i = 0; i < count; i++)
        {
            int attributeIndex = UnityEngine.Random.Range(0, _monsterKinds);
            float rate = UnityEngine.Random.Range(0.0f, 1.0f)*100;
            int monsterIndex = 0;
            if (rate < 0.001f) // 12
            {
                monsterIndex = 11;
            } 
            else if (rate < 0.1f) // 11
            {
                monsterIndex = 10;
            }
            else if (rate < 1.0f) // 10
            {
                monsterIndex = 9;
            }
            else if (rate < 3.0f) // 9
            {
                monsterIndex = 8;
            }
            else if (rate < 5.0f) // 8
            {
                monsterIndex = 7;
            }
            else if (rate < 8.0f) // 7
            {
                monsterIndex = 6;
            }
            else if (rate < 14.0f) // 6
            {
                monsterIndex = 5;
            }
            else if (rate < 20.0f) // 5
            {
                monsterIndex = 4;
            }
            else if (rate < 28.0f) // 4
            {
                monsterIndex = 3;
            }
            else if (rate < 40.0f) // 3
            {
                monsterIndex = 2;
            }
            else if (rate < 55.0f) // 2
            {
                monsterIndex = 1;
            }
            else if (rate < 1.0f) // 1
            {
                monsterIndex = 0;
            }
            monsterIndex = monsterIndex * 4 + UnityEngine.Random.Range(0, 4);
            GameObject card = GameManager.Instance.GetCard(monsterIndex, PickContents);
            //GameObject icon = null;
            //if (monsterGroup == (int)Attributes.Earth)
            //{
            //    card = Instantiate(PickContents.Find("tempEarth").gameObject);
            //    icon = Instantiate(HomeScript.EarthIconPrefabs[monsterIndex]);
            //}
            //else if (monsterGroup == (int)Attributes.Water)
            //{
            //    card = Instantiate(PickContents.Find("tempWater").gameObject);
            //    icon = Instantiate(HomeScript.WaterIconPrefabs[monsterIndex]);
            //}
            //else if (monsterGroup == (int)Attributes.Fire)
            //{
            //    card = Instantiate(PickContents.Find("tempFire").gameObject);
            //    icon = Instantiate(HomeScript.FireIconPrefabs[monsterIndex]);
            //}
            //else if (monsterGroup == (int)Attributes.Dark)
            //{
            //    card = Instantiate(PickContents.Find("tempDark").gameObject);
            //    icon = Instantiate(HomeScript.DarkIconPrefabs[monsterIndex]);
            //}
            //card.transform.Find("imgGlow").gameObject.SetActive(monsterIndex > 5);
            //icon.transform.SetParent(card.transform);
            //icon.transform.localPosition = new Vector3(1, 6, icon.transform.localPosition.z);
            //card.transform.SetParent(PickContents);
            //card.transform.localScale = new Vector3(1, 1, 1);
            //Transform lblNo = card.transform.Find("lblNo");
            //lblNo.GetComponent<TextMeshProUGUI>().text = (monsterIndex + 1).ToString();
            //lblNo.SetParent(null);
            //lblNo.SetParent(card.transform);
            //lblNo.localPosition = new Vector3(-21, 39, 0);
            card.name = string.Format("card{0}", i);
            card.transform.Find("imgPotion").gameObject.SetActive(false);
            card.transform.Find("lblCount").gameObject.SetActive(false);
            card.transform.Find("lblNo").GetComponent<TextMeshProUGUI>().text = (monsterIndex / 4 + 1).ToString();
            card.SetActive(false);

            Sequence seq = DOTween.Sequence();
            seq.PrependInterval(1 + 0.3f * i).OnComplete(() => { card.SetActive(true); });

            //int newMonsterCount = ObscuredPrefs.GetInt(string.Format(Consts.Key_Monster_Count_Format, monsterGroup, monsterIndex), 0);
            //newMonsterCount++;
            //ObscuredPrefs.SetInt(string.Format(Consts.Key_Monster_Count_Format, monsterGroup, monsterIndex), newMonsterCount);
            SaveData.Instance.Data.AddInventoryItem(monsterIndex);

            if (count - 1 == i)
            {
                Sequence btnSeq = DOTween.Sequence();
                PnlPick.transform.Find("Panel").Find("btnOk").gameObject.SetActive(false);
                btnSeq.PrependInterval(1 + 0.3f * i).OnComplete(() => { PnlPick.transform.Find("Panel").Find("btnOk").gameObject.SetActive(true); });
            }
        }
        Save();
    }
    public void Save()
    {
        SaveData.Instance.Save();
    }
    public void ShowMessage(string msg)
    {
        HomeScript.ShowInstanceMessage(msg);
    }

    public void ShowVideo()
    {

    }

    public bool IsVideoForFreeChestReady()
    {

        return false;
    }
    public void OnTabClick(TextMeshProUGUI lbl)
    {
        string name = lbl.gameObject.name;
        int index = int.Parse(name.Substring(name.Length - 1, 1));
        PnlChest.SetActive(index == 0);
        PnlGold.SetActive(index == 1);
        PnlGem.SetActive(index == 2);
        for(int i = 0; i < 3; i++)
        {
            this.transform.Find("Tab").Find(string.Format("lblTab{0}", i)).GetComponent<TextMeshProUGUI>().color = i == index ? new Color(246/255f, 225 / 255f, 156 / 255f) : new Color(190 / 255f, 181 / 255f, 182 / 255f);
        }
        
        TfTabIndicator.DOMove(TabIndicatorPositions[index], 0.3f).SetEase(Ease.OutSine);
        
        if (index == 0)
        {
            
        }
    }
    public void OnGemClick(int index)
    {
        if(index == 0)
        {
            HomeScript.AddGem(100);
            // test now
        }
        else if (index == 1)
        {
            HomeScript.AddGem(500);
        }
        else if (index == 2)
        {
            HomeScript.AddGem(1200);
        }
        else if (index == 3)
        {
            HomeScript.AddGem(2500);
        }
        else if (index == 4)
        {
            HomeScript.AddGem(7000);
        }
        else if (index == 5)
        {
            HomeScript.AddGem(150000);
        }
    }
    public void OnGoldClick(int index)
    {
        int price = 0;
        if (index == 0)
        {
            price = 10;
        }
        else if (index == 1)
        {
            price = 50;
        }
        else if (index == 2)
        {
            price = 100;
        }
        else if (index == 3)
        {
            price = 200;
        }
        else if (index == 4)
        {
            price = 500;
        }
        else if (index == 5)
        {
            price = 1000;
        }
        if (price > DataManager.Instance.Gem)
        {
            HomeScript.ShowInstanceMessage("not enough gem");
            return;
        }
        DataManager.Instance.Gem -= price;
        if (index == 0)
        {
            HomeScript.AddGold(120);
        }
        else if (index == 1)
        {
            HomeScript.AddGold(550);
        }
        else if (index == 2)
        {
            HomeScript.AddGold(1300);
        }
        else if (index == 3)
        {
            HomeScript.AddGold(2500);
        }
        else if (index == 4)
        {
            HomeScript.AddGold(6500);
        }
        else if (index == 5)
        {
            HomeScript.AddGold(150000);
        }
    }
}
