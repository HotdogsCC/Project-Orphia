using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.XR;

public class DialogueBox : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private GameObject gb;
    [SerializeField] private PlayerMovement player;
    private List<string> messages = new List<string>();
    private int currentMessageCount = 0;
    public void SetMessages(List<string> messageBoxMsgs)
    {
        Time.timeScale = 0;
        currentMessageCount = 0;
        messages = messageBoxMsgs;
        text.text = messages[currentMessageCount];
        gb.SetActive(true);
    }

    private void LoadNextMessage()
    {
        currentMessageCount++;
        if(currentMessageCount+1 > messages.Count)
        {
            gb.SetActive(false);
            Time.timeScale = 1;
        }
        else
        {
            text.text = messages[currentMessageCount];
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            LoadNextMessage();
        }
    }
}
