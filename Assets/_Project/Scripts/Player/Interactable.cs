using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    private PlayerController playerController;

    private void OnEnable()
    {
        if (playerController == null)
            playerController = GetComponent<PlayerController>();
    }
    private void OnTriggerEnter(Collider collier)
    {
        PlayerController enemy = collier.GetComponentInParent<PlayerController>();

        if (enemy.hasBomb)
        {
            Debug.Log(collier.transform.parent.name);

            GameLogic.Instance.TransferBomb(playerController, enemy);
        }
      
    }
}
