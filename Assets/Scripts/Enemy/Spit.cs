using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spit : MonoBehaviour
{
    private Vector2 targetPosition;
    [SerializeField] float moveSpeed;
    void Update()
    {
        transform.position = Vector2.MoveTowards(transform.position, targetPosition, moveSpeed);
    }

    public void SetTarget(Vector2 targ)
    {
        targetPosition = targ;
    }

}
