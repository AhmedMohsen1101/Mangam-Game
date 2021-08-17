using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameLogic : MonoBehaviour
{
    public static GameLogic Instance;
    private List<PlayerController> players = new List<PlayerController>(); 
    private PlayerController currentCarrier;

    [SerializeField] private GameObject currentBomb;
    private void Awake()
    {
        if (Instance == null)
            Instance = this;

        players = FindObjectsOfType<PlayerController>().ToList();

       
        StartCoroutine(StartNewRound());
    }

    public void TransferBomb(PlayerController oldPlayer, PlayerController newCarrier)
    {
        if (oldPlayer != null)
            oldPlayer.ReleaseBomb();

        currentCarrier = newCarrier;

        currentCarrier.HoldBomb(currentBomb.transform);
        
        //Debug.Log(oldPlayer.gameObject.name + " " + newCarrier.gameObject.name + " " + currentBomb.name);
    }

    public void ExcludePlayer()
    {
        PlayerController player = currentCarrier;
        players.Remove(currentCarrier);
        player.Die();

        StopAllCoroutines();

        StartCoroutine(StartNewRound());
        
    }

    private IEnumerator StartNewRound()
    {
        if(players.Count == 1)
        {
            //Win Condition
            yield break;
        }
        yield return new WaitForSeconds(1);
        currentBomb = CreateBomb();
        int randomPlayerIndex = Random.Range(0, players.Count);
        TransferBomb(null ,players[randomPlayerIndex]);
        Debug.Log("New Round");
        yield break;
    }

    private GameObject CreateBomb()
    {
        return Instantiate(Resources.Load("Bomb")) as GameObject;
    }
}
