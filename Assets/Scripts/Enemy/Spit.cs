using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spit : MonoBehaviour
{
    private Vector2 targetPosition;
    private Vector3 direction;
    [SerializeField] float moveSpeed;
    float feta = 0;

    //Looks at the player
    private void Start()
    {
        feta = Mathf.Atan((targetPosition.y - transform.position.y) / (targetPosition.x - transform.position.x));

        feta = feta * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, feta);

        direction = (targetPosition - new Vector2(transform.position.x, transform.position.y)).normalized;
    }

    void Update()
    {
        transform.position = transform.position + (direction * moveSpeed * Time.deltaTime);
        
    }

    public void SetTarget(Vector2 targ)
    {
        targetPosition = targ;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.transform.tag == "Player")
        {
            collision.transform.GetComponent<PlayerMovement>().DamageInflicted(25, 700, 800, transform.position);
        }
        Destroy(gameObject);
    }

}
