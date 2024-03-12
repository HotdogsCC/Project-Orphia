using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    [SerializeField] private string messageText = "Change me!!";
    private DialogueBox dialogueBox;

    private void Start()
    {
        dialogueBox = FindFirstObjectByType<DialogueBox>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        dialogueBox.SetText(messageText);
    }
}
