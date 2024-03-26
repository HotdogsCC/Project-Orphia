using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class Companion : MonoBehaviour
{
    [SerializeField] Camera cam;
    [SerializeField] private GameObject player;
    [SerializeField] float moveSpeed = 1f;
    private Vector3 targetPosition;
    private bool companionMouseFollow = false;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            companionMouseFollow = !companionMouseFollow;
        }

        if (companionMouseFollow)
        {
            targetPosition = cam.ScreenToWorldPoint(Input.mousePosition);
            
        }
        else
        {
            targetPosition = player.transform.position;
        }

        targetPosition = new Vector3(targetPosition.x, targetPosition.y, 0);
        gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, targetPosition, moveSpeed * Vector3.Distance(targetPosition, gameObject.transform.position) * Time.deltaTime);
    }
}
