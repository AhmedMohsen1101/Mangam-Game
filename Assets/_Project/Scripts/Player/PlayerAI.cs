using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum AIState
{
    Chase,
    Run, 
    RandomMove,
    Check,
    Stop,
}
public class PlayerAI : MonoBehaviour
{
    public AIState aIState;
    [SerializeField] private Transform worldCenter;
    [SerializeField] private float minEnemyCheckRate = 2f; //sec
    [SerializeField] private float maxEnemyCheckRate = 5f; //sec
    [SerializeField] private float minRadius = 2f; //sec
    [SerializeField] private float maxRadius = 7f; //sec
    [SerializeField] private float maxChasingTime = 30;
    [SerializeField] private LayerMask playablesLayerMask; 

    private float nextCheckTime;
    private float chasingTime;

    private PlayerController playerController;
    private PlayerController detectedEnemy;

    private Dictionary<PlayerController, float> nearEnemies = new Dictionary<PlayerController, float>();

    private bool isEnemyDetected;
    
    private void OnEnable()
    {
        if (playerController == null)
            playerController = GetComponent<PlayerController>();
    }


    private void Update()
    {
        if (isEnemyDetected)
        {
            Chase();
            return;
        }
        if (nextCheckTime <= Time.time)
        {
            float rate = Random.Range(minEnemyCheckRate, maxEnemyCheckRate);
            nextCheckTime = rate + Time.time;

            CheckEnemiesAround();

            if (Random.Range(0, 5) < 5)
                RandomMove();
        }

    }
    #region AI Logic
    private void CheckEnemiesAround()
    {
        float checkRadius = Random.Range(minRadius, maxRadius);
        Collider[] colliders = Physics.OverlapSphere(transform.position, checkRadius, playablesLayerMask);

        aIState = AIState.Check;

        foreach (var enemyCollider in colliders)
        {
            if (enemyCollider.transform.parent == this.gameObject)
                continue;

            PlayerController controller = enemyCollider.GetComponentInParent<PlayerController>();

            if (playerController.hasBomb)
            {
                isEnemyDetected = true;
                detectedEnemy = controller;
                break;
            }  

            if (controller.hasBomb)
            {
                RunAwayFromEnemy();
            }
            
        }
        
    }

    /// <summary>
    /// Chase any random target
    /// </summary>
    private void Chase()
    {
        chasingTime += Time.deltaTime;

        if(chasingTime >= maxChasingTime)
        {
            StopMoving();
            ResetEnemyInfo();

            return;
        }
        if (!playerController.hasBomb)
        {
            StopMoving();
            RunAwayFromEnemy();
            ResetEnemyInfo();
            return;
        }
        playerController.MoveToDestination(detectedEnemy.transform.position);

        aIState = AIState.Chase;
    }

    private void ResetEnemyInfo()
    {
        isEnemyDetected = false;
        detectedEnemy = null;
        chasingTime = 0;
    }
    #endregion

    #region AI Movement
    private void RandomMove()
    {
        if (RandomPoint(out Vector3 newPos))
            playerController.MoveToDestination(newPos);

        aIState = AIState.RandomMove;
    }

    private void RunAwayFromEnemy()
    {
        if (RandomPointAway(out Vector3 newPos))
            playerController.MoveToDestination(newPos);

        aIState = AIState.Run;
    }

    private void StopMoving()
    {
        playerController.StopMoving();

        aIState = AIState.Stop;

    }
    #endregion

    #region Navmesh Random destinations
    private bool RandomPoint(out Vector3 result)
    {
        for (int i = 0; i < 100; i++)
        {
            Vector3 randomPoint = worldCenter.position + Random.insideUnitSphere * 27;

            NavMeshHit hit;

            if(NavMesh.SamplePosition(randomPoint, out hit, 1, NavMesh.AllAreas))
            {
                result = hit.position;
                return true;
            }
        }

        result = worldCenter.position;
        return false;
    }

    private bool RandomPointAway(out Vector3 result)
    {
        for (int i = 0; i < 100; i++)
        {
            Vector3 randomPoint = worldCenter.position + Random.insideUnitSphere * 27;

            NavMeshHit hit;

            if (NavMesh.SamplePosition(randomPoint, out hit, 1, NavMesh.AllAreas))
            {
                result = hit.position;
                float distance = Vector3.Distance(hit.position, transform.position);
                
                if (distance >= 15)
                    return true;
            }
        }

        result = worldCenter.position;
        return false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(worldCenter.position, 27);
    }
    #endregion
}
