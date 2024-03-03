using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Despawner : MonoBehaviour
{
    [SerializeField] private float timeAlive = 1f;

    private void Start()
    {
        StartCoroutine(Despawn());
    }

    IEnumerator Despawn()
    {
        yield return new WaitForSeconds(timeAlive);
        Destroy(gameObject);
    }
}
