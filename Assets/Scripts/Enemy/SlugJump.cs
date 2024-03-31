using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlugJump : MonoBehaviour
{
    [SerializeField] private Slug slug;

    private void OnTriggerStay2D(Collider2D collision)
    {
        slug.Jump();
    }
}
