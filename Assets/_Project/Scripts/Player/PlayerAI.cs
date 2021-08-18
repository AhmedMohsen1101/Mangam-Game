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
    FindTarget,
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
        }
        if (playerController.hasBomb && !isEnemyDetected)
        {
            FindTarget(); 
        }

        if (nextCheckTime <= Time.time && !playerController.hasBomb)
        {
            float rate = Random.Range(minEnemyCheckRate, maxEnemyCheckRate);
            nextCheckTime = rate + Time.time;
           
            CheckEnemiesAround();

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
            PlayerController enemy = enemyCollider.GetComponentInParent<PlayerController>();

            if (enemy.gameObject == this.gameObject)
                continue;

            if (enemy.hasBomb)
            {
                if(!IsGoodDestination(enemy.transform))
                {
                    StopMoving();
                    RunAwayFrom(enemy.transform);
                    return;
                }
              
            }

            MoveRandomly();
        }
    }

    private void FindTarget()
    {
        float checkRadius = Random.Range(minRadius, maxRadius);
        Collider[] colliders = Physics.OverlapSphere(transform.position, checkRadius, playablesLayerMask);

        aIState = AIState.FindTarget;

        foreach (var enemyCollider in colliders)
        {
            PlayerController controller = enemyCollider.GetComponentInParent<PlayerController>();
            
            if (controller.gameObject == this.gameObject)
                continue;
          
            detectedEnemy = controller;
            isEnemyDetected = true;
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
            RunAwayFrom(detectedEnemy.transform);
            ResetEnemyInfo();
            return;
        }

        if (detectedEnemy == null)
        { 
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
    private void MoveRandomly()
    {
        if (RandomPoint(out Vector3 newPos))
            playerController.MoveToDestination(newPos);

        aIState = AIState.RandomMove;
    }

    private void RunAwayFrom(Transform enemy)
    {
        if (RandomPointAway(out Vector3 newPos, enemy))
            playerController.MoveToDestination(newPos);

        aIState = AIState.Run;
    }
   
    private void StopMoving()
    {
        playerController.StopAgentMoving();

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

    private bool IsGoodDestination(Transform enemy)
    {
        float distance = Vector3.Distance(transform.position, enemy.position);
        if (distance >= 5)
        {
            return true;
        }
        return false;
    }
    private bool RandomPointAway(out Vector3 result, Transform enemy)
    {
        for (int i = 0; i < 100; i++)
        {
            Vector3 randomPoint = worldCenter.position + Random.insideUnitSphere * 27;

            NavMeshHit hit;

            if (NavMesh.SamplePosition(randomPoint, out hit, 1, NavMesh.AllAreas))
            {
                result = hit.position;
                float distance = Vector3.Distance(hit.position, enemy.position);
                
                if (distance >= 8)
                {
                    Vector3 direction = enemy.position - transform.position;
                    float angle = Vector3.Angle(result, direction);

                    if(angle >= 60)
                        return true;
                }
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
