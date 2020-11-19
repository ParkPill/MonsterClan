using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    public TMPro.TextMeshProUGUI LblTitle;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    public void OnOkClick()
    {
        SceneManager.LoadScene("Lobby");
    }
    public void OnVideoClick()
    {

    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
