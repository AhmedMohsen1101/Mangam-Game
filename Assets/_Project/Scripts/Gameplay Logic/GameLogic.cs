using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameLogic : MonoBehaviour
{
    public static GameLogic Instance;

    public List<PlayerController> players = new List<PlayerController>();

    private GameObject currentBomb; //Bomb in the scene 
    private PlayerController currentCarrier;

    #region Delegations
    public UnityAction OnStartRound;
    public UnityAction OnVictory;
    #endregion

    private void OnEnable()
    {
        if (Instance == null)
            Instance = this;

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
            OnVictory?.Invoke();
            yield break;
        }
        yield return new WaitForSeconds(1);
        currentBomb = CreateBomb();
        int randomPlayerIndex = Random.Range(0, players.Count);
        TransferBomb(null, players[randomPlayerIndex]);
        Debug.Log("New Round");
        yield break;
    }

    private GameObject CreateBomb()
    {
        return Instantiate(Resources.Load("Bomb")) as GameObject;
    }
}
