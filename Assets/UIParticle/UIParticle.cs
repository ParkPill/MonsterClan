using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIParticle : MonoBehaviour
{
    public GameObject ParticlePrefab;
    private float _width, _height;
    public int CountPerSecond = 5;
    
    public int Angle = -90;
    public float Speed = 0.4f;
    public float LifeMin = 1;
    public float LifeMax = 3;
    public float SizeMin = 0.1f;
    public float SizeMax = 1;
    private float _nextTime = 0;
    public bool IsBurst;
    // Start is called before the first frame update
    void Start()
    {
        _width = GetComponent<RectTransform>().sizeDelta.x;
        _height = GetComponent<RectTransform>().sizeDelta.y;
        if (IsBurst)
        {
            Burst();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (IsBurst)
        {
            return;
        }
        float dt = Time.deltaTime;
        _nextTime -= dt;
        if (_nextTime <= 0)
        {
            CreateParticle();
            _nextTime += Random.Range(0, 1.0f / CountPerSecond);
        }
    }
    public void Burst()
    {
        int count = (int)(CountPerSecond * (LifeMax - LifeMin) / 2);
        for (int i = 0; i < count; i++)
        {
            CreateParticle();
        }
    }
    void CreateParticle()
    {
        GameObject obj = Instantiate(ParticlePrefab, transform);
        obj.SetActive(true);
        float x = -_width / 2 + Random.Range(0, _width);
        float y = -_height / 2 + Random.Range(0, _height);
        obj.transform.position = new Vector3(transform.position.x + x,
                                            transform.position.y + y,
                                            obj.transform.position.z);
        ParticleLife pl = obj.GetComponent<ParticleLife>();
        float scale = Random.Range(SizeMin, SizeMax);
        pl.transform.localScale = new Vector3(scale, scale, scale);
        pl.Angle = Angle;
        pl.Speed = Speed;
        pl.StayDur = Random.Range(LifeMin, LifeMax);
        pl.AppearDur = pl.StayDur * 0.1f;
        pl.DisappearDur = pl.StayDur * 0.2f;
        if (IsBurst)
        {
            float halfWidth = _width / 2;
            float halfHeight = _height / 2;
            float burstPower = Random.Range(1.5f, 3.5f);
            pl.BurstPower = (x*x + y*y)*burstPower/(halfWidth*halfWidth + halfHeight*halfHeight);
            pl.BurstAngle = Mathf.Atan2(y, x)*180/3.14f;
        }
    }
}
