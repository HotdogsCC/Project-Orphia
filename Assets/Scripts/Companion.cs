using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class Companion : MonoBehaviour
{
    [SerializeField] Camera cam;
    private Vector3 targetPosition;

    // Update is called once per frame
    void Update()
    {
        targetPosition = cam.ScreenToWorldPoint(Input.mousePosition);
        targetPosition = new Vector3(targetPosition.x, targetPosition.y, 0);
        gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, targetPosition, 0.1f);
    }
}
