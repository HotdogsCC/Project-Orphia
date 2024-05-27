using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    [SerializeField] private List<string> messages = new List<string>();
    [SerializeField] private List<Sprite> images = new List<Sprite>();
    private DialogueBox dialogueBox;

    private void Start()
    {
        dialogueBox = FindFirstObjectByType<DialogueBox>();
    }

    //sends messages to the dialogue box script
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            dialogueBox.SetMessages(messages, images);
            Destroy(this);
        }
    }
}
