using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Student
{
    public int Grade;
    public int ClassNumber;
    public int StudentNumber;
    public string Name;

}

public class Unit
{
    public int Hp = 10;
    public int Att = 5;
    public int Def = 1;
    public string Name;

    public Unit()
    {

    }
    public Unit(string name, int hp, int att, int def)
    {
        Name = name;
        Hp = hp;
        Att = att;
        Def = def;
    }

    public float GetHP()
    {
        int buff = 10;
        return Hp * ( 1 + buff*0.01f);
    }

    public void Attack(Unit unit)
    {
        unit.Hp -= this.Att - unit.Def;
    }
}


public class Home : MonoBehaviour
{
    public GameObject PnlInventory;
    public GameObject PnlStageLobby;
    public List<GameObject> EarthIconPrefabs;
    public List<GameObject> WaterIconPrefabs;
    public List<GameObject> FireIconPrefabs;
    public List<GameObject> DarkIconPrefabs;
    public List<GameObject> EventEarthIconPrefabs;
    public List<GameObject> EventWaterIconPrefabs;
    public List<GameObject> EventFireIconPrefabs;
    public List<GameObject> EventDarkIconPrefabs;
    public List<GameObject> CardPrefabs;
    public GameObject Background;
    public Transform TheCanvas;
    public Transform Popups;
    public PlayerData Data;
    public AudioClip SoundGem;
    public AudioClip SoundGold;
    public AudioClip SoundGoldFly;
    public TextMeshProUGUI LblGold;
    public TextMeshProUGUI LblGem;

    List<Student> studentList = new List<Student>();

    //public GameObject NormalCardPrefab;
    private int _totalStageCatetoryCount = 6;
    // Start is called before the first frame update
    void Start()
    {

        GameManager.Instance.CurrentStage = -1;
        GameManager.Instance.TheHome = this;
        Data = SaveData.Instance.Data;

        //SaveData.Instance.Data = new PlayerData();
        //SaveData.Instance.Save(); // test 

        Background.transform.DOLocalMoveX(-80, 350);

        //PnlInventory.GetComponent<Inventory>().OnItemSelected.AddListener(OnItemSelected);


        // init done

        //int[] array = { 3, 5, 2, 6, 8, 9, 0, 1, 4, 7 };

        //// 배열 안의 원하는 숫자(8)의 순번(index)을 반환(return)하는 함수를 만드시오!
        //int index = GetIndexOf(array, 8);

        ////List<int> list = new List<int>(array);
        ////list.Sort();
        //int idx = this.GetIndexOf(array, 9);
        //Debug.Log(string.Format("Index of 9 is {0}", idx));

        //List<Unit> unitList = new List<Unit>();
        //Unit swordsman = new Unit("swordsman", 50, 30, 10);
        //unitList.Add(swordsman);
        //Unit magician = new Unit("magician", 10, 50, 1);
        //unitList.Add(magician);
        //Unit healer = new Unit("healer", 30, 3, 5);
        //unitList.Add(healer);
        //swordsman.Attack(magician);

        //// 아래에 사용된 4가지 함수(Sum, Deduct, Multiply, Devide)를 만드시오. 간단간단 4칙연산임
        //// 나머지 에러는 알아서 해결하시오ㅎㅎ
        //int num0 = 20;
        //int num1 = 3;
        //int result = Sum(num0, num1);
        //Debug.Log("result: {0}");
        //int result = Deduct(num0, num1);
        //Debug.Log("result: {0}");
        //int result = Multiply(num0, num1);
        //Debug.Log("result: {0}");
        //int result = Devide(num0, num1);
        //Debug.Log("result: {0}");

        //Debug.Log(string.Format("Magician's HP is {0}", magician.Hp));


        //int num = 2;
        //for (int i = 0; i < 10; i++)
        //{
        //    str = PrintSum(num, i);
        //    Debug.Log(str);
        //    // 이곳의 코드는 무엇일까요?
        //}
        // 결과는 동일하다.
        // 2 + 0 = 2
        // 2 + 2 = 4
        // 2 + 4 = 6
        // ...
        // ...

        //int num = 4;
        //int result = SumAndMultiply(num);
        //Debug.Log(string.Format("{0}+{0}*{0}={1}", num, result));
        //string str = "23984723943274892374923798";
        //int intNum = 14082349321408234932;
        //long longNum = 3294729384732984729;
        // int 4byte int
        // int 16byte int16
        // int 32byte int32
        // long 64byte
        // 1byte = 8bit
        // 1 bit = 2
        // 2 bit = 4
        // 3 bit = 8
        // 4 bit = 16
        // 32bit

        // 힌트1. 나누기를 사용하세요
        // 힌트2.
        int result = 2 / 3;
        // result is 0
        // 힌트3.
        result = 9 / 10;
        // i/10
        List<string> nameList = new List<string>();
        for (int i = 0; i < 100; i++)
        {
            //nameList.Add("배철수 " + (i + 1).ToString() + " 은 1학년 " + 1 +"반");
            nameList.Add(string.Format("배철수 {0}은 1학년 {1}반", i+1, i/10 + 1));

        }
        // 배철수 1 은 1학년 1반
        // 배철수 2 은 1학년 1반
        // ..

        // 배철수 10 은 1학년 1반
        // 배철수 11 은 1학년 2반








        int rest = 10 % 4;
        // 10/4의 나머지는 2
        // 힌트1.
        //rest = 

        
        for (int i = 0; i < 100; i++)
        {
            // 객체(Object)
            Student student = new Student();
            student.Name = "배철수" + (i + 1);
            student.Grade = 0;
            student.ClassNumber = i / 10 + 1;
            // 반마다 1~10번까지 번호를 부여하시오.
            student.StudentNumber = i % 10 + 1;

            studentList.Add(student);

            //Debug.Log(string.Format("{0}은 {1}학년 {2}반",
            //                                                student.Name,
            //                                                student.Grade + 1,
            //                                                student.ClassNumber));
        }
        Student student1 = new Student();
        student1.Name = "배철수";

        Student student2 = new Student();
        student2.Name = "배철수";

        Student student3 = student1;
        if (student1 == student3)
        {
            // true?
        }



        string name = "배철수76";
        //if (name.Equals(name1))
        //{
        //    // enter
        //}
        int classNumber = -1;
        
        for (int i = 0; i < studentList.Count; i++)
        {
            // 이름이 같은지 비교하시오.
            if (name.Equals(studentList[i].Name))
            {
                classNumber = studentList[i].ClassNumber;
            }
        }


        // 감방문제
        // GetClassNumber 함수를 만드시오.
        //int classNumber = GetClassNumber(name);
        //Debug.Log(string.Format("{0}는 {1}반입니다.", name, classNumber));





        //// 1. 나머지
        //int result = 20 % 4;

        //// 2. 문자열 비교
        //string name0 = "234";
        //string name1 = "sdflskjfdslk";
        //if (name.Equals(name1))
        //{
        //    Debug.Log("같아요~");
        //}
        //else
        //{
        //    Debug.Log("No, it is not the same.");
        //}


        //int index = 50;
        //Debug.Log(string.Format("list[{0}]", studentList[index]));
        // list[50]배철수 51

    }


    public int SumAndMultiply(int num0)
    {

        int result = num0 + num0 * num0;
        return result;
    }
    public int Sum(int num0, int num1)
    {
        return num0 + num1;
    }
    public int Multiply(int num0, int num1)
    {
        return num0 * num1;
    }

    public int GetIndexOf(int[] array, int num)
    {
        for (int i = 0; i < array.Length; i++)
        {
            if (array[i] == num)
            {
                return i;
            }
        }
        return -1;
    }




    // Update is called once per frame
    void Update()
    {
        
    }
    public void HideGameObject(GameObject obj)
    {
        obj.SetActive(false);
    }
    public void OnBackButtonClick()
    {
        bool closed = false;
        Transform popups = TheCanvas.transform.Find("Popups");
        for (int i = popups.childCount - 2; i >= 0; i--)
        {
            GameObject obj = popups.GetChild(i).gameObject;
            if (obj.activeSelf)
            {
                if (obj.name.Contains("Effect"))
                {
                    return;
                }
                HideGameObject(obj);
                closed = true;
                break;
            }
        }

    }

    public void UpdateStageCatergory()
    {

    }
    public void OnDungeonClick() {
        Popups.Find("pnlStageCategory").gameObject.SetActive(true);

        Transform pnl = Popups.Find("pnlStageCategory");
        Transform content = pnl.Find("Scroll View").Find("Content");
        int currentStage = Data.CurrentStage;
        for (int i = 0; i < _totalStageCatetoryCount; i++)
        {
            Transform stage = content.Find(string.Format("stage{0}", i));
            if (stage == null)
            {
                stage = Instantiate(content.Find("stage0").gameObject, content).transform;
                stage.name = string.Format("stage{0}", i);
            }

            for (int j = 0; j < 8; j++)
            {
                Transform img = stage.Find(string.Format("Sample ({0})", j));
                string path = GameManager.Instance.GetSpritePath(i * 8 + j/2 + 4*(j%2));
                img.GetComponent<Image>().sprite = Resources.Load<Sprite>(path);
            }
            stage.gameObject.SetActive(i <= currentStage / 12);
            //stage.gameObject.SetActive(true);
            if (i <= currentStage / 12)
            {
                //stage.gameObject.SetActive(true);
            }
            else
            {
                Instantiate(content.Find("lock").gameObject, content).gameObject.SetActive(true);
            }
        }
        //content.Find("lock").gameObject.SetActive(currentStage / 10 < _totalStageCatetoryCount - 1);
        Transform comingSoon = content.Find("comingSoon");
        comingSoon.parent = content.parent;
        comingSoon.parent = content;
    }
    private int _currentStagePage;
    public void OnStageClick(int index)
    {
        Popups.Find("pnlStage").gameObject.SetActive(true);
        UpdateStage(index);
    }
    public void OnHomeClick()
    {
        Transform popups = TheCanvas.transform.Find("Popups");
        for (int i = popups.childCount - 2; i >= 0; i--)
        {
            GameObject obj = popups.GetChild(i).gameObject;
            if (obj.activeSelf)
            {
                if (obj.name.Contains("Effect"))
                {
                    return;
                }
                HideGameObject(obj);
            }
        }
    }
    private StageItem _selectedStageItem;
    private List<GameObject> _cardList = new List<GameObject>();
    public void OnStageItemClick(GameObject obj)
    {
        StageItem item = obj.GetComponent<StageItem>();
        _selectedStageItem = item;
        //Debug.Log(string.Format("list count: {0}", item.UnitInfoList.Count));
        GameManager.Instance.EnemyUnitList.Clear();
        for (int i = 0; i < item.UnitInfoList.Count; i++)
        {
            GameManager.Instance.EnemyUnitList.Add(new UnitInfo(item.UnitInfoList[i]));
            //Debug.Log(string.Format("index : {0}", item.UnitInfoList[i].Index));
        }
        GameManager.Instance.CurrentStage = item.StageIndex;
        // enemy
        GameObject card;
        Transform pnl = TheCanvas.Find("Popups").Find("pnlStageLobby");
        TextMeshProUGUI lbl = pnl.Find("Horizontal").Find("slotEnemy").Find("CharacterInfo").Find("lblName").GetComponent<TextMeshProUGUI>();
        lbl.text = string.Format("STAGE {0}", _selectedStageItem.StageIndex + 1);
        lbl = pnl.Find("Horizontal").Find("slotEnemy").Find("CharacterInfo").Find("lblName").GetComponent<TextMeshProUGUI>();
        lbl.text = LanguageManager.Instance.GetText(string.Format("stage tip {0}", Random.Range(0, 10)));

        Transform enemyContent = pnl.Find("Horizontal").Find("slotEnemy").Find("Monsters");
        foreach (Transform tf in enemyContent)
        {
            Destroy(tf.gameObject);
        }
        GameManager.Instance.EnemyUnitList.Clear();
        for (int i = 0; i < item.UnitInfoList.Count; i++)
        {
            UnitInfo info = item.UnitInfoList[i];
            card = GameManager.Instance.GetCard(info.Index, enemyContent);
            card.name = string.Format("slot{0}", i);
            card.transform.localScale = new Vector3(1, 1, 1);
            card.gameObject.SetActive(true);

            GameManager.Instance.EnemyUnitList.Add(info); // this is for stage ai
        }

        // set hero
        //lbl = pnl.Find("Horizontal").Find("slotHero").Find("CharacterInfo").Find("lblName").GetComponent<TextMeshProUGUI>();
        //lbl.text = Data.Name;
        //lbl = pnl.Find("Horizontal").Find("slotHero").Find("CharacterInfo").Find("lblName").GetComponent<TextMeshProUGUI>();
        //lbl.text = LanguageManager.Instance.GetText(string.Format("stage tip {0}", Random.Range(0, 10)));

        //Transform heroContent = pnl.Find("Horizontal").Find("slotHero").Find("Monsters");
        //foreach (Transform tf in heroContent)
        //{
        //    Destroy(tf.gameObject);
        //}

        //for (int i = 0; i < Data.GetDeckCount; i++)
        //{
        //    UnitInfo info = Data.GetDeckItem(i);

        //    card = GameManager.Instance.GetCard(info.Index, heroContent);
        //    card.name = string.Format("slot{0}", i);
        //    card.transform.localScale = new Vector3(1, 1, 1);
        //}
        //int index, count;
        //bool isEmpty;
        //_cardList.Clear();
        //for (int i = 0; i < SaveData.Instance.Data.DeckCount; i++)
        //{
        //    //group = ObscuredPrefs.GetInt(string.Format(Consts.Key_Multiplay_Equip_Group_Foramt, i), -1);
        //    //index = ObscuredPrefs.GetInt(string.Format(Consts.Key_Multiplay_Equip_Index_Foramt, i), -1);
        //    //Debug.Log(string.Format("{2}. group: {0}/index:{1}", group, index, i));
        //    UnitInfo info = SaveData.Instance.Data.GetDeckItem(i);
        //    index = info.Index;
        //    isEmpty = false;
        //    if (info == null)
        //    {
        //        isEmpty = true;
        //        //Debug.Log("isempth 1");
        //    }
        //    else
        //    {
        //        //count = ObscuredPrefs.GetInt(string.Format(Consts.Key_Monster_Count_Format, group, index), 0);
        //        count = info.Count;
        //        //Debug.Log("isempth 2");
        //        if (count <= 0)
        //        {
        //            //Debug.Log("isempth 3");
        //            isEmpty = true;
        //        }
        //    }
        //    //Debug.Log("isempth " + isEmpty);
        //    if (isEmpty)
        //    {
        //        card = Instantiate(CardPrefabs[0], heroContent);
        //    }
        //    else
        //    {
        //        //card = Instantiate(HomeScript.CardPrefabs[1 + group % 4]);
        //        //GameObject temp = Instantiate(HomeScript.GetMonsterIconPrefab(group, index));
        //        //temp.transform.SetParent(card.transform.Find("GradeFrame"));
        //        //temp.transform.localScale = new Vector3(1, 1, 1);
        //        //temp.transform.localPosition = new Vector3(-4, 8, 0);
        //        card = GameManager.Instance.GetCard(index, heroContent);
        //        card.gameObject.SetActive(true);
        //    }
        //    _cardList.Add(card);
        //    card.name = string.Format("slot{0}", i);
        //    if (i == 0)
        //    {
        //        card.GetComponent<Button>().onClick.AddListener(() => OnSlotClick(_cardList[0]));
        //    }
        //    else if (i == 1)
        //    {
        //        card.GetComponent<Button>().onClick.AddListener(() => OnSlotClick(_cardList[1]));
        //    }
        //    else if (i == 2)
        //    {
        //        card.GetComponent<Button>().onClick.AddListener(() => OnSlotClick(_cardList[2]));
        //    }
        //    else if (i == 3)
        //    {
        //        card.GetComponent<Button>().onClick.AddListener(() => OnSlotClick(_cardList[3]));
        //    }
        //    else if (i == 4)
        //    {
        //        card.GetComponent<Button>().onClick.AddListener(() => OnSlotClick(_cardList[4]));
        //    }
        //    else if (i == 5)
        //    {
        //        card.GetComponent<Button>().onClick.AddListener(() => OnSlotClick(_cardList[5]));
        //    }
        //    else if (i == 6)
        //    {
        //        card.GetComponent<Button>().onClick.AddListener(() => OnSlotClick(_cardList[6]));
        //    }
        //    else if (i == 7)
        //    {
        //        card.GetComponent<Button>().onClick.AddListener(() => OnSlotClick(_cardList[7]));
        //    }

        //    //card.transform.SetParent(HeroCardContainer.transform);
        //    card.transform.localScale = new Vector3(1, 1, 1);
        //}

        pnl.gameObject.SetActive(true);
    }
    private int _selectedSlot;
    //public void OnSlotClick(GameObject obj)
    //{
    //    int index = int.Parse(obj.name.Substring(obj.name.Length - 1, 1));
    //    Debug.Log(string.Format("clicked slot: {0}", index));
    //    _selectedSlot = index;
    //    ShowInventoryForPick();
    //}
    //void OnItemSelected()
    //{
    //    if (!gameObject.activeSelf || PnlStageLobby.activeSelf)
    //    {
    //        return;
    //    }
    //    UnitInfo info = PnlInventory.GetComponent<Inventory>().SelectedInfo;
    //    Debug.Log(string.Format("selected {0}", info.Index));
    //    //ObscuredPrefs.SetInt(string.Format(Consts.Key_Multiplay_Equip_Group_Foramt, _selectedSlot), group);
    //    //ObscuredPrefs.SetInt(string.Format(Consts.Key_Multiplay_Equip_Index_Foramt, _selectedSlot), index);
    //    SaveData.Instance.Data.SetDeckItem(_selectedSlot, info);

    //    OnStageItemClick(_selectedStageItem.gameObject);
    //}
    //public void ShowInventoryForPick()
    //{
    //    PnlInventory.GetComponent<Inventory>().ShowInventory(true);
    //}
    public void OnStartStageClick()
    {
        SceneManager.LoadScene("Battle");
    }
    public void UpdateStage(int index)
    {
        _currentStagePage = index;
        Transform pnl = Popups.Find("pnlStage");
        Transform content = pnl.Find("Scroll View").Find("Viewport").Find("Content");
        int starCount;

        for (int i = 0; i < 12; i++)
        {
            Transform stage = content.Find(i.ToString());
            if (stage == null)
            {
                stage = Instantiate(content.Find("temp").gameObject, content).transform;
            }
            StageItem stageItem = stage.GetComponent<StageItem>();
            if (stageItem == null)
            {
                stageItem = stage.gameObject.AddComponent<StageItem>();
            }
            stageItem.StageIndex = i;
            stage.gameObject.SetActive(true);
            stage.name = i.ToString();
            bool isCurrent = Data.CurrentStage == _currentStagePage * 10 + i;
            stage.Find("stageCurrent").gameObject.SetActive(isCurrent);
            stage.Find("imgContent").Find("current").gameObject.SetActive(isCurrent);
            stage.Find("stageCurrent").Find("Text").GetComponent<TextMeshProUGUI>().text = (i + 1).ToString();
            stage.Find("stageLock").gameObject.SetActive(Data.CurrentStage < _currentStagePage * 10 + i);
            starCount = Data.GetStarCount(_currentStagePage * 10 + i);
            for (int j = 0; j < 3; j++)
            {
                stage.Find(string.Format("imgStar{0}", j)).GetComponent<Image>().sprite = Resources.Load<Sprite>(string.Format("uiSprites/{0}", starCount > j ? "stage_star" : "stage_star_dim"));
            }
            UnitInfo info = new UnitInfo();
            int maxIndex = _currentStagePage * 8 + i;
            if (i >= 8)
            {
                maxIndex = _currentStagePage * 8 + 7;
            }
            if (maxIndex > 47)
            {
                maxIndex = 47;
            }
            
            List<UnitInfo> list = stageItem.UnitInfoList;
            list.Clear();
            info.Index = maxIndex;
            list.Add(info);
            //Debug.Log("stage item index " + stageItem.UnitInfoList[i].Index);
            int decrease = maxIndex;
            int loopCounter = 0;

            while(list.Count < 8)
            {
                if (_currentStagePage == 0 && i < 8)
                {
                    decrease--;
                    if (decrease < 0)
                    {
                        decrease = 0;
                    }
                    list.Add(new UnitInfo(decrease));
                }
                else
                {
                    int idx = Random.Range(0, maxIndex);
                    bool included = false;
                    foreach (UnitInfo inf in list)
                    {
                        if (inf.Index == idx)
                        {
                            included = true;
                            break;
                        }
                    }
                    if (!included)
                    {
                        list.Add(new UnitInfo(idx));
                    }
                }
                loopCounter++;
                if (loopCounter > 10000)
                {
                    break;
                }
            }

            for (int j = 0; j < list.Count - 1; j++)
            {
                for (int k = 0; k < list.Count - j - 1; k++)
                {
                    if (list[k].Index < list[k+1].Index)
                    {
                        UnitInfo temp = list[k];
                        list[k] = list[k + 1];
                        list[k + 1] = temp;
                    }
                }
            }


            float x = stage.Find("imgContent").Find("img0").localPosition.x;
            float gapX = 140;
            for (int j = 0; j < list.Count; j++)
            {
                Transform img = stage.Find("imgContent").Find(string.Format("img{0}", j));
                if (img == null)
                {
                    img = Instantiate(stage.Find("imgContent").Find("img0").gameObject, stage.Find("imgContent")).transform;
                    img.name = string.Format("img{0}", j);
                }
                img.localPosition = new Vector3(x + gapX*j, img.localPosition.y, img.localPosition.z);
                img.GetComponent<Image>().sprite = Resources.Load<Sprite>(GameManager.Instance.GetSpritePath(list[j].Index));
                list[j].Level = i < 8 ? 0 : i - 7;
                img.Find("lblLevel").GetComponent<Text>().text = (list[j].Level + 1).ToString();
                img.gameObject.SetActive(!stage.Find("stageLock").gameObject.activeSelf);
                //Debug.Log("list j " + list[j].Index);
            }
            stage.Find("imgContent").Find("Lock").gameObject.SetActive(stage.Find("stageLock").gameObject.activeSelf);
            if (_currentStagePage == 0 && i < 8)
            {
                for (int j = 0; j < list.Count; j++)
                {
                    Transform img = stage.Find("imgContent").Find(string.Format("img{0}", j));
                    if (j > i)
                    {
                        img.gameObject.SetActive(false);
                    }
                }
            }
            
        }
    }
    private void GetUnitListForStage(int stage)
    {
        UnityEngine.Random.InitState(77);
        int category = stage / 10;
        int unlockedIndex = (category + 1) * 2;


    }
    public void OnPrevStageCategoryClick()
    {
        if (_currentStagePage <= 0)
        {
            return;
        }
        _currentStagePage--;
        UpdateStage(_currentStagePage);
    }
    public void OnNextStageCategoryClick()
    {
        if (_currentStagePage >= 5)
        {
            return;
        }
        _currentStagePage++;
        UpdateStage(_currentStagePage);
    }
    public void OnStageCategoryClick(GameObject obj)
    {
        Debug.Log("stage category" + obj.name.Substring(5));
        OnStageClick(int.Parse(obj.name.Substring(5)));
    }
    public void OnInventoryClick()
    {
        PnlInventory.GetComponent<Inventory>().ShowInventory(InventoryUseCases.Browse);
        Debug.Log("count: " + Data.InventoryCount);
    }
    
    public GameObject GetMonsterIconPrefab(int group, int index)
    {
        if (group == 0)
        {
            return EarthIconPrefabs[index];
        }
        else if (group == 1)
        {
            return WaterIconPrefabs[index];
        }
        else if (group == 2)
        {
            return FireIconPrefabs[index];
        }
        else if (group == 3)
        {
            return DarkIconPrefabs[index];
        }
        else if (group == 4)
        {
            return EventEarthIconPrefabs[index];
        }
        else if (group == 5)
        {
            return EventWaterIconPrefabs[index];
        }
        else if (group == 6)
        {
            return EventFireIconPrefabs[index];
        }
        else if (group == 7)
        {
            return EventDarkIconPrefabs[index];
        }
        return null;
    }
    public void ShowInstanceMessage(string key)
    {
        Popups.Find("InstanceMessage").GetComponent<InstantMessageUI>().ShowMessage(LanguageManager.Instance.GetText(key));
    }
    public void AddGold(int amount)
    {
        if (amount == 0)
        {
            return;
        }
        Debug.Log("add godl");
        GameObject lastObj;
        int coinCount = 20;
        if (amount < 10000)
        {
            coinCount = 10;
        }
        PlaySound(SoundGold);
        for (int i = 0; i < coinCount; i++)
        {
            GameObject obj = new GameObject();
            obj.transform.parent = TheCanvas.transform;
            Image img = obj.AddComponent<Image>();
            img.sprite = Resources.Load<Sprite>("uiSprites/gold");
            img.SetNativeSize();
            float scale = 1.0f;
            img.transform.localScale = new Vector3(scale, scale, scale);

            float width = 0;// TheCanvas.GetComponent<RectTransform>().sizeDelta.x * TheCanvas.GetComponent<Canvas>().scaleFactor;
            float height = 0;// TheCanvas.GetComponent<RectTransform>().sizeDelta.y * TheCanvas.GetComponent<Canvas>().scaleFactor;
            float range = 2;
            obj.transform.position = new Vector3(width / 2 + -range / 2 + Random.Range(0, range), height / 2 - range / 2 + Random.Range(0, range), 0);
            Sequence seq = DOTween.Sequence();
            float dur = 0.4f + Random.Range(0f, 0.4f);
            seq.Append(obj.transform.DOMoveY(obj.transform.position.y - range/2 - Random.Range(0, range/2), dur).OnComplete(() =>
            {
                PlaySound(SoundGoldFly);
            }));
            seq.Append(obj.transform.DOMove(LblGold.transform.position, 0.5f).SetEase(Ease.OutSine).OnComplete(() => {
                LblGold.GetComponent<UINumberRaiser>().Number = DataManager.Instance.Gold;
            }));
            DestroyLater dl = obj.AddComponent<DestroyLater>();
            dl.SetTime(dur + 0.5f);

            if (i == coinCount - 1)
            {
                lastObj = obj;
            }
        }
        DataManager.Instance.Gold += amount;
        SaveData.Instance.Save();
    }
    public void AddGem(int amount)
    {
        if (amount == 0)
        {
            return;
        }
        Debug.Log("add gem");
        GameObject lastObj;
        int coinCount = 20;
        if (amount < 10000)
        {
            coinCount = 10;
        }
        PlaySound(SoundGem);
        for (int i = 0; i < coinCount; i++)
        {
            GameObject obj = new GameObject();
            obj.transform.parent = TheCanvas.transform;
            Image img = obj.AddComponent<Image>();
            img.sprite = Resources.Load<Sprite>("uiSprites/gem");
            img.SetNativeSize();
            float scale = 1.0f;
            img.transform.localScale = new Vector3(scale, scale, scale);

            float width = 0;// TheCanvas.GetComponent<RectTransform>().sizeDelta.x * TheCanvas.GetComponent<Canvas>().scaleFactor;
            float height = 0;// TheCanvas.GetComponent<RectTransform>().sizeDelta.y * TheCanvas.GetComponent<Canvas>().scaleFactor;
            float range = 2;
            obj.transform.position = new Vector3(width / 2 + -range / 2 + Random.Range(0, range), height / 2 - range / 2 + Random.Range(0, range), 0);
            Sequence seq = DOTween.Sequence();
            float dur = 0.4f + Random.Range(0f, 0.4f);
            seq.Append(obj.transform.DOMoveY(obj.transform.position.y - range/2 - Random.Range(0, range/2), dur).OnComplete(() =>
            {
                PlaySound(SoundGoldFly);
            }));
            seq.Append(obj.transform.DOMove(LblGem.transform.position, 0.5f).SetEase(Ease.OutSine).OnComplete(() => {
                LblGem.GetComponent<UINumberRaiser>().Number = DataManager.Instance.Gem;
            }));
            DestroyLater dl = obj.AddComponent<DestroyLater>();
            dl.SetTime(dur + 0.5f);

            if (i == coinCount - 1)
            {
                lastObj = obj;
            }
        }
        DataManager.Instance.Gem += amount;
        SaveData.Instance.Save();
    }
    public void PlaySound(AudioClip clip)
    {

    }
    private void OnApplicationQuit()
    {
        SaveData.Instance.Save();
    }
    private void OnApplicationPause(bool pause)
    {
        SaveData.Instance.Save();
    }
}
