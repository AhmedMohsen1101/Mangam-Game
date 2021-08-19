using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLogicUI : MonoBehaviour
{
    [SerializeField] private TMPro.TMP_Text roundsDisplayText;
    [SerializeField] private TMPro.TMP_Text displayText;

    private void OnEnable()
    {
        GameLogic.Instance.OnStartRound += OnStartRound;
        GameLogic.Instance.OnVictory += OnVictory;
        GameLogic.Instance.OnLose += OnLose;
    }
    private void OnDisable()
    {
        GameLogic.Instance.OnStartRound -= OnStartRound;
        GameLogic.Instance.OnVictory -= OnVictory;
        GameLogic.Instance.OnLose -= OnLose;
    }

    private void OnStartRound()
    {
        roundsDisplayText.enabled = true;
        roundsDisplayText.text = "New Round";

        LeanTween.scale(roundsDisplayText.gameObject, Vector3.one * 2f, 2).setEasePunch();
        StartCoroutine(DisableText());
    }

    [ContextMenu("OnLose")]
    private void OnVictory()
    {
        displayText.enabled = true;
        displayText.text = "Victory!";

        LeanTween.scale(displayText.gameObject, Vector3.one * 2f, 3).setEasePunch();
    }

    [ContextMenu("OnLose")]
    private void OnLose()
    {
        displayText.enabled = true;
        displayText.text = "Game Over!!";

        LeanTween.scale(displayText.gameObject, Vector3.one * 2f, 3).setEasePunch();
    }

   private IEnumerator DisableText()
    {
        yield return new WaitForSeconds(2);

        roundsDisplayText.enabled = false;
    }
}
