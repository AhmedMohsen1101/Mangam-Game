using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Animator))]

public class PlayerController : MonoBehaviour
{
    public bool hasBomb;
    public float moveSpeed = 5;
    public float turnSpeed = 15;

    [SerializeField] private Stun stun;
    [SerializeField] private Death death;

    protected NavMeshAgent agent;
    protected Animator animator;
    protected Transform bombTransfrom;
    
    protected void OnEnable()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        bombTransfrom = transform.Find("BombPosition");
       
        //agent.speed = moveSpeed;
        //agent.angularSpeed = turnSpeed;

    }
    private void Update()
    {
        if (agent.hasPath)
            animator.SetFloat("Movement", agent.velocity.magnitude);
    }
    /// <summary>
    /// move to a certain direction can be user be the player inputs and AI
    /// </summary>
    /// <param name="movement"></param>
    public void Move(Vector3 movement)
    {
        if (stun.isStunned || death.isDead)
        {
            StopMoving();
            return;
        }

        if (movement.magnitude <= 0.001f)
            return;

        agent.Move(movement * Time.fixedDeltaTime * moveSpeed);
        animator.SetFloat("Movement", movement.magnitude);

        if(movement.magnitude > 0.01f)
        {
            Quaternion smoothRotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(movement), Time.fixedDeltaTime * turnSpeed);
            transform.rotation = smoothRotation;
        }
    }

    /// <summary>
    /// Move to a certain position ex:(chasing the player)
    /// </summary>
    /// <param name="target"></param>
    public void MoveToDestination(Vector3 target)
    {
        if (stun.isStunned || death.isDead)
        {
            StopMoving();
            return;
        }

        animator.SetFloat("Movement", agent.velocity.magnitude);

        if(!agent.hasPath)
        {
            agent.SetDestination(target);
        }
        
    }

    public void StopMoving()
    {
        agent.ResetPath();
        animator.SetFloat("Movement", 0);
    }


    /// <summary>
    /// Become the carrier of the bomb
    /// </summary>
    /// <param name="bomb"></param>
    public void HoldBomb(Transform bomb)
    {
        bomb.SetParent(null);
        bomb.SetParent(bombTransfrom);
        bomb.localPosition = Vector3.zero;
        
        hasBomb = true;

        StartCoroutine(Stun());
    }

    /// <summary>
    /// when transfer the bomb to another player
    /// </summary>
    public void ReleaseBomb()
    {
        hasBomb = false;
    }

    /// <summary>
    /// A stun for a while prevent the player from moving 
    /// </summary>
    /// <returns></returns>
    private IEnumerator Stun()
    {
        if (agent.hasPath)
            StopMoving();

        stun.isStunned = true;
        animator.SetBool("Dizzy", true);

        stun.PlayVisualEffect();
        stun.PlaySoundEffect();
        yield return new WaitForSeconds(stun.stunDuration);
        stun.StopVisualEffect();
       
        stun.isStunned = false;
        animator.SetBool("Dizzy", false);

    }

    /// <summary>
    /// 
    /// </summary>
    public void Die()
    {
        StartCoroutine(RoutineDie());
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    private IEnumerator RoutineDie()
    {
        if (agent.hasPath)
            StopMoving();

        death.isDead = true;
        animator.SetTrigger("Die");

        yield return new WaitForSeconds(0.4f);
        death.PlayEffect();
        yield return new WaitForSeconds(2.1f);
        Destroy(gameObject);
    }
}

[System.Serializable]
public class Stun
{
    public ParticleSystem stunEffect;
    public AudioSource stunSoundEffect;
    public float stunDuration;
    public bool isStunned;

    public void PlayVisualEffect()
    {
        if (this.stunEffect != null)
            this.stunEffect.Play();

    }
    public void PlaySoundEffect()
    {
        if (stunSoundEffect != null)
            stunSoundEffect.Play();
    }
    public void StopVisualEffect()
    {
        if (this.stunEffect != null)
            this.stunEffect.Stop();
    }
}
[System.Serializable]
public class Death
{
    public ParticleSystem[] deathEffects;
    public bool isDead;

    public void PlayEffect()
    {
        if(deathEffects.Length > 0)
        {
            ParticleSystem particleSystem = deathEffects[Random.Range(0, deathEffects.Length)];
            particleSystem.gameObject.SetActive(true);
            LeanTween.moveLocalY(particleSystem.gameObject, 5f, 2);
            particleSystem.Play(); 
        }
    }
}
