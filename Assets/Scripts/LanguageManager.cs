using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
public enum AvailableLanauge
{
    English,
    Korean
}
public class LanguageManager : MonoBehaviour {
    public string FilePath = "MergeWar Language - language";
    private Dictionary<string, Dictionary<string, string>> LanguageDic = new Dictionary<string, Dictionary<string, string>>();
    private string[] _languageArray;
    public event EventHandler LanguageChanged;
	public Font ftGoyang;
	public Font ftEnglish;
	public Font ftArial;
	#region Singleton
	private static LanguageManager _instance;
    public static LanguageManager Instance
    {
        get
        {
            if (!_instance)
            {
                _instance = GameObject.FindObjectOfType(typeof(LanguageManager)) as LanguageManager;
                if (!_instance)
                {
                    GameObject container = new GameObject();
                    container.name = "LanguageManager Instance" + UnityEngine.Random.Range(0, 1000);
                    _instance = container.AddComponent(typeof(LanguageManager)) as LanguageManager;
                }
                _instance.LoadCSV();
            }

            return _instance;
        }
    }
    #endregion
    // Use this for initialization
    void Start () {
    }
	public void Init()
    {

    }
    public void LoadCSV()
	{
		string _alternatePrefix = "29ffkkdjel_";
		List<string> _alternateList = new List<string> ();
		var ta = Resources.Load(FilePath) as TextAsset;
		if (ta == null) return;
		//Debug.Log(string.Format("text fiel: {0}", ta.ToString()));
		string wholeText = ta.text;
		//string commaAlternate = "`";
		int alternateCount = 0;
		if (wholeText.Contains("\"") ) {
			//Debug.Log("** started");
			int inspectStartIndex = 0;
			int quoteIndex = 0;
			bool isQuoteOn = false;

			while ((quoteIndex = wholeText.IndexOf("\"", inspectStartIndex)) >= 0){
				//Debug.Log("** started1");
				isQuoteOn = !isQuoteOn;

				inspectStartIndex = quoteIndex;
				//Debug.Log("**  inspectStartIndex: " + inspectStartIndex.ToString());

				int quoteEndIndex = wholeText.IndexOf("\"", inspectStartIndex+1);
                //Debug.Log("**  quoteEndIndex: " + quoteEndIndex.ToString());

                string quotation = wholeText.Substring (quoteIndex, quoteEndIndex - quoteIndex);
				wholeText = wholeText.Remove (quoteIndex, quoteEndIndex - quoteIndex + 1);
				wholeText = wholeText.Insert (quoteIndex, string.Format ("{0}{1}", _alternatePrefix, alternateCount));
				alternateCount++;
				_alternateList.Add (quotation);
			}
			//Debug.Log("** end");
		}

		var arrayString = wholeText.Split('\n');
		int index = 0;

		foreach (var line in arrayString)
		{
			
			var values = line.Split(',');

			if (index == 0)
			{
				foreach (string str in values)
				{
					if (!string.IsNullOrEmpty(str) && !LanguageDic.ContainsKey(str))
					{
						Dictionary<string, string> dic = new Dictionary<string, string>();
						string key = str.Replace(System.Environment.NewLine, "");
						LanguageDic.Add(key, dic);
					}
				}
				_languageArray = values;
			}
			else
			{
				for (int i = 1; i < _languageArray.Length; i++)
				{
					string str = values [i];
					if (str.StartsWith (_alternatePrefix)) {
						int atIndex = int.Parse (str.Substring (_alternatePrefix.Length));
						str = _alternateList [atIndex];
						str = str.Replace ("\"", "");
					}
                    if (!LanguageDic[_languageArray[i]].ContainsKey(values[0]))
					{
						LanguageDic[_languageArray[i]].Add(values[0], str);
					}
				}
			}
			index++;
		}
		try
		{
			


			/*
	        var reader = new StreamReader(string.Format("{0}/{1}", Application.dataPath, FilePath));
	        while (!reader.EndOfStream)
	        {
	            var line = reader.ReadLine();
				var values = line.Split(',');
	            if (index == 0)
	            {
	                foreach (string str in values)
	                {
	                    if (!string.IsNullOrEmpty(str))
	                    {
	                        Dictionary<string, string> dic = new Dictionary<string, string>();
	                        LanguageDic.Add(str, dic);
	                    }
	                }
	                _languageArray = values;
	            }
	            else
	            {
	                for (int i = 1; i < _languageArray.Length; i++)
	                {
	                    LanguageDic[_languageArray[i]].Add(values[0], values[i]);
	                }
	            }
	            index++;
	        }*/
		}catch(Exception e){
			Debug.Log (e.ToString ());
		}
		
		//SetCurrentLanguage((SystemLanguage)PlayerPrefs.GetInt(Consts.KEY_LANGUAGE, (int)SystemLanguage.English));
		//Debug.Log ("loaded: " + LanguageDic ["english"].Count.ToString());
    }
	public string GetText(string key)
    {
		//Debug.Log(string.Format("get text {0}", key));
		string language;
		if (GetCurrentLanguage() == SystemLanguage.Korean)
		{
            language = "ko";
            //language = _languageArray[2];
		}
		else // english
		{
			language = "en";
		}
        if (LanguageDic.ContainsKey(language) && LanguageDic[language].ContainsKey(key))
        {
			return LanguageDic[language][key];
		}
        else
        {
			return key;
        }
    }
    public void SetCurrentLanguage(SystemLanguage lang)
    {
		//PlayerPrefs.SetInt(Consts.KEY_LANGUAGE, (int)lang);
		SaveData.Instance.Data.Language = (int)lang;
		LanguageChanged?.Invoke(this, new EventArgs());
    }
    public SystemLanguage GetCurrentLanguage()
    {
		int savedLanguage = SaveData.Instance.Data.Language;//PlayerPrefs.GetInt(Consts.KEY_LANGUAGE, -1);
		//Debug.Log(string.Format("system language: {0}, saved:{1}", Application.systemLanguage, savedLanguage));
		SystemLanguage currentLang;
        if (savedLanguage < 0) {
            currentLang = Application.systemLanguage;
        }
        else
        {
			currentLang = (SystemLanguage)savedLanguage;
        }

        if (currentLang == SystemLanguage.Korean)
        {
			return SystemLanguage.Korean;
        }
        else
        { 
			return SystemLanguage.English;
        }
    }
	public Font GetFont()
    {
		return GetFont(GetCurrentLanguage());
    }

	public Font GetFont(SystemLanguage lang)
    {
        if(lang == SystemLanguage.Korean)
        {
			return ftGoyang;
        }
        else if (lang == SystemLanguage.English)
		{
			return ftGoyang;
		}
		else
        {
			return ftArial;
        }
    }
}
