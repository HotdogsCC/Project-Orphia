using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TimerUpdater : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI timer;
    [SerializeField] TextMeshProUGUI killCountTMP;
    [SerializeField] private static int killCount = 0;

    private void Start()
    {
        killCount = 0;
    }
    // Update is called once per frame

    // a lot of strange calculations to get time, in seconds (e.g. 21.3249), to be displayed nicely in a 00:00 format
    void Update()
    {
        int seconds = ((int)Time.timeSinceLevelLoad);
        int minutes = 0;

        while(seconds > seconds % 60)
        {
            seconds += -60;
            minutes++;
        }

        string secStr = seconds.ToString();
        string minStr = minutes.ToString();

        if (seconds < 10)
        {
            secStr = "0" + seconds.ToString();
        }

        if (minutes < 10)
        {
            minStr = "0" + minutes.ToString();
        }

        timer.text = minStr + ":" + secStr;

        killCountTMP.text = killCount.ToString();
    }

    public static void AddToKill()
    {
        killCount++;
    }
}
