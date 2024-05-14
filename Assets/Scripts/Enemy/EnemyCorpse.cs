using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCorpse : MonoBehaviour
{ 
    private Enemy.EnemyType enemyType;
    private int health = 0;
    [SerializeField] private GameObject corpseConsumeParts;

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


    public int Consumed()
    {
        Instantiate(corpseConsumeParts, gameObject.transform.position, Quaternion.identity);
        Destroy(gameObject);
        return health;
    }
}
