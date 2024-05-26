using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.XR;
using UnityEditor;
using UnityEngine.UI;

public class DialogueBox : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private GameObject gb;
    [SerializeField] private PlayerMovement player;
    [SerializeField] private Image characterSpeaking;
    private Animator animator;
    private List<string> messages = new List<string>();
    private List<Sprite> image = new List<Sprite>();
    private int currentMessageCount = 0;
    
    private List<char> charactersInMessage = new List<char>();

    private void Start()
    {
        animator = GetComponent<Animator>();
    }
    public void SetMessages(List<string> messageBoxMsgs, List<Sprite> images)
    {
        charactersInMessage = new List<char>();
        player.ResetVelocity();
        player.ResetAnimations();
        player.GetComponent<PlayerMovement>().enabled = false;
        currentMessageCount = 0;
        messages = messageBoxMsgs;
        image = images;

        characterSpeaking.sprite = image[currentMessageCount];
        foreach (char c in messages[currentMessageCount])
        {
            charactersInMessage.Add(c);
        }

        text.text = null;
        StartCoroutine(JustWaitAMoForTheAnimation());
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
            StopAllCoroutines();
            charactersInMessage = new List<char>();
            foreach (char c in messages[currentMessageCount])
            {
                charactersInMessage.Add(c);
            }
            text.text = null;
            StartCoroutine(DisplayCharacters());
            characterSpeaking.sprite = image[currentMessageCount];
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            LoadNextMessage();
        }
    }

    IEnumerator JustWaitAMoForTheAnimation()
    {
        yield return new WaitForSeconds(1);
        StartCoroutine(DisplayCharacters());
    }

    IEnumerator DisplayCharacters()
    {
        foreach (char c in charactersInMessage)
        {
            text.text = text.text + c;
            yield return new WaitForSeconds(0.01f);
        }
    }
}
