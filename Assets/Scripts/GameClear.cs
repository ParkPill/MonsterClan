using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameClear : MonoBehaviour
{
    public bool IsMultiplay = false;
    public List<GameObject> StarFillList;
    public List<GameObject> StarParticleList;
    public GameObject EntireParticle;
    public GameObject BtnGet;
    public GameObject BtnVideo;
    private float _timeChecker;
    private float _extraTimeForBtnGet = 2;
    public int StarCount = 3;
    public GameObject BGGlow;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float dt = Time.deltaTime;
        _timeChecker += dt;
        if (_timeChecker > 1)
        {
            _timeChecker -= 1;
            if (!StarFillList[0].activeSelf)
            {
                StarFillList[0].SetActive(true);
                StarParticleList[0].SetActive(true);
            }
            else if (!StarFillList[1].activeSelf && StarCount > 1)
            {
                StarFillList[1].SetActive(true);
                StarParticleList[1].SetActive(true);
            }
            else if (!StarFillList[2].activeSelf && StarCount > 2)
            {
                StarFillList[2].SetActive(true);
                StarParticleList[2].SetActive(true);
                EntireParticle.SetActive(true);
                BGGlow.SetActive(true);
            }
            else if (!BtnVideo.activeSelf)
            {
                BtnVideo.SetActive(true);
            }
        }
        if (BtnVideo.activeSelf && !BtnGet.activeSelf)
        {
            _extraTimeForBtnGet -= dt;
            if (_extraTimeForBtnGet < 0)
            {
                BtnGet.SetActive(true);
            }
        }
    }

    public void OnGetClick()
    {
        SceneManager.LoadScene("Lobby");
    }
    public void OnVideoClick()
    {

    }
}
