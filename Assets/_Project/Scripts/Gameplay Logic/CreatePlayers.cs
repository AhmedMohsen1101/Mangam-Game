using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatePlayers : MonoBehaviour
{
    private GameLogic gameLogic;
    [SerializeField] private ParticleSystem spawnEffect;
    [SerializeField] private AudioSource spawnSoundEffect;
    private IEnumerator Start()
    {
        if (gameLogic == null)
            gameLogic = GetComponent<GameLogic>();

        yield return new WaitForSeconds(3);

        for (int i = 0; i < gameLogic.players.Count; i++)
        {
            yield return new WaitForSeconds(0.7f);
            StartCoroutine(ActivatePlayer(gameLogic.players[i].gameObject));
        }

        int random = Random.Range(0, gameLogic.players.Count);

        for (int i = 0; i < gameLogic.players.Count; i++)
        {
            if (i != random)
                gameLogic.players[i].GetComponent<PlayerAI>().enabled = true;
        }
        gameLogic.enabled = true;
        yield return new WaitForSeconds(0.2f);
        gameLogic.players[random].GetComponent<PlayerInput>().enabled = true;
        
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
}
