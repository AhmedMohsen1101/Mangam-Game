using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [HideInInspector] public int Default = Animator.StringToHash("Default");
    [HideInInspector] public int Shake = Animator.StringToHash("Shake");

    private Animator animator;
    
    public Animator GetAnimator
    {
        get
        {
            if(animator == null)
            {
                animator = GetComponent<Animator>();
            }
            return animator;
        }
    }


    public void CameraTrigger(int trigger)
    {
        GetAnimator.SetTrigger(trigger);
    }
}
