using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WalkingEnemySpawnTrigger : MonoBehaviour
{
    private bool gotTriggered = false;
    [SerializeField] protected WalkingEnemyController enemyToSpawn;
    [SerializeField] protected Transform enemySpawnPoint;
    [SerializeField] protected Transform firstEnemyDestination;
    [SerializeField] protected EnemyPath enemyPath;

    private void OnTriggerEnter(Collider other)
    {
        if (!gotTriggered && other.GetComponent<Player>() != null)
        {
            SpawnEnemy();
        }
    }

    public void SpawnEnemy()
    {
        if (!gotTriggered)
        {
            gotTriggered = true;
            WalkingEnemyController temp = Instantiate(enemyToSpawn, enemySpawnPoint.position, enemySpawnPoint.rotation);
            temp.GetComponent<EnemyWalk>().CurrentPath = enemyPath;
            temp.GetComponent<EnemyWalk>().currentDestination = firstEnemyDestination.position;
            temp.GetComponent<NavMeshAgent>().SetDestination(firstEnemyDestination.position);
        }
    }
}
