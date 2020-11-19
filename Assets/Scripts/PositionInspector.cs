using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionInspector : MonoBehaviour {
    public Vector3 LocalPosition
    {
        get { return transform.localPosition; }
    }
    public Vector3 GlobalPosition
    {
        get { return transform.position; }
    }
}
