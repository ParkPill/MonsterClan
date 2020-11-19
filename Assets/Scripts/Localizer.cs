using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Localizer : MonoBehaviour {
    public string LocalizingKey = string.Empty;
    public TMP_FontAsset TMPKorean;
    public TMP_FontAsset TMPEnglish;
    // Use this for initialization
    void Start () {
        UpdateLanguage();
        LanguageManager.Instance.LanguageChanged += OnLanguageChanged;
	}

    private void OnLanguageChanged(object sender, System.EventArgs e)
    {
        UpdateLanguage();
    }

    public void UpdateLanguage()
    {
		if (GetComponent<Image>() != null)
        {
            Debug.Log(LocalizingKey);
            GetComponent<Image>().sprite = Resources.Load<Sprite>(LanguageManager.Instance.GetText(LocalizingKey));
            GetComponent<Image>().SetNativeSize();
        }
		else if (GetComponent<Text>() != null)
        {
            GetComponent<Text>().text = LanguageManager.Instance.GetText(LocalizingKey);
            GetComponent<Text>().font = LanguageManager.Instance.GetFont();
        }
        else if (GetComponent<TextMeshProUGUI>() != null)
        {
            GetComponent<TextMeshProUGUI>().text = LanguageManager.Instance.GetText(LocalizingKey);
            if (LanguageManager.Instance.GetCurrentLanguage() == SystemLanguage.Korean)
            {
                GetComponent<TextMeshProUGUI>().font = TMPKorean;
            }
            else
            {
                GetComponent<TextMeshProUGUI>().font = TMPEnglish;
            }
        }
    }
	// Update is called once per frame
	void Update () {
		
	}
}
