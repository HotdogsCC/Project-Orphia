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
    private Animator animator;
    private List<string> messages = new List<string>();
    private int currentMessageCount = 0;
    private void Start()
    {
        animator = GetComponent<Animator>();
    }
    public void SetMessages(List<string> messageBoxMsgs)
    {
        player.ResetVelocity();
        player.GetComponent<PlayerMovement>().enabled = false;
        currentMessageCount = 0;
        messages = messageBoxMsgs;
        text.text = messages[currentMessageCount];
        animator.SetBool("On", true);
    }

    private void LoadNextMessage()
    {
        currentMessageCount++;
        if(currentMessageCount+1 > messages.Count)
        {
            player.GetComponent<PlayerMovement>().enabled = true;
            animator.SetBool("On", false);
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
