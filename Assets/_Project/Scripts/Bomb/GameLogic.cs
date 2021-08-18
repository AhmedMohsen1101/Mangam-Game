using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameLogic : MonoBehaviour
{
    public static GameLogic Instance;
    
    private GameObject currentBomb; //Bomb in the scene 
    private PlayerController currentCarrier; 
    
    private List<PlayerController> players = new List<PlayerController>(); 

    private void Awake()
    {
        if (Instance == null)
            Instance = this;

        players = FindObjectsOfType<PlayerController>().ToList();

        StartCoroutine(StartNewRound());
    }

    /// <summary>
    /// Transfer from player to player
    /// </summary>
    /// <param name="oldCarrier"></param>
    /// <param name="newCarrier"></param>
    public void TransferBomb(PlayerController oldCarrier, PlayerController newCarrier)
    {
        if (oldCarrier != null)
            oldCarrier.ReleaseBomb();

        currentCarrier = newCarrier;

        currentCarrier.HoldBomb(currentBomb.transform);
    }

    /// <summary>
    /// 
    /// </summary>
    public void ExcludePlayer()
    {
        PlayerController player = currentCarrier;
        players.Remove(currentCarrier);
        player.Die();

        StopAllCoroutines();

        StartCoroutine(StartNewRound());
        
    }

    /// <summary>
    /// Start new Round by picking random player to carry the bomb
    /// </summary>
    /// <returns></returns>
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
