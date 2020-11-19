using System.Collections;
using System.Collections.Generic;
using System.Text;
using CodeStage.AntiCheat.ObscuredTypes;
using UnityEngine;

public class BigNum
{
    char splitChar = '.';
    //int unitIndex = 0;
    List<ObscuredInt> numbers = new List<ObscuredInt>();
    public BigNum()
    {
        numbers.Clear();
        numbers.Add(0);
    }
    public BigNum(int num) : this()
    {
        AddNum(num, 0);
    }
    public BigNum(float num) : this()
    {
        AddNum((int)num, 0);
    }
    public BigNum(string str) : this()
    {
        if (string.IsNullOrEmpty(str))
        {
            numbers.Clear();
            numbers.Add(0);
        }
        else
        {
            SetNumFromFullNumberByCommaSplitString(str);
        }
    }
    public BigNum(BigNum init) : this()
    {
        AddNum(init);
    }
    public void resetNum()
    {
        numbers.Clear();
        numbers.Add(0);
    }
    public void SetNumFromFullNumberByCommaSplitString(string str)
    {
        numbers.Clear();
        //ValueVector rows = GameManager::getInstance().split(str, splitChar);
        string[] rows = str.Split(splitChar);
        string num;
        for (int i = 0; i < (int)rows.Length; i++)
        {
            num = rows[i];
            //        Value(num).asString();
            //Debug.Log(string.Format("num: {0}", num));
            numbers.Add(int.Parse(num));
        }
    }
    public bool IsZeroOrUnder()
    {
        return numbers.Count >= 1 && numbers[0] <= 0;
    }
    public string GetNumForSave()
    {
        StringBuilder str = new StringBuilder();
        string num;
        for (int i = 0; i < (int)numbers.Count; i++)
        {
            num = numbers[i].ToString();
            str.Append(num);
            if (numbers.Count > i + 1)
            {
                str.Append(splitChar);
            }
        }
        return str.ToString();
    }
    public void AddNum(int amount)
    {
        AddNum(amount, 0);
    }
    public void AddNum(int amount, int unit)
    {
        for (int i = (int)numbers.Count; i < unit + 1; i++)
        { // create if not exist
            numbers.Add(0);
        }
        numbers[unit] += amount;
        Arrange();
    }
    public void AddNum(BigNum num)
    {
        //Debug.Log(string.Format("num: {0}", num));
        for (int i = (int)numbers.Count; i < num.numbers.Count; i++)
        { // create if not exist
            numbers.Add(0);
        }

        for (int i = 0; i < num.numbers.Count; i++)
        {
            numbers[i] += num.numbers[i];
        }
        Arrange();
    }
    public void SubtractNum(int amount, int unit)
    {
        AddNum(-amount, unit);
    }
    public void SubtractNum(BigNum num)
    {
        for (int i = (int)numbers.Count; i < num.numbers.Count; i++)
        { // create if now exist
            numbers.Add(0);
        }

        for (int i = 0; i < num.numbers.Count; i++)
        {
            numbers[i] -= num.numbers[i];
        }
        Arrange();
    }
    public void Arrange()
    {
        bool positiveExist = false;
        bool negativeExist = false;
        bool biggestNumIsPositive = false;
        int num = 0;
        for (int i = 0; i < numbers.Count; i++)
        {
            num = numbers[i];
            if (num != 0)
            {
                biggestNumIsPositive = num > 0;
            }
            if (num > 0)
            {
                positiveExist = true;
            }
            if (num < 0)
            {
                negativeExist = true;
            }
            if (num > 1000 || num < -1000)
            {
                numbers[i] = num % 1000;
                AddNum(num / 1000, i + 1);
            }
        }

        if (biggestNumIsPositive && negativeExist)
        {
            for (int i = 0; i < numbers.Count - 1; i++)
            {
                num = numbers[i];
                if (num < 0)
                {
                    numbers[i + 1]--;
                    numbers[i] += 1000;
                }
            }
        }
        else if (!biggestNumIsPositive && positiveExist)
        {
            for (int i = 0; i < numbers.Count - 1; i++)
            {
                num = numbers[i];
                if (num > 0)
                {
                    numbers[i + 1]++;
                    numbers[i] -= 1000;
                }
            }
        }
        RemoveUnusedHigherZero();
    }
    public void RemoveUnusedHigherZero()
    {
        while (numbers[numbers.Count - 1] == 0 && numbers.Count > 1)
        {
            numbers.RemoveAt(numbers.Count - 1);
        }
    }
    /// <summary>
    /// 1 bigger, -1 smaller, 0 equal
    /// </summary>
    /// <param name="num"></param>
    /// <returns></returns>
    public int IsBiggerThanThis(BigNum num)
    {
        RemoveUnusedHigherZero();
        num.RemoveUnusedHigherZero();
        if (numbers.Count != num.numbers.Count)
        {
            if (numbers.Count > num.numbers.Count)
            {
                return 1;
            }
            else
            {
                return -1;
            }
        }
        int index = (int)numbers.Count - 1;
        while (index >= 0)
        {
            if (numbers[index] != num.numbers[index])
            {
                if (numbers[index] > num.numbers[index])
                {
                    return 1;
                }
                else
                {
                    return -1;
                }
            }
            index--;
        }
        return 0;
    }
    public void Multiply(float rate)
    {
        int num;
        float rated;
        int integeredNum;
        int dataToAddLowerLevel = 0;
        for (int i = (int)numbers.Count - 1; i >= 0; i--)
        {
            num = numbers[i];
            rated = num * rate;
            integeredNum = (int)(rated + 0.5f); // round off
            numbers[i] = integeredNum;
            numbers[i] += dataToAddLowerLevel;
            if (i > 0 && integeredNum != rated)
            {
                dataToAddLowerLevel = (int)(rated - integeredNum) * 1000;
            }
        }
        Arrange();
    }
    public string GetUnitStr(int unit)
    {
        if (unit <= 0)
        {
            return string.Empty;
        }
        unit = unit - 1;

        int devider = 26;
        int loop = 1 + unit / devider;
        int index = unit % devider;
        char ch = (char)((int)'a' + index);
        if (index == 0)
        {
            ch = 'K';
        }
        else if (index == 1)
        {
            ch = 'M';
        }
        else if (index == 2)
        {
            ch = 'B';
        }
        else
        {
            ch = (char)((int)'a' + index - 3);
        }
        StringBuilder str = new StringBuilder();
        for (int i = 0; i < loop; i++)
        {
            str.Append(ch);
        }
        return str.ToString();
    }
    public string GetFullNumberStringByCommaSplit()
    {
        StringBuilder str = new StringBuilder();
        for (int i = 0; i < numbers.Count; i++)
        {
            str.Append(numbers[i].ToString());
            str.Append(splitChar);
        }
        string finalStr = str.ToString();
        return finalStr.Substring(0, finalStr.Length - 1);
    }
    public int ToNum()
    {
        int total = 0;
        for (int i = 0; i < numbers.Count; i++)
        {
            total += (int)(numbers[i] * Mathf.Pow(1000, i));
        }
        return total;
    }
    public override string ToString()
    {
        return GetExpression();
    }
    public string ToStringFiveChars()
    {
        int unitCount = 0;
        int places = numbers.Count;
        //Debug.Log(string.Format("{0}/{1}", unitCount, places));
        //Debug.Log("places: " + places);
        for (int i = 0; i < 100; i++)
        {
            if (places > 2)
            {
                places--;
                unitCount++;
            }
            else
            {
                break;
            }
        }
        if (places == 2)
        {
            if (numbers[numbers.Count - 1] >= 100)
            {
                places--;
                unitCount++;
            }
        }

        string str = string.Empty;
        for (int i = 0; i < places; i++)
        {
            if (i == 0)
            {
                str += numbers[numbers.Count - 1 - i].ToString();
            }
            else
            {
                str += numbers[numbers.Count - 1 - i].ToString("000");
            }

        }
        str += GetUnitStr(unitCount);
        //if (numbers.Count > 0 &&  numbers[1] == 100)
        //{
        //    Debug.Log(string.Format("{0}/{1}/{2}", unitCount, places, str));
        //}

        return str;

        //if (numbers.Count > 5)
        //{// 1 000 000
        //    int index = numbers.Count-3
        //    str.Append(numbers[numbers.Count - 1]);
        //    int firstDigitPlace = (int)str.Length;
        //    str = str.Append(splitChar);
        //    int digitPlaceLeft = 4 - firstDigitPlace;
        //    digitPlaceLeft = 1;
        //    str = str.Append(GetZeroBaseNum(numbers[numbers.Count - 2], true).Substring(0, digitPlaceLeft));
        //    str = str.Append(GetUnitStr((int)numbers.Count - 1));
        //    return str.ToString();
        //}
        //else
        //{
        //    return numbers[0].ToString();
        //}

    }
    public string GetExpression()
    {
        StringBuilder str = new StringBuilder();
        if (numbers.Count > 1)
        {
            str.Append(numbers[numbers.Count - 1]);
            int firstDigitPlace = (int)str.Length;
            str = str.Append(splitChar);
            int digitPlaceLeft = 4 - firstDigitPlace;
            //digitPlaceLeft = 1;
            str = str.Append(GetZeroBaseNum(numbers[numbers.Count - 2], true).Substring(0, digitPlaceLeft));
            str = str.Append(GetUnitStr((int)numbers.Count - 1));
            return str.ToString();
        }
        else
        {
            return numbers[0].ToString();
        }
    }
    public string GetZeroBaseNum(int num, bool removeNegative)
    {
        StringBuilder str = new StringBuilder();
        bool isNegative = num < 0;
        if (isNegative)
        {
            num *= -1;
            if (!removeNegative)
            {
                str.Append("-");
            }
        }
        if (num >= 100)
        {
            ;
        }
        else if (num >= 10)
        {
            str = str.Append("0");
        }
        else
        {
            str = str.Append("00");
        }

        return str.Append(num.ToString()).ToString();
    }

    public void SetNum(BigNum num)
    {
        numbers.Clear();
        for (int i = 0; i < num.numbers.Count; i++)
        {
            numbers.Add(num.numbers[i]);
        }
    }
}
