using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private float yOffset;
    private Vector3 offset = new Vector3(0f, 0f, -10f);
    private Vector3 velocity = Vector3.zero;

    // Update is called once per frame
    void Update()
    {
        offset = new Vector3(0f, yOffset, -10f);
        Vector3 targetPosition = player.position + offset;
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, 0.25f);
    }
}
