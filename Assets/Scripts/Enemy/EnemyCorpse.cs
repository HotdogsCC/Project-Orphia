using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCorpse : MonoBehaviour
{ 
    private Enemy.EnemyType enemyType;
    [SerializeField] private int health = 0;
    [SerializeField] private GameObject corpseConsumeParts;

    //Assigns health value to the corpse from the enemy type
    public void SetEnemyType(Enemy.EnemyType type)
    {
        enemyType = type;
        switch (enemyType)
        {
            case Enemy.EnemyType.Small:
                health = 15;
                break;
            case Enemy.EnemyType.Large:
                health = 25;
                break;
            case Enemy.EnemyType.Boss:
                health = 100;
                break;
            default:
                Debug.LogError("invalid enemy type");
                break;
        }
    }

    // Runs when the corpse is interacted with
    public int Consumed()
    {
        Instantiate(corpseConsumeParts, gameObject.transform.position, Quaternion.identity);
        Destroy(gameObject);
        return health;
    }
}
