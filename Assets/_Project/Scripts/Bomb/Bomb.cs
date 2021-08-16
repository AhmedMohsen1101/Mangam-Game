using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    public LayerMask CarrierLayerMask;
    public float duration;
   
    private readonly float radius = 1;
    private readonly float maxDistance = 0.5f;

    private PlayerController playerController;

    private void Start()
    {
        duration = 30;
    }
    private void Update()
    {
        duration -= Time.deltaTime;

        if(duration <= 0)
        {
            Explode();
        }
    }

    private void Explode()
    {
        playerController.Die();
    }


}
