using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.ParticleSystem;

public class KeyWall : MonoBehaviour
{
    public void Unlocked()
    {
        Destroy(gameObject);
    }
}
