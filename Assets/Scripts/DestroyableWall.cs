using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyableWall : MonoBehaviour
{
    [SerializeField] private GameObject particles;
    public void Destroyed()
    {
        Instantiate(particles, gameObject.transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
