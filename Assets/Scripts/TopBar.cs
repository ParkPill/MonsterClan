using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TopBar : MonoBehaviour
{
    
    // Start is called before the first frame update
    void Start()
    {
        DataManager.Instance.OnCurrencyChanged.AddListener(OnCurrencyChanged);
        transform.Find("StatusGold").Find("lbl").GetComponent<UINumberRaiser>().Number = DataManager.Instance.Gold;
        transform.Find("StatusGem").Find("lbl").GetComponent<UINumberRaiser>().Number = DataManager.Instance.Gem;
        UpdateCurrency();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void OnCurrencyChanged()
    {
        UpdateCurrency();
    }
    public void UpdateCurrency()
    {
        if (transform.Find("StatusGold").Find("lbl").GetComponent<UINumberRaiser>().Number > DataManager.Instance.Gold)   
        {
            transform.Find("StatusGold").Find("lbl").GetComponent<UINumberRaiser>().Number = DataManager.Instance.Gold;
        }

        if (transform.Find("StatusGem").Find("lbl").GetComponent<UINumberRaiser>().Number > DataManager.Instance.Gem)
        {
            transform.Find("StatusGem").Find("lbl").GetComponent<UINumberRaiser>().Number = DataManager.Instance.Gem;
        }
        
        //transform.Find("StatusEnergy").Find("lbl").GetComponent<UINumberRaiser>().Number = DataManager.Instance.Energy;
    }
}
