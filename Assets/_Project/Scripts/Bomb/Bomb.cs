using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    public float duration;
   
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
        GameLogic.Instance.ExcludePlayer();

        Destroy(gameObject, 0.1f);
    }


}
