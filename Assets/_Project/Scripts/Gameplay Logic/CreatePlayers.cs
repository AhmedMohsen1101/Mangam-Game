using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatePlayers : MonoBehaviour
{
    private GameLogic gameLogic;
    [SerializeField] private ParticleSystem spawnEffect;
    [SerializeField] private AudioSource spawnSoundEffect;

    /// <summary>
    /// To Intial the players in the scene with cool effects
    /// </summary>
    /// <returns></returns>
    private void Start()
    {
        if (gameLogic == null)
            gameLogic = GetComponent<GameLogic>();
        FadingUI.Instance.FadeOut();

        StartCoroutine(RountineSpawnPlayers());
    }


    private IEnumerator RountineSpawnPlayers()
    {
        yield return new WaitForSeconds(5);

        for (int i = 0; i < gameLogic.players.Count; i++)
        {
            yield return new WaitForSeconds(0.7f);
            StartCoroutine(ActivatePlayer(gameLogic.players[i].gameObject));
        }

        StartCoroutine (GetMainPlayerRandomly());
    }
        
    private IEnumerator ActivatePlayer(GameObject obj)
    {
        Vector3 effectPos = obj.transform.position;
        effectPos.y = 0.3f;
        spawnEffect.transform.position = effectPos;

        spawnEffect.Play();
        spawnSoundEffect.Play();
        yield return new WaitForSeconds(0.3f);
        obj.SetActive(true);
        CameraManager.Instance.ShakeCamera(0.25f);
    }


    private IEnumerator GetMainPlayerRandomly()
    {
        int playerIndex = Random.Range(0, gameLogic.players.Count);

        for (int i = 0; i < gameLogic.players.Count; i++)
        {
            if (i != playerIndex)
                gameLogic.players[i].GetComponent<PlayerAI>().enabled = true;
        }
        gameLogic.enabled = true;
        yield return new WaitForSeconds(0.2f);
        
        gameLogic.SetMainPlayer(playerIndex);
        
        GetComponent<GameLogicUI>().enabled = true;
    }
}
