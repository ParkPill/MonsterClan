using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StrEventArg : EventArgs
{
    public string StringValue = string.Empty;
    public StrEventArg() { }
    public StrEventArg(string value)
    {
        StringValue = value;
    }
}
public class GameObjectEventArg : EventArgs
{
    public GameObject TheGameObject;
    public GameObjectEventArg() { }
    public GameObjectEventArg(GameObject value)
    {
        TheGameObject = value;
    }
}
public enum VideoType
{
    HeartRefill = 0,
    PremiumVideo
}
public class GameManager : MonoBehaviour
{
    public GameScript TheGameScript;
    public static System.Random Random = new System.Random();
    public static System.Random DailyRandom = new System.Random();
    public GameObject LoadingPrefab;
    public GameObject CurrentPopup;
    public float Power =10;
    public int CurrentStage = -1;
    public AudioSource BGMAudioSource;
    //public GameData GameData;
    //private string _gameDataFileName = "/msfsyis.sis";
    //public event EventHandler CharacterChanged;
    public event OnEventDelegate<VideoType> VideoComplete;
    public bool IsVsAI = true;
    private VideoType _playedVideoType;
    public int AILevel = 0;
    public bool IsMultiPlay = false;
    public Home TheHome;
    public List<UnitInfo> EnemyUnitList = new List<UnitInfo>();
    //private BannerView bannerView;
    //private InterstitialAd interstitial;
    private static GameManager _instance;
    public static GameManager Instance
    {
        get
        {
            if (!_instance)
            {
                _instance = GameObject.FindObjectOfType(typeof(GameManager)) as GameManager;
                if (!_instance)
                {
                    GameObject container = new GameObject();
                    container.name = "GameManager Instance" + Random.Next(1000);
                    _instance = container.AddComponent(typeof(GameManager)) as GameManager;
                    _instance.Init();
                    DontDestroyOnLoad(_instance);
                }
            }
            return _instance;
        }
    }
    //public List<GameObject> CardPrefabs = new List<GameObject>();
    public GameObject CardPrefab;
    public List<GameObject> EarthIconPrefabs = new List<GameObject>();
    public List<GameObject> WaterIconPrefabs = new List<GameObject>();
    public List<GameObject> FireIconPrefabs = new List<GameObject>();
    public List<GameObject> DarkIconPrefabs = new List<GameObject>();
    public List<GameObject> EventEarthIconPrefabs = new List<GameObject>();
    public List<GameObject> EventWaterIconPrefabs = new List<GameObject>();
    public List<GameObject> EventFireIconPrefabs = new List<GameObject>();
    public List<GameObject> EventDarkIconPrefabs = new List<GameObject>();

    
    public void Init()
    {
        CardPrefab = Resources.Load("MonsterPrefabs/Cards/tempCommon") as GameObject;
        //CardPrefabs.Add(Resources.Load("MonsterPrefabs/Cards/tempCommon") as GameObject);
        //CardPrefabs.Add(Resources.Load("MonsterPrefabs/Cards/tempEarth") as GameObject);
        //CardPrefabs.Add(Resources.Load("MonsterPrefabs/Cards/tempWater") as GameObject);
        //CardPrefabs.Add(Resources.Load("MonsterPrefabs/Cards/tempFire") as GameObject);
        //CardPrefabs.Add(Resources.Load("MonsterPrefabs/Cards/tempDark") as GameObject);
        for (int i = 0; i < 12; i++)
        {
            EarthIconPrefabs.Add(Resources.Load(string.Format("MonsterPrefabs/Icons/forest_4{0:00}", i + 1)) as GameObject);
            WaterIconPrefabs.Add(Resources.Load(string.Format("MonsterPrefabs/Icons/sea_3{0:00}", i + 1)) as GameObject);
            FireIconPrefabs.Add(Resources.Load(string.Format("MonsterPrefabs/Icons/cave_6{0:00}", i + 1)) as GameObject);
            DarkIconPrefabs.Add(Resources.Load(string.Format("MonsterPrefabs/Icons/undead_1{0:00}", i + 1)) as GameObject);
            EventEarthIconPrefabs.Add(Resources.Load(string.Format("MonsterPrefabs/Icons/jungle_2{0:00}", i + 1)) as GameObject);
            EventWaterIconPrefabs.Add(Resources.Load(string.Format("MonsterPrefabs/Icons/ice_8{0:00}", i + 1)) as GameObject);
            EventFireIconPrefabs.Add(Resources.Load(string.Format("MonsterPrefabs/Icons/field_5{0:00}", i + 1)) as GameObject);
            EventDarkIconPrefabs.Add(Resources.Load(string.Format("MonsterPrefabs/Icons/devil_7{0:00}", i + 1)) as GameObject);
        }
    }
    private void Start()
    {
        
    }
    public GameObject GetCard(int index, Transform parent)
    {
        GameObject card = Instantiate(GameManager.Instance.CardPrefab, parent);
        Attributes attri = DataManager.Instance.GetAttributes(index);
        if (attri == Attributes.Earth)
        {
            card.GetComponent<Image>().sprite = Resources.Load<Sprite>("reward_item_frame_green");
        }
        else if (attri == Attributes.Water)
        {
            card.GetComponent<Image>().sprite = Resources.Load<Sprite>("reward_item_frame_blue");
        }
        else if (attri == Attributes.Fire)
        {
            card.GetComponent<Image>().sprite = Resources.Load<Sprite>("reward_item_frame_red");
        }
        else if (attri == Attributes.Dark)
        {
            card.GetComponent<Image>().sprite = Resources.Load<Sprite>("reward_item_frame_purple");
        }
        float range = DataManager.Instance.GetRange(index, 0);
        string path = GameManager.Instance.GetSpritePath(index);
        card.transform.Find("imgIcon").GetComponent<Image>().sprite = Resources.Load<Sprite>(path);
        card.transform.Find("iconMelee").gameObject.SetActive(range < 2);
        card.transform.Find("iconRange").gameObject.SetActive(range >= 2);
        card.transform.Find("iconEarth").gameObject.SetActive(attri == Attributes.Earth);
        card.transform.Find("iconWater").gameObject.SetActive(attri == Attributes.Water);
        card.transform.Find("iconFire").gameObject.SetActive(attri == Attributes.Fire);
        card.transform.Find("iconDark").gameObject.SetActive(attri == Attributes.Dark);
        card.transform.Find("lblCount").GetComponent<TextMeshProUGUI>().text = DataManager.Instance.GetMPCost(index).ToString();
        card.transform.Find("lblNo").GetComponent<TextMeshProUGUI>().text = (index / 4 + 1).ToString();



        //GameObject temp = Instantiate(GameManager.Instance.CardPrefab);
        //temp.transform.SetParent(card.transform.Find("GradeFrame"));
        //temp.transform.localScale = new Vector3(1, 1, 1);
        //temp.transform.localPosition = new Vector3(-4, 8, 0);

        //string path = GameManager.Instance.GetSpritePath(index);
        //temp.GetComponent<Image>().sprite = Resources.Load<Sprite>(path);

        card.transform.localScale = new Vector3(1, 1, 1);

        return card;
    }
    public GameObject SetCard(GameObject card, int index)
    {
        Attributes attri = DataManager.Instance.GetAttributes(index);
        if (attri == Attributes.Earth)
        {
            card.GetComponent<Image>().sprite = Resources.Load<Sprite>("reward_item_frame_green");
        }
        else if (attri == Attributes.Water)
        {
            card.GetComponent<Image>().sprite = Resources.Load<Sprite>("reward_item_frame_blue");
        }
        else if (attri == Attributes.Fire)
        {
            card.GetComponent<Image>().sprite = Resources.Load<Sprite>("reward_item_frame_red");
        }
        else if (attri == Attributes.Dark)
        {
            card.GetComponent<Image>().sprite = Resources.Load<Sprite>("reward_item_frame_purple");
        }
        float range = DataManager.Instance.GetRange(index, 0);
        string path = GameManager.Instance.GetSpritePath(index);
        card.transform.Find("imgIcon").GetComponent<Image>().sprite = Resources.Load<Sprite>(path);
        card.transform.Find("imgIcon").gameObject.SetActive(index >= 0);
        card.transform.Find("iconMelee").gameObject.SetActive(index >= 0 && range < 2);
        card.transform.Find("iconRange").gameObject.SetActive(index >= 0 && range >= 2);
        card.transform.Find("iconEarth").gameObject.SetActive(index >= 0 && attri == Attributes.Earth);
        card.transform.Find("iconWater").gameObject.SetActive(index >= 0 && attri == Attributes.Water);
        card.transform.Find("iconFire").gameObject.SetActive(index >= 0 && attri == Attributes.Fire);
        card.transform.Find("iconDark").gameObject.SetActive(index >= 0 && attri == Attributes.Dark);
        card.transform.Find("lblCount").GetComponent<TextMeshProUGUI>().text = DataManager.Instance.GetMPCost(index).ToString();
        card.transform.Find("lblNo").GetComponent<TextMeshProUGUI>().text = (index / 4 + 1).ToString();

        card.transform.Find("imgPotion").gameObject.SetActive(index >= 0);
        card.transform.Find("lblCount").gameObject.SetActive(index >= 0);
        card.transform.Find("lblNo").gameObject.SetActive(index >= 0);

        //GameObject temp = Instantiate(GameManager.Instance.CardPrefab);
        //temp.transform.SetParent(card.transform.Find("GradeFrame"));
        //temp.transform.localScale = new Vector3(1, 1, 1);
        //temp.transform.localPosition = new Vector3(-4, 8, 0);

        //string path = GameManager.Instance.GetSpritePath(index);
        //temp.GetComponent<Image>().sprite = Resources.Load<Sprite>(path);

        card.transform.localScale = new Vector3(1, 1, 1);

        return card;
    }
    public string GetSpritePath(int index)
    {
        if (index < 48)
        {
            if (index%4 == 0) // earth
            {
                return string.Format("monster/4/forest_4{0:00}", index/4 + 1);
            }else if (index % 4 == 1) // water
            {
                return string.Format("monster/3/sea_3{0:00}", index / 4 + 1);
            }
            else if (index % 4 == 2) // fire
            {
                return string.Format("monster/6/cave_6{0:00}", index / 4 + 1);
            }
            else if (index % 4 == 3) // dark
            {
                return string.Format("monster/1/undead_1{0:00}", index / 4 + 1);
            }
        }
        return string.Empty;
    }

    //public GameObject GetMonsterIconPrefab(int index)
    //{
    //    if (index < 48)
    //    {
    //        return TheHome.NormalCardPrefab;
    //    }
        
        //if (group == 0)
        //{
        //    return EarthIconPrefabs[index];
        //}
        //else if (group == 1)
        //{
        //    return WaterIconPrefabs[index];
        //}
        //else if (group == 2)
        //{
        //    return FireIconPrefabs[index];
        //}
        //else if (group == 3)
        //{
        //    return DarkIconPrefabs[index];
        //}
        //else if (group == 4)
        //{
        //    return EventEarthIconPrefabs[index];
        //}
        //else if (group == 5)
        //{
        //    return EventWaterIconPrefabs[index];
        //}
        //else if (group == 6)
        //{
        //    return EventFireIconPrefabs[index];
        //}
        //else if (group == 7)
        //{
        //    return EventDarkIconPrefabs[index];
        //}
    //    return null;
    //}
    private void OnApplicationPause(bool pause)
    {
//        SaveGameData();
//        AchievementManager.Instance.SaveAchievementData();
    }
    public void OpenPopupAnimated(GameObject obj)
    {
        OpenPopupAnimated(obj, 1);
    }
    public void OpenPopupAnimated(GameObject obj, float scale)
    {
        OpenPopupAnimated(obj, scale, () => { });
    }
    public void OpenPopupAnimated(GameObject obj, float scale, System.Action callback)
    {
        //obj.SetActive(true);
        //var seq = LeanTween.sequence();
        //seq.append(LeanTween.scale(obj, new Vector3(scale * 0.9f, scale * 0.9f, scale * 0.9f), 0.0f));
        //seq.append(LeanTween.scale(obj, new Vector3(scale, scale, scale), 0.4f).setEaseInOutBack());
        //seq.append(callback);
    }
    public Canvas GetCanvasInCurrentScene()
    {
        Canvas canvas = null;
        foreach (GameObject obj in SceneManager.GetActiveScene().GetRootGameObjects())
        {
            canvas = obj.GetComponent<Canvas>();
            if (canvas != null)
            {
                break;
            }
        }
        return canvas;
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (CurrentPopup != null)
            {
                Destroy(CurrentPopup);
            }
        }
    }
	public void SetPositionY(Transform trans, float y){
		Vector3 pos = trans.position;
		pos.y = y;
		trans.position = pos;
	}

    void Awake()
    {
        //if (!_instance)
        //{
        //    _instance = this;
        //    DontDestroyOnLoad(gameObject);
        //}
    }

    //public bool IsSoundOn()
    //{
    //    return PlayerPrefs.GetInt(Consts.KEY_SOUND_VOLUME, 1) == 1;
    //}
    //public void SetSoundOnOff(bool on)
    //{
    //    PlayerPrefs.SetInt(Consts.KEY_SOUND_VOLUME, on?1:0);
    //}
    //public void SetSoundVolume(float volume)
    //{
    //    PlayerPrefs.SetFloat(Consts.KEY_SOUND_VOLUME, volume);
    //    PersistentAudioSettings.MixerVolume = volume;
    //}
    //public float GetSoundVolume()
    //{
    //    return PlayerPrefs.GetFloat(Consts.KEY_SOUND_VOLUME, 1);
    //}
    //public bool IsMusicOn()
    //{
    //    return PlayerPrefs.GetInt(Consts.KEY_MUSIC_VOLUME, 1) == 1;
    //}
    //public void SetMusicVolume(float volume)
    //{
    //    PlayerPrefs.SetFloat(Consts.KEY_MUSIC_VOLUME, volume);
    //    PersistentAudioSettings.MusicVolume = volume;
    //}
    //public float GetMusicVolume()
    //{
    //    return PlayerPrefs.GetFloat(Consts.KEY_MUSIC_VOLUME, 1);
    //}
    //public void SetMusicOnOff(bool on)
    //{
    //    PlayerPrefs.SetInt(Consts.KEY_MUSIC_VOLUME, on ? 1 : 0);
    //}
    //public void PlayAudioClip(AudioClip clip)
    //{
    //    if (IsSoundOn())
    //    {
    //        AudioSource source = GetComponent<AudioSource>();
    //        if (source == null)
    //        {
    //            source = gameObject.AddComponent<AudioSource>();
    //        }
    //        source.clip = clip;
    //        source.Play();
    //    }
    //}
    //public void PlaySound(GameObject gameObj)
    //{
    //    if (IsSoundOn())
    //    {
    //        AudioSource source = gameObj.GetComponent<AudioSource>();
    //        if (source != null)
    //        {
    //            source.Play();
    //        }
    //    }
    //}
    //public void PlayBGM(GameObject gameObj)
    //{
    //    AudioSource source = gameObj.GetComponent<AudioSource>();
    //    if (source != null)
    //    {
    //        BGMAudioSource = source;
    //        source.Play();
    //    }
    //}
    public static float GetAngle(Vector3 pos1, Vector2 pos2)
    {
        float xGap = pos2.x - pos1.x;
        float yGap = pos2.y - pos1.y;
        return Mathf.Atan2(yGap, xGap) * 180 / Mathf.PI;
    }
    public static float GetAngle(Vector2 pos1, Vector2 pos2)
    {
        float xGap = pos2.x - pos1.x;
        float yGap = pos2.y - pos1.y;
        return Mathf.Atan2(yGap, xGap) * 180 / Mathf.PI;
    }

    public static Vector3 GetDistanceByAngleAndSpeed(double angle, float speed)
    {
        double theta = angle * Math.PI / 180;
        return new Vector3((float)Math.Cos(theta) * speed, (float)Math.Sin(theta) * speed, 0);
    }
    public float GetDistance(Transform t1, Transform t2)
    {
        return Mathf.Sqrt(Mathf.Pow(t1.position.x - t2.position.x, 2) +
                        Mathf.Pow(t1.position.y - t2.position.y, 2) +
                        Mathf.Pow(t1.position.z - t2.position.z, 2));
    }

    //public void MoveUpAndDestroy(GameObject obj, float scale, float time, float fadeOutDelay = 2)
    //{
    //    obj.transform.DOMove(new Vector3(obj.transform.position.x, obj.transform.position.y + 1, 1), time).SetEase(Ease.OutSine);
    //    ScaleAndFadeOut(obj, time, scale, time, fadeOutDelay);
    //}
    //public void FadeInAndOutUI(GameObject obj, float inTime, float outTime)
    //{
    //    var seq = LeanTween.sequence();
    //    seq.append(LeanTween.value(obj, 1, 0, 0.2f).setOnUpdate((float val) => {
    //        obj.GetComponent<MaskableGraphic>().color = new Color(0.1960784f, 0.1960784f, 0.1960784f, val);
    //    }));
    //    seq.append(0.3f);

    //    seq.append(LeanTween.value(obj, 0, 1, 0.2f).setOnUpdate((float val) => {
    //        obj.GetComponent<MaskableGraphic>().color = new Color(0.1960784f, 0.1960784f, 0.1960784f, val);
    //    }));
    //    seq.append(0.2f);
    //    seq.append(() => { FadeInAndOutUI(obj, inTime, outTime); });
    //    //Debug.Log("FadeInAndOut!");
    //}
    //public void FadeOutAndDestroy(GameObject obj, float time, float delay = 0)
    //{
    //    FadeOutAndDestroy fadeOut = obj.AddComponent<FadeOutAndDestroy>();
    //    fadeOut.StartFadeOut(time, delay);
    //}
    //public void ScaleAndFadeOut(GameObject obj, float scaleTime, float scale, float fadeTime, float fadeOutDelay = 1)
    //{
    //    FadeOutAndDestroy(obj, fadeTime, fadeOutDelay);
    //    obj.transform.DOScale(scale, scaleTime).SetEase(Ease.OutElastic);
    //}
    public string GetTimeLeftString(int seconds)
    {
        if (seconds > 3599)
        {
            return string.Format("{0:00}:{1:00}:{2:00}", seconds / 3600, seconds / 60, seconds % 60);
        }
        else
        {
            return string.Format("{0:00}:{1:00}", seconds / 60, seconds % 60);
        }
    }
}

