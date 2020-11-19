using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class UnitInfo
{
    public int SocialID = 0;
    public int Count = 0;
    public int Level = 0;
    public int Enforced = 0;
    public int Index = 0;

    public UnitInfo()
    {

    }
    public UnitInfo(int index)
    {
        Index = index;
    }
    public UnitInfo(UnitInfo info)
    {
        this.SocialID = info.SocialID;
        this.Count = info.Count;
        this.Level = info.Level;
        this.Enforced = info.Enforced;
        this.Index = info.Index;
    }
    public UnitInfo(int id, int count, int level, int enforced, int index)
    {
        SocialID = id;
        Count = count;
        Level = level;
        Enforced = enforced;
        Index = index;
    }

    //public UnitInfo(string str)
    //{
    //    Init(str);   
    //}
    //public void Init(string str)
    //{
    //    string[] strDetails = str.Split(',');
    //    SocialID = int.Parse(strDetails[0]);
    //    Count = int.Parse(strDetails[1]);
    //    Level = int.Parse(strDetails[2]);
    //    Rank = int.Parse(strDetails[3]);
    //    Group = int.Parse(strDetails[4]);
    //    Index = int.Parse(strDetails[5]);
    //}
    //public override string ToString()
    //{
    //    return string.Format("{0},{1},{2},{3},{4},{5}", SocialID, Count, Level, Rank, Group, Index);
    //}
}
