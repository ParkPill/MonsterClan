using System.Collections;
using System.Collections.Generic;
using CodeStage.AntiCheat.Storage;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum Teams
{
    AI,
    PVPEnemy,
    Hero
}
public class CardRefill : MonoBehaviour
{
    public float RefillTime = 2;
    private float _mpRefillTimer = 0;
    public Transform CardContainer;
    List<UnitInfo> _unitList = new List<UnitInfo>();
    List<GameObject> _cardPresetList = new List<GameObject>();
    List<GameObject> _cardList= new List<GameObject>();
    public Vector3 FirstCardPosition;
    public float CardXGap;
    public Teams TheTeam;
    public int MP = 0;
    public int MPMax = 20;
    public Slider MpBar;
    public Vector2 EnemyPosMin;
    public Vector2 EnemyPosMax;
    public TextMeshProUGUI lblMpCount;
    public BattleScript TheBattleScript;
    public Material GrayscaleMat;
    // Start is called before the first frame update
    void Start()
    {
        if (TheTeam == Teams.Hero)
        {
            SetHeroCard();
        }
        else if(TheTeam == Teams.AI){
            if (GameManager.Instance.EnemyUnitList.Count > 7)
            {
                SetScheduledAI();
            }
            else
            {
                SetAICard(GameManager.Instance.AILevel);
            }
        }else if(TheTeam == Teams.PVPEnemy) {

        }
        UpdateAvailableCards();
    }
    /// <summary>
    /// level is 1 to 9
    /// </summary>
    /// <param name="level"></param>
    public void SetAICard(int level)
    {
        int indexRange = level < 5 ? 8 : 12;

        _unitList.Clear();
        int index;
        bool exist = false;
        int loopCounter = 0;
        while (_unitList.Count < 8)
        {
            index = UnityEngine.Random.Range(0, indexRange);
            exist = false;
            foreach(UnitInfo info in _unitList)
            {
                if(info.Index == index) {
                    exist = true;
                    break;
                }
            }
            if (!exist)
            {
                _unitList.Add(new UnitInfo(0, 0, level, 0, index));
            }
            loopCounter++;
            if (loopCounter > 10000)
            {
                break;
            }
        }
    }
    public void SetScheduledAI()
    {
        _unitList.Clear();
        for (int i = 0; i < GameManager.Instance.EnemyUnitList.Count; i++)
        {
            _unitList.Add(GameManager.Instance.EnemyUnitList[i]);
        }
    }
    public void SetHeroCard()
    {
        GameObject card;
        _unitList.Clear();
        for (int i = 0; i < 8; i++)
        {
            //group = ObscuredPrefs.GetInt(string.Format(Consts.Key_Multiplay_Equip_Group_Foramt, i), -1);
            //index = ObscuredPrefs.GetInt(string.Format(Consts.Key_Multiplay_Equip_Index_Foramt, i), -1);
            //level = ObscuredPrefs.GetInt(string.Format(Consts.Key_UnitLevelFormat, group, index), 0);
            //Debug.Log(string.Format("{2}. group: {0}/index:{1}", group, index, i));

            UnitInfo info = SaveData.Instance.Data.GetDeckItem(i);
            if (info == null)
            {
                continue;
            }
            _unitList.Add(info);
            card = GameManager.Instance.GetCard(_unitList[i].Index, CardContainer);

            card.name = string.Format("slot{0}", i);
            card.AddComponent<DragDropUI>();
            card.GetComponent<DragDropUI>().TheGameScript = TheBattleScript;
            card.GetComponent<DragDropUI>().SlotIndex = i;
            //_cardPresetList.Add(card);
            //card.transform.SetParent(CardContainer);
            _cardList.Add(card);
        }
    }
    
    // Update is called once per frame
    void Update()
    {
        float dt = Time.deltaTime;
        if (MP < MPMax)
        {
            _mpRefillTimer += dt;
            if (TheTeam == Teams.Hero)
            {
                float amount = _mpRefillTimer / RefillTime;
                //Debug.Log("mp amount: " + amount);
                MpBar.value = amount;
            }
            if (_mpRefillTimer >= RefillTime)
            {
                _mpRefillTimer -= RefillTime;
                MP++;
                UpdateAvailableCards();
                //AddCard();

                if (TheTeam == Teams.AI)
                {
                    DoAIMove();
                }
                else if (TheTeam == Teams.Hero)
                {
                    lblMpCount.text = MP.ToString();
                }
            }
        }
    }
    public void UpdateAvailableCards()
    {
        foreach(GameObject card in _cardList)
        {
            int cost = int.Parse(card.transform.Find("lblCount").GetComponent<TextMeshProUGUI>().text);
            card.transform.Find("imgPotion").GetComponent<Image>().material = cost <= MP ? null : GrayscaleMat;
        }
    }
    private void DoAIMove()
    {
        bool doMove = UnityEngine.Random.Range(0, 2) == 0;
        Debug.Log("do ai move " + doMove);
        if (doMove)
        {
            int index = UnityEngine.Random.Range(0, _cardList.Count);
            int cost = GetCost(_unitList[index]);
            Debug.Log("do ai move in " + index);
            if (MP >= cost)
            {
                MP -= cost;
                SummonUnit(_unitList[index].Index, _unitList[index].Level);
            }
        }
    }
    public void SummonUnit(int index, int level)
    {
        float x = UnityEngine.Random.Range(EnemyPosMin.x, EnemyPosMax.x);
        float y = UnityEngine.Random.Range(EnemyPosMin.x, EnemyPosMax.x);

        Debug.Log("summon ai move in ");

        TheBattleScript.Summon(new Vector3(x, y, -0.2f), index, level, Teams.AI, TheBattleScript.FrameCounter + 60);
    }
    public int GetCost(UnitInfo info)
    {
        return info.Index;
    }
    public int GetCost(int group, int index)
    {
        return index;
    }
    public void AddCard()
    {
        int slot = UnityEngine.Random.Range(0, 8);
        GameObject card = Instantiate(_cardPresetList[slot]);
        card.transform.SetParent(CardContainer);
        _cardList.Add(card);
        //UpdateCardPositions();
    }
    public void UpdateCardPositions()
    {
        int index = 0;
        foreach(GameObject card in _cardList)
        {
            card.transform.localPosition = new Vector3(FirstCardPosition.x + CardXGap * index, FirstCardPosition.y, 0);
            index++;
        }
    }
}
