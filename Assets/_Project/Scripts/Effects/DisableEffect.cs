using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableEffect : MonoBehaviour
{
    private ParticleSystem effect;
    
    private IEnumerator Start()
    {
        if (effect == null)
            effect = GetComponent<ParticleSystem>();

        yield return new WaitForSeconds(effect.duration);
        
        effect.gameObject.SetActive(false);
    }

   
}
