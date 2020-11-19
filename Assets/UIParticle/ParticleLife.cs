using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ParticleLife : MonoBehaviour
{
    public float AppearDur = 0.3f;
    public float StayDur = 2;
    public float DisappearDur = 0.8f;
    public float Angle = 90;
    public float Speed = 4;
    private float _timeChecker = 0;
    private Image _image;
    private Color _color;
    public float BurstAngle;
    public float BurstPower = 0;
    // Start is called before the first frame update
    void Start()
    {
        _image = GetComponent<Image>();
        _color = _image.color;
        _image.color = new Color(_color.r, _color.g, _color.b, 0);
    }

    // Update is called once per frame
    void Update()
    {
        float dt = Time.deltaTime;
        _timeChecker += dt;
        if (_timeChecker < AppearDur)
        {
            _image.color = new Color(_color.r, _color.g, _color.b, _timeChecker / AppearDur);
        }
        else if(_timeChecker < AppearDur + StayDur)
        {

        }else if(_timeChecker < AppearDur + StayDur + DisappearDur)
        {
            _image.color = new Color(_color.r, _color.g, _color.b, 1 - (_timeChecker - (AppearDur + StayDur)) / DisappearDur);
        }
        else
        {
            Destroy(gameObject);
        }
        float x = Mathf.Cos(Angle * 3.14f / 180) * Speed;
        float y = Mathf.Sin(Angle * 3.14f / 180) * Speed;
        transform.Translate(new Vector3(x, y, transform.position.z));

        if (BurstPower > 0)
        {
            BurstPower -= dt;
            x = Mathf.Cos(BurstAngle * 3.14f / 180) * BurstPower;
            y = Mathf.Sin(BurstAngle * 3.14f / 180) * BurstPower;
            transform.Translate(new Vector3(x, y, transform.position.z));
        }
    }
}
