using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class Companion : MonoBehaviour
{
    [SerializeField] Camera cam;
    [SerializeField] private PlayerInteraction player;
    [SerializeField] float moveSpeed = 1f;
    [SerializeField] float xOffSet = -2f;
    [SerializeField] float yOffSet = 2f;
    private Vector3 targetPosition;
    [SerializeField] private bool companionMouseFollow = false;
    private Rigidbody2D rb;
    private bool hasKey = false;
    private GameObject keyGO;
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        rb.WakeUp();
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            companionMouseFollow = !companionMouseFollow;
        }

        if (companionMouseFollow)
        {
            targetPosition = cam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10f));
            
        }
        else
        {
            targetPosition = new Vector3(player.transform.position.x + xOffSet, player.transform.position.y + yOffSet, player.transform.position.z);
        }

        targetPosition = new Vector3(targetPosition.x, targetPosition.y, 0);
        gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, targetPosition, moveSpeed * Vector3.Distance(targetPosition, gameObject.transform.position) * Time.deltaTime);

        if (player.hasKey)
        {
            hasKey = false;
        }

        if (hasKey)
        {
            keyGO.transform.position = transform.position;
        }
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        //stores that the key has been collected, destorys key
        if (collision.tag == "key")
        {
            hasKey = true;
            keyGO = collision.gameObject;
        }
        
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.tag == "wasp")
        {
            Enemy wasp = collision.transform.GetComponent<Enemy>();
            wasp.HitEnemy(5, 5, 5);
            wasp.GetComponent<Rigidbody2D>().gravityScale = 1;
        }
    }
}
