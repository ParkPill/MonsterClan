using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class ScaleUpWhenVisible : MonoBehaviour
{
    public float InitScale = 0.1f;
    public float TargetScale = 1;
    public float ScailingDur = 0.5f;
    // Start is called before the first frame update
    void Start()
    {
        transform.localScale = new Vector3(InitScale, InitScale, InitScale);
        transform.DOScale(TargetScale, ScailingDur).SetEase(Ease.OutBack);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
