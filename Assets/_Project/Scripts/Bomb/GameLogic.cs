using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameLogic : Singleton<GameLogic>
{
    private List<PlayerController> players = new List<PlayerController>(); 
    private PlayerController currentCarrier;

    private GameObject currentBomb;
    private void OnEnable()
    {
        players = FindObjectsOfType<PlayerController>().ToList();

        Debug.Log("Players Count " + players.Count);

        StartCoroutine(StartNewRound());
    }

    public void TransferBomb(PlayerController oldPlayer, PlayerController newCarrier)
    {
        if (oldPlayer != null)
            oldPlayer.ReleaseBomb();

        currentCarrier = newCarrier;
        currentCarrier.HoldBomb(currentBomb.transform);
    }

    public void ExcludePlayer()
    {
        if (players.Contains(currentCarrier))
        {
            currentCarrier.Die();
            players.Remove(currentCarrier);

            StartCoroutine(StartNewRound());
        }
    }


    private IEnumerator StartNewRound()
    {
        yield return new WaitForSeconds(5);

        currentBomb = CreateBomb();
        int randomPlayerIndex = Random.Range(0, players.Count);
        TransferBomb(null ,players[randomPlayerIndex]);
        Debug.Log("New Round");
    }

    private GameObject CreateBomb()
    {
        return Instantiate(Resources.Load("Bomb")) as GameObject;
    }
}
