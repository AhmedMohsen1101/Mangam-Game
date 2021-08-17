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
    }
    
    public void Move(Vector3 movement)
    {
        if (stun.isStunned)
            return;
        
        if (death.isDead)
            return;

        agent.Move(movement * Time.fixedDeltaTime * moveSpeed);
        animator.SetFloat("Movement", movement.magnitude);

        if(movement.magnitude > 0.01f)
        {
            Quaternion smoothRotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(movement), Time.fixedDeltaTime * turnSpeed);
            transform.rotation = smoothRotation;
        }
    }

    
    [ContextMenu("HoldBomb")]
    public void HoldBomb(Transform bomb)
    {
        bomb.SetParent(null);
        bomb.SetParent(bombTransfrom);
        bomb.localPosition = Vector3.zero;
        hasBomb = true;
        Debug.Log(this.gameObject.name + " Hold Bomb");
        StartCoroutine(Stun());
    }
    public void ReleaseBomb()
    {
        hasBomb = false;
    }
    private IEnumerator Stun()
    {
        stun.isStunned = true;
        animator.SetBool("Dizzy", true);

        stun.PlayEffect();
        yield return new WaitForSeconds(stun.stunDuration);
       
        stun.isStunned = false;
        animator.SetBool("Dizzy", false);

        stun.StopEffect();
    }
    public void Die()
    {
        StartCoroutine(RoutineDie());
    }
    private IEnumerator RoutineDie()
    {
        death.isDead = true;
        animator.SetTrigger("Die");

        yield return new WaitForSeconds(0.2f);
        death.PlayEffect();
        yield return new WaitForSeconds(2.3f);
        Destroy(gameObject);
    }
}

[System.Serializable]
public class Stun
{
    public ParticleSystem stunEffect;
    public float stunDuration;
    public bool isStunned;

    public void PlayEffect()
    {
        if (this.stunEffect != null)
            this.stunEffect.Play();
    }
    public void StopEffect()
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
