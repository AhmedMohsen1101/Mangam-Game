using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    public float duration;
    public GameObject renderedObject;
    public ParticleSystem explosionEffect;
    public ParticleSystem explosionTextEffect;
    public AudioSource countDownSound;
    private bool isExplosed = false;

    private BombGUI bombUI;

    private void OnEnable()
    {
        if (bombUI == null)
            bombUI = GameObject.FindObjectOfType<BombGUI>();

        bombUI.SetMaxDuration(duration);
    }
    private void Update()
    {
        duration -= Time.deltaTime;
        
        bombUI.UpdateSliderValue(duration);

        if(duration <= 6)
        {
            if (countDownSound != null)
                if (!countDownSound.isPlaying)
                    countDownSound.Play();
        }

        if (duration <= 0)
        {
            countDownSound.Stop();
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
