using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TimerUpdater : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI timer;

    private void Start()
    {

    }
    // Update is called once per frame
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
    }
}
