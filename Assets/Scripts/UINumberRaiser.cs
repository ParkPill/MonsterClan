using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UINumberRaiser : MonoBehaviour
{
    private bool IsTextMesh = false;
    private TextMeshProUGUI _lblMesh;
    private Text _lbl;
    private float _timeChecker = 0;
    //public float RaiseInterval = 0.2f;
    public float RaiseDur = 0.5f;
    public int Number;
    private int _previousNumber = -1;
    // Start is called before the first frame update
    void Start()
    {
        SetUI();
        IsTextMesh = _lblMesh != null;
    }
    void SetUI()
    {
        _lblMesh = GetComponent<TextMeshProUGUI>();
        _lbl = GetComponent<Text>();
    }
    public void Invalidate()
    {
        _previousNumber = Number - 1;
    }
    // Update is called once per frame
    void Update()
    {
        if (_previousNumber == Number)
        {
            return;
        }
        _timeChecker += Time.deltaTime;
        if (_timeChecker >= RaiseDur)
        {
            _timeChecker = 0;
            _previousNumber = Number;
        }
        float rate = 0.1f;
        if (_previousNumber < Number)
        {
            _previousNumber += (int)((Number - _previousNumber) * rate);
            if (_previousNumber > Number)
            {
                _previousNumber = Number;
            }
        }
        else if (_previousNumber > Number)
        {
            _previousNumber -= (int)((_previousNumber - Number) * rate);
            if (_previousNumber < Number)
            {
                _previousNumber = Number;
            }
        }
        if (IsTextMesh)
        {
            _lblMesh.text = GetFormattedNumber(_previousNumber);
        }
        else
        {
            _lbl.text = GetFormattedNumber(_previousNumber);
        }
    }
    public int GetNumber(string text)
    {
        BigNum num = new BigNum(text);
        return num.ToNum();
            
        //int multiply = 1;
        //if (text.EndsWith("K"))
        //{
        //    multiply *= 1000;
        //}
        //else if (text.EndsWith("M"))
        //{
        //    multiply *= 1000000;
        //}
        //else if (text.EndsWith("B"))
        //{
        //    multiply *= 1000000000;
        //}
        //int number = 0;
        //if (multiply == 1)
        //{
        //    number = int.Parse(text); 
        //}
        //else
        //{
        //    number = int.Parse(text.Substring(0, text.Length - 1));
        //}
        //return number*multiply;
    }
    public void SetNumberAndText(int num)
    {
        if (_lbl == null && _lblMesh == null)
        {
            _lblMesh = GetComponent<TextMeshProUGUI>();
            _lbl = GetComponent<Text>();
        }
        //Debug.Log("what?");
        Number = num;
        _previousNumber = num;
        if (IsTextMesh)
        {
            _lblMesh.text = GetFormattedNumber(num);
        }
        else
        {
            _lbl.text = GetFormattedNumber(num);
        }
    }
    public string GetFormattedNumber(int num)
    {
        BigNum bn = new BigNum(num);
        return bn.ToString();
        //if (num >= 1000000000)
        //{
        //    return string.Format("{0}B", num / 1000000000);
        //}else if (num >= 1000000)
        //{
        //    return string.Format("{0}M", num / 1000000);
        //}
        //else if (num >= 1000)
        //{
        //    return string.Format("{0}K", num / 1000);
        //}
        //return num.ToString();
    }
}
