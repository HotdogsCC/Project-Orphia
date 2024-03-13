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
    public void SetText(string msg)
    {
        Time.timeScale = 0;
        text.text = msg;
        gb.SetActive(true);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Time.timeScale = 1;
            gb.SetActive(false);
        }
    }
}
