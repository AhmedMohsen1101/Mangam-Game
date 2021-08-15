using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum Type
{
    Player,
    Enemy,
}
[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Animator))]
public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5;
    public float turnSpeed = 15;

    private NavMeshAgent agent;
    private Animator animator;
    private void OnEnable()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }

    public void Move(Vector3 movement)
    {
        agent.Move(movement.normalized * Time.fixedDeltaTime * moveSpeed);
        animator.SetFloat("Movement", movement.magnitude);

        if(movement.magnitude > 0.01f)
        {
            Quaternion smoothRotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(movement), Time.fixedDeltaTime * turnSpeed);
            transform.rotation = smoothRotation;
        }
    }
}
