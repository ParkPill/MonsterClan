using System.Collections;
using System.Collections.Generic;
using CodeStage.AntiCheat.Storage;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class BattleScript : GameScript
{
    public GameObject AI;
    public GameObject MarkerUI;
    public GameObject MarkerGround;
    public GameObject GroundEffect;
    public float ClickDistance = 5000;
    public OnEventDelegate<UnitBehaviour> OnUnitSummon;
    public List<UnitBehaviour> EnemyList = new List<UnitBehaviour>();
    public List<UnitBehaviour> HeroList = new List<UnitBehaviour>();
    public List<UnitBehaviour> ReadyUnitList = new List<UnitBehaviour>();
    public GameObject HeroTemp;
    public GameObject EnemyTemp;
    private DragDropUI _dragStarter;
    private bool _dragStarted = false;
    public CardRefill CardManager;
    public GameObject CastleLeft;
    public GameObject CastleRight;
    public GameObject PnlDraw;
    public GameClear GameClearScript;
    public GameOver GameOverScript;
    private float _gameTimer = 180;
    public TextMeshProUGUI LblTimer;
    public bool IsGameOver = false;
    public int FrameCounter = 0;
    public bool IsMultiPlay = false;
    private bool _isGameStarted = false;
    private int _countDown;
    public TextMeshProUGUI LblCountDown;
    public GameObject CardCover;
    public GameObject[] MissileList;
    public GameObject[] MissileBigList;
    public GameObject[] MissileHitList;
    public ObjectPooler ThePool;
    // Start is called before the first frame update
    void Start()
    {
        if (GameManager.Instance.IsVsAI)
        {
            AI.SetActive(true);
        }
        IsMultiPlay = GameManager.Instance.IsMultiPlay;
        if (IsMultiPlay)
        {
            MultiplayManager.Instance.OnEnemySummon += OnEnemySummon;
            CastleRight.GetComponent<UnitBehaviour>().TheTeam = Teams.PVPEnemy;
            MultiplayManager.Instance.RequestStartGame();
        }
        else
        {
            MultiplayManager.Instance.IsGameStartRequested = true;
        }
    }
    void OnEnemyReady()
    {
        _isGameStarted = true;
    }

    protected void OnEnemySummon(string msg)
    {
        //string.Format("{0}/{1}/{2}/{3}", group * 100 + index + level * 1000, _frameCounter + 60, (int)mousePos2D.x, (int)mousePos2D.y));
        //string msg = string.Format("{0}{1},{2}", (char)NetworkCode.Construct, name, content);
        Debug.Log("msg:  " + msg);
        string strInfo = msg;
        var array = strInfo.Split(',');
        string name = string.Empty;
        if (array.Length > 0)
        {
            name = array[0];
        }
        Debug.Log(string.Format("name: {0}, {1}", name, array));
        string strValue = array[1];
        var valueArray = strValue.Split('/');
        int value = int.Parse(valueArray[0]);
        int frameCount = int.Parse(valueArray[1]);
        int x = int.Parse(valueArray[2]);
        int y = int.Parse(valueArray[3]);
        int index = value % 1000;
        int level = value / 1000;
        Summon(new Vector3(x, y, -0.2f), index, level, Teams.PVPEnemy, frameCount);
    }
    IEnumerator StartCountDown()
    {
        int totalCount = 3;
        int count = totalCount;
        for (int i = 0; i < totalCount; i++)
        {
            LblCountDown.text = count.ToString();
            // play sound
            yield return new WaitForSeconds(1);
            count--;
        }
        LblCountDown.text = "BATTLE!";
        Sequence seq = DOTween.Sequence();
        float width = TheCanvas.GetComponent<RectTransform>().rect.width * TheCanvas.GetComponent<RectTransform>().localScale.x;
        float x = LblCountDown.transform.position.x;
        float shakeBitDur = 0.06f;
        float disappearDur = 0.5f;
        seq.Append(LblCountDown.transform.DOMoveX(x + 7, shakeBitDur));
        seq.Append(LblCountDown.transform.DOMoveX(x - 7, shakeBitDur));
        seq.Append(LblCountDown.transform.DOMoveX(x + 7, shakeBitDur));
        seq.Append(LblCountDown.transform.DOMoveX(x - 7, shakeBitDur));
        seq.Append(LblCountDown.transform.DOMoveX(x + 7, shakeBitDur));
        seq.Append(LblCountDown.transform.DOMoveX(x - 7, shakeBitDur));
        seq.Append(LblCountDown.transform.DOMoveX(x + 7, shakeBitDur));
        seq.Append(LblCountDown.transform.DOMoveX(x - 7, shakeBitDur));
        seq.Append(LblCountDown.transform.DOMoveX(x - width, disappearDur));
        // play sound
        CardCover.transform.DOMoveX(CardCover.transform.position.x + width, disappearDur).SetEase(Ease.InBack);
    }
    // Update is called once per frame
    void Update()
    {
        if (!_isGameStarted)
        {
            if (MultiplayManager.Instance.IsGameStartRequested)
            {
                _isGameStarted = true;
                StartCoroutine(StartCountDown());
            }
            return;
        }
        if (IsGameOver)
        {
            return;
        }
        if (CastleLeft == null)
        {
            GameOver();
        }
        else if(CastleRight == null) {
            GameClear();
        }
        _gameTimer -= Time.deltaTime;
        LblTimer.text = ((int)_gameTimer).ToString();
        if (_gameTimer < 0)
        {
            TimeOver();
        }
    }
    void FixedUpdate()
    {
        if (!_isGameStarted)
        {
            return;
        }
        FrameCounter++;
        List<UnitBehaviour> unitsToRemove = new List<UnitBehaviour>();
        foreach(UnitBehaviour unit in ReadyUnitList)
        {
            unit.FrameCountLeftToArrive--;
            if (unit.FrameCountLeftToArrive <= 0)
            {
                unitsToRemove.Add(unit);
            }
        }
        foreach(UnitBehaviour unit in unitsToRemove)
        {
            ReadyUnitList.Remove(unit);
        }
        unitsToRemove.Clear();

        //if mouse button (left hand side) pressed instantiate a raycast
        //if (Input.GetMouseButtonDown(0))
        //{
        //    if (Input.GetMouseButtonDown(0))
        //    {
        //        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //        Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);

        //        RaycastHit2D hit = Physics2D.Raycast(mousePos2D, Vector2.zero);
        //        //Debug.Log(string.Format("hit: {0}", hit.collider));
        //        //Debug.Log(string.Format("hit: {0}", hit.collider.gameObject.name));
                
        //    }
        //}
    }
    public void RemoveUnit(UnitBehaviour unit)
    {
        if (unit.TheTeam== Teams.Hero)
        {
            HeroList.Remove(unit);
        }
        else
        {
            EnemyList.Remove(unit);
        }
        Destroy(unit.gameObject);
    }
    public override void OnDragStarted(DragDropUI drag)
    {
        if(MarkerUI!= null)
        {
            MarkerUI.SetActive(true);
            MarkerGround.SetActive(true);
            MarkerUI.transform.position = drag.transform.position;
        }
        _dragStarter = drag;
        _dragStarted = true;
    }

    public override void OnDraging(DragDropUI drag, PointerEventData eventData)
    {
        base.OnDraging(drag, eventData);
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);

        MarkerGround.transform.position = mousePos2D;
    }
    
    public override void OnDrop(DragDropUI drag)
    {
        if (!_dragStarted)
        {
            return;
        }
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 mousePos2D = new Vector3(mousePos.x, mousePos.y, -0.1f);
        int i = _dragStarter.SlotIndex;
        UnitInfo info = SaveData.Instance.Data.GetDeckItem(i);
        int index = info.Index;//ObscuredPrefs.GetInt(string.Format(Consts.Key_Multiplay_Equip_Index_Foramt, i), -1);
        int level = info.Level;// ObscuredPrefs.GetInt(string.Format(Consts.Key_UnitLevelFormat, group, index), 0);
        int price = DataManager.Instance.GetMPCost(index);
        if (price <= CardManager.MP)
        {
            mousePos2D = new Vector3((int)mousePos2D.x, (int)mousePos2D.y, -0.2f);
            Summon(mousePos2D, index, level, Teams.Hero, FrameCounter + 60);
            if (IsMultiPlay)
            {
                MultiplayManager.Instance.SendSummon(string.Format("{0}/{1}/{2}/{3}", index + level*1000, FrameCounter + 60, (int)mousePos2D.x, (int)mousePos2D.y));
            }
            else
            {

            }
            
            CardManager.MP -= price;
            CardManager.UpdateAvailableCards();

            GroundEffect.transform.position = mousePos2D;
            GroundEffect.SetActive(true);
            GroundEffect.GetComponent<SpriteExplosion>().Play();
        }
        
        //Debug.Log(string.Format("Drop success: {0}/{1}/{2} ", group+1, index, level));
    }
    
    public void Summon(Vector3 pos, int index, int level, Teams team, int arriveFrame)
    {
        GameObject obj;
        if (team == Teams.Hero)
        {
            obj = Instantiate(HeroTemp);
        }
        else
        {
            obj = Instantiate(EnemyTemp);
            pos = new Vector3(-pos.x, pos.y, pos.z);
        }
        
        obj.transform.position = pos;
        UnitBehaviour unit = obj.GetComponent<UnitBehaviour>();
        unit.Init(index, level, team);
        unit.FrameCountLeftToArrive = arriveFrame - FrameCounter;
        unit.TheBattleScript = this;
        OnUnitSummon += unit.OnNewEnemyAppeared;
    }
    public void FireNewEnemy(UnitBehaviour unit)
    {
        if (OnUnitSummon != null)
        {
            OnUnitSummon(unit);
        }
    }

    public override void OnDragEnded()
    {
        if (MarkerUI != null)
        {
            MarkerUI.SetActive(false);
            MarkerGround.SetActive(false);
        }
        _dragStarted = false;
        Debug.Log("Drop ended: ");
    }

    public void GameOver()
    {
        GameOverScript.gameObject.SetActive(true);
        EndGame();
    }
    public void GameClear()
    {
        int stage = GameManager.Instance.CurrentStage;
        stage++;
        if (stage >= 0 && SaveData.Instance.Data.CurrentStage < stage)
        {
            SaveData.Instance.Data.CurrentStage = stage;
            SaveData.Instance.Save();
        }
        GameClearScript.gameObject.SetActive(true);
        EndGame();
    }
    public void TimeOver()
    {
        LblTimer.text = "TIME SET";
        PnlDraw.SetActive(true);
        GameOverScript.LblTitle.text = LanguageManager.Instance.GetText("draw");

        EndGame();
    }
    public void EndGame()
    {
        MultiplayManager.Instance.Disconnect();
        IsGameOver = true;
    }
    public void OnPauseClick()
    {
        UnityEngine.Time.timeScale = 0;
        Transform pnl = TheCanvas.transform.Find("Popups").Find("pnlPause");
        ShowGameObject(pnl.gameObject);
        TextMeshProUGUI lbl = pnl.Find("lblStage").GetComponent<TextMeshProUGUI>();
        int stage = GameManager.Instance.CurrentStage;
        if (stage >= 0)
        {
            lbl.text = string.Format("Stage {0}-{1}", stage / 12, stage % 12 + 0);
        }
    }
    public void OnHomeClick()
    {
        if (IsMultiPlay)
        {
            MultiplayManager.Instance.Disconnect();
        }
        MultiplayManager.Instance.OnEnemySummon -= OnEnemySummon;
        SceneManager.LoadScene("Lobby");
    }
    public void OnResume()
    {
        UnityEngine.Time.timeScale = 1;
        HideGameObject(TheCanvas.transform.Find("Popups").Find("pnlPause").gameObject);
    }
    public void ShowGameObject(GameObject obj)
    {
        obj.SetActive(true);
    }
    public void HideGameObject(GameObject obj)
    {
        obj.SetActive(false);
    }
}
