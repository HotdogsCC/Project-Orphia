using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : MonoBehaviour
{
    public void PickedUp()
    {
        Destroy(gameObject);
    }
}
