﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    private PlayerController playerController;

    private bool canTrigger = true;
    private void OnEnable()
    {
        if (playerController == null)
            playerController = GetComponent<PlayerController>();

        canTrigger = true;
    }
    private void OnTriggerEnter(Collider collier)
    {
        if (!canTrigger)
            return;

        PlayerController enemy = collier.GetComponentInParent<PlayerController>();

        if (playerController.hasBomb)
        {
            Debug.Log(collier.transform.parent.name);

            StartCoroutine(CatchPlayer(enemy));
        }
      
    }
    /// <summary>
    /// To Prevent trigger after the bomb transfer 
    /// </summary>
    /// <returns></returns>
    private IEnumerator CatchPlayer(PlayerController enemy)
    {
        canTrigger = false;
        
        yield return new WaitForEndOfFrame();
        GameLogic.Instance.TransferBomb(playerController, enemy);
        
        yield return new WaitForSeconds(3);
        canTrigger = true;
    }

    

}
