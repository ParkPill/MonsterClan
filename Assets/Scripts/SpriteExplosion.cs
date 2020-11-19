using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class SpriteExplosion : MonoBehaviour
{
    public Transform Sprite;
    public float FadeDuration = 0.3f;
    public float FadeDelay = 0.1f;
    private float _fadeDelayChecker;
    private float _fadeTimeChecker;
    public ParticleSystem Particle;
    public float InitSpriteScaleX = 0.7f;
    public float InitSpriteScaleY = 0.2f;
    public float InitSpriteScaleZ = 0.7f;
    public float SpriteEndScaleX = 2;
    public float SpriteEndScaleY = 0.64f;
    // Start is called before the first frame update
    void Start()
    {
        Play();
    }

    public void Play()
    {
        Sprite.localScale = new Vector3(InitSpriteScaleX, InitSpriteScaleY, InitSpriteScaleZ);
        Sprite.DOScale(new Vector3(SpriteEndScaleX, SpriteEndScaleY, Sprite.localScale.z), FadeDelay + FadeDuration);

        //Sprite.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
        _fadeTimeChecker = FadeDuration;
        _fadeDelayChecker = FadeDelay;
        Particle.Play();
    }

    // Update is called once per frame
    void Update()
    {
        if (_fadeTimeChecker > 0)
        {
            float dt = Time.deltaTime;
            if (_fadeDelayChecker > 0)
            {
                _fadeDelayChecker -= dt;
            }
            else
            {
                _fadeTimeChecker -= dt;
                Sprite.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, _fadeTimeChecker / FadeDuration);
            }
        }
    }
}
