using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaspSpit : MonoBehaviour
{
    private PlayerMovement player;
    [SerializeField] private float spitWaitTime = 2;
    [SerializeField] GameObject spit;
    private Wasp wasp;
    private bool canSpit = true;

    private void Start()
    {
        player = FindObjectOfType<PlayerMovement>();
        wasp = GetComponentInParent<Wasp>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "PlayerHitbox")
        {
            wasp.agro = true;
        }
        
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "PlayerHitbox")
        {
            wasp.agro = false;
        }
    }

    private void Update()
    {
        TryToSpit();
    }

    private void TryToSpit()
    {
        if(canSpit && wasp.agro && GetComponentInParent<Enemy>().isStunned == false && GetComponentInParent<Enemy>().isTailSucking == false)
        {
            canSpit = false;
            StartCoroutine(WaitAndThen(spitWaitTime));
            GameObject spawnedSpit = Instantiate(spit, transform.position, Quaternion.identity);
            spawnedSpit.GetComponent<Spit>().SetTarget(player.transform.position);
        }  
    }

    private IEnumerator WaitAndThen(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        canSpit = true;
    }
}
