using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameLogic : MonoBehaviour
{
    public static GameLogic Instance;
    public List<PlayerController> players = new List<PlayerController>();

    [SerializeField] private ParticleSystem winEffect;
    
    private GameObject currentBomb; //Bomb in the scene 
    private PlayerController currentCarrier;
    public PlayerController mainPlayer { get; set; }


    #region Delegations
    public UnityAction OnStartRound;
    public UnityAction OnVictory;
    public UnityAction OnLose;
    #endregion

    private void OnEnable()
    {
        if (Instance == null)
            Instance = this;
        
        StopAllCoroutines();
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
        {
            oldCarrier.ReleaseBomb();
            CameraManager.Instance.ShakeCamera(0.25f);
        }

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

    public void SetMainPlayer(int playerIndex)
    {
        mainPlayer = players[playerIndex];
        mainPlayer.GetComponent<PlayerInput>().enabled = true;
    }
    /// <summary>
    /// Start new Round by picking random player to carry the bomb
    /// </summary>
    /// <returns></returns>
    private IEnumerator StartNewRound()
    {
        if(mainPlayer != null && mainPlayer.death.isDead)
        {
            OnLose?.Invoke();
            LoseCondition();
            yield break;
        }
        if(players.Count == 1)
        {
            OnVictory?.Invoke();
            WinCondition();
            yield break;
        }
        yield return new WaitForSeconds(1);

        currentBomb = CreateBomb();
        int randomPlayerIndex = Random.Range(0, players.Count);
        TransferBomb(null, players[randomPlayerIndex]);
        OnStartRound?.Invoke();
    }

    private GameObject CreateBomb()
    {
        return Instantiate(Resources.Load("Bomb")) as GameObject;
    }

    private void LoseCondition()
    {
        for (int i = 0; i < players.Count; i++)
        {
            players[i].StopAgentMoving();
            players[i].GetComponent<PlayerAI>();
        }
    }

    private void WinCondition()
    {
        mainPlayer.GetComponent<PlayerInput>().enabled = false;

        if (winEffect != null)
        {
            winEffect.transform.SetParent(mainPlayer.transform);
            winEffect.transform.localScale = new Vector3(0, 1, 0);
            winEffect.Play();
        }
    }
}
