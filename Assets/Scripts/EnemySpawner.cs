using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    int minWaitTime = 0;
    int maxWaitTime = 15;

    [SerializeField] GameObject slug;
    [SerializeField] GameObject wasp;

    [SerializeField] Transform bottomLeft;
    [SerializeField] Transform topRight;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(WaitingAndThenSpawning());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator WaitingAndThenSpawning()
    {
        yield return new WaitForSeconds(Random.Range(minWaitTime, maxWaitTime));
        Debug.Log("spawned!");
        int coinFlip = Random.Range(0, 2);
        if(coinFlip == 0)
        {
            Instantiate(slug, new Vector3(Random.Range(bottomLeft.position.x, topRight.position.x), Random.Range(bottomLeft.position.y, topRight.position.y), 0), Quaternion.identity);
        }
        else
        {
            Instantiate(wasp, new Vector3(Random.Range(bottomLeft.position.x, topRight.position.x), Random.Range(bottomLeft.position.y, topRight.position.y), 0), Quaternion.identity);
        }
        StartCoroutine(WaitingAndThenSpawning());
    }
}
