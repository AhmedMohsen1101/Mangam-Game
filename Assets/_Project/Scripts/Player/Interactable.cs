using System.Collections;
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

        if (enemy.hasBomb)
        {
            Debug.Log(collier.transform.parent.name);

            GameLogic.Instance.TransferBomb(playerController, enemy);
            StartCoroutine(CoolDown());
        }
      
    }
    /// <summary>
    /// To Prevent trigger after the bomb transfer 
    /// </summary>
    /// <returns></returns>
    private IEnumerator CoolDown()
    {
        canTrigger = false;
        yield return new WaitForSeconds(3);
        canTrigger = true;
    }

}
