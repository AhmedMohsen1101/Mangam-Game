﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    public float duration;
    public MeshRenderer meshRenderer;
    public ParticleSystem explosionEffect;
    public ParticleSystem BombFuse;

    private bool isExplosed = false;
    private void Update()
    {
        duration -= Time.deltaTime;

        if(duration <= 0)
        {
            if (!isExplosed)
                Explode();
        }
    }

    private void Explode()
    {
        isExplosed = true;

        GameLogic.Instance.ExcludePlayer();

        if (meshRenderer != null)
            meshRenderer.enabled = false;

        if (BombFuse != null)
            BombFuse.Stop();

        if (explosionEffect != null)
        {
            explosionEffect.gameObject.SetActive(true);
            explosionEffect.Play();
            CameraManager.Instance.ShakeCamera(1f);
            //Destroy(this.gameObject, explosionEffect.duration + 0.5f);
        }
           
    }


}
