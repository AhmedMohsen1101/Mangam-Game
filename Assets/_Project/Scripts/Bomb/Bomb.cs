using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    public float duration;
    public GameObject renderedObject;
    public ParticleSystem explosionEffect;
    public ParticleSystem explosionTextEffect;

    private bool isExplosed = false;
    private void Update()
    {
        duration -= Time.deltaTime;

        if (duration <= 0)
        {
            if (!isExplosed)
                Explode();
        }
    }

    private void Explode()
    {
        isExplosed = true;

        GameLogic.Instance.ExcludePlayer();

        if (renderedObject != null)
            renderedObject.SetActive(false);

        if (explosionEffect != null)
        {
            explosionEffect.gameObject.SetActive(true);
            explosionEffect.Play();
            CameraManager.Instance.ShakeCamera(01f);
            //Destroy(this.gameObject, explosionEffect.duration + 0.5f);
        }
        if (explosionTextEffect != null)
        {
            explosionTextEffect.gameObject.SetActive(true);
            explosionTextEffect.Play();
        }
    }


}
