using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using BestHTTP;
using DG.Tweening;
using UnityEngine;

public class ServerManager : MonoBehaviour
{
    private static ServerManager _instance;
    public static ServerManager Instance
    {
        get
        {
            if (!_instance)
            {
                _instance = GameObject.FindObjectOfType(typeof(ServerManager)) as ServerManager;
                if (!_instance)
                {
                    GameObject container = new GameObject();
                    container.name = "ServerManager Instance" + UnityEngine.Random.Range(0, 1000);
                    _instance = container.AddComponent(typeof(ServerManager)) as ServerManager;
                    DontDestroyOnLoad(_instance);
                }
            }
            return _instance;
        }
    }
    DateTime _receivedTime;
    DateTime _receivedLocalTime;
    public bool IsTimeReady = false;
    public bool IsFromGoogleDotCom = false;
    public event OnEventDelegate TimeRefreshed;
    public string serverUrl = "http://222.120.115.95:8103";
    // Start is called before the first frame update
    void Start()
    {
        _receivedTime = DateTime.Now;
        _receivedLocalTime = DateTime.Now;
        GetHTTPTime();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GetHTTPTime()
    {
        Debug.Log("get http time");
        if (IsFromGoogleDotCom)
        {
            serverUrl = "https://time.google.com";
        }
        
        IsTimeReady = false;
        HTTPRequest request = new HTTPRequest(new Uri(serverUrl + "/time"), OnTimeRequestFinished);
        request.Send();
        
    }
    bool IsRequestSuccess(HTTPRequest request, HTTPResponse response)
    {
        switch (request.State)
        {
            // The request finished without any problem.
            case HTTPRequestStates.Finished:
                if (response.IsSuccess)
                {
                    // Everything went as expected!
                    return true;
                }
                else
                {
                    Debug.LogWarning(string.Format("Request finished Successfully, but the server sent an error. Status Code: {0}-{1} Message: {2}",
                                                    response.StatusCode,
                                                    response.Message,
                                                    response.DataAsText));
                }
                break;

            // The request finished with an unexpected error. The request's Exception property may contain more info about the error.
            case HTTPRequestStates.Error:
                Debug.LogError("Request Finished with Error! " + (request.Exception != null ? (request.Exception.Message + "\n" + request.Exception.StackTrace) : "No Exception"));
                break;

            // The request aborted, initiated by the user.
            case HTTPRequestStates.Aborted:
                Debug.LogWarning("Request Aborted!");
                break;

            // Connecting to the server is timed out.
            case HTTPRequestStates.ConnectionTimedOut:
                Debug.LogError("Connection Timed Out!");
                break;

            // The request didn't finished in the given time.
            case HTTPRequestStates.TimedOut:
                Debug.LogError("Processing the request Timed Out!");
                break;
        }
        return false;
    }
    void OnTimeRequestFinished(HTTPRequest request, HTTPResponse response)
    {
        if(IsRequestSuccess(request, response))
        {
            Debug.Log("Request Finished! Text received: " + response.DataAsText);
            if (IsFromGoogleDotCom)
            {
                DateTime theTime;
                if (TryGetDate(response.DataAsText, out theTime))
                {
                    Debug.Log("time from google success");
                    SetTime(theTime);
                }
                else
                {
                    Debug.Log("time from google failed: " + response.DataAsText);
                }
            }
            else
            {
                SetTime(Convert.ToDateTime(response.DataAsText));
            }
            
        }
        else
        {
            Debug.Log("time Request failed! Text received: " + response.DataAsText);
            var seq = DOTween.Sequence();
            seq.PrependInterval(1.0f).OnComplete(() =>
            {
                GetHTTPTime();
            });
        }
    }
    private static bool TryGetDate(string source, out DateTime date)
    {
        try
        {
            var chars = source.ToCharArray(5, 20);

            var d1 = chars[0] & 0x0f;
            var d2 = chars[1] & 0x0f;
            var day = d1 * 10 + d2;

            var mo1 = (int)chars[4];
            var mo2 = (int)chars[5];
            var sum = mo1 + mo2;
            int month;

            switch (sum)
            {
                case 207:
                    month = 1;
                    break;
                case 199:
                    month = 2;
                    break;
                case 211:
                    month = 3;
                    break;
                case 226:
                    month = 4;
                    break;
                case 218:
                    month = 5;
                    break;
                case 227:
                    month = 6;
                    break;
                case 225:
                    month = 7;
                    break;
                case 220:
                    month = 8;
                    break;
                case 213:
                    month = 9;
                    break;
                case 215:
                    month = 10;
                    break;
                case 229:
                    month = 11;
                    break;
                case 200:
                    month = 12;
                    break;
                default:
                    month = 1;
                    break;
            }

            var y1 = chars[7] & 0x0f;
            var y2 = chars[8] & 0x0f;
            var y3 = chars[9] & 0x0f;
            var y4 = chars[10] & 0x0f;
            var year = y1 * 1000 + y2 * 100 + y3 * 10 + y4;

            var h1 = chars[12] & 0x0f;
            var h2 = chars[13] & 0x0f;
            var hour = h1 * 10 + h2;

            var m1 = chars[15] & 0x0f;
            var m2 = chars[16] & 0x0f;
            var minute = m1 * 10 + m2;

            var s1 = chars[18] & 0x0f;
            var s2 = chars[19] & 0x0f;
            var second = s1 * 10 + s2;

            date = new DateTime(year, month, day, hour, minute, second, DateTimeKind.Utc);

            return true;
        }
        catch (Exception e)
        {
            Debug.LogError("Error while parsing date: " + e);
            date = default(DateTime);
            return false;
        }
    }
    public void SetTime(DateTime theTime)
    {
        _receivedTime = theTime;
        _receivedLocalTime = DateTime.Now;
        IsTimeReady = true;
        TimeRefreshed?.Invoke();
    }
    public DateTime GetCurrentTime()
    {
        return _receivedTime + (DateTime.Now - _receivedLocalTime);
    }
    void OnApplicationFocus(bool hasFocus)
    {
        Debug.Log("Enter foreground");
        GetHTTPTime();
    }
}
