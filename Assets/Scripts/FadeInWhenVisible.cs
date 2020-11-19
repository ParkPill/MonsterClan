using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class FadeInWhenVisible : MonoBehaviour
{
    public float FadeDur = 0.5f;
    private float _timeCheker = 0;
    private Color _originalColor;
    private Image _image;
    // Start is called before the first frame update
    void Start()
    {
        _image = GetComponent<Image>();
        _originalColor = _image.color;
        _image.color = new Color(_originalColor.r, _originalColor.g, _originalColor.b, 0);
    }

    // Update is called once per frame
    void Update()
    {
        if(_image.color.a < 1)
        {
            _timeCheker += Time.deltaTime;
            _image.color = new Color(_originalColor.r, _originalColor.g, _originalColor.b, _timeCheker / FadeDur);
        }
    }
}
