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

        LeanTween.scale(displayText.gameObject, Vector3.one * 1.5f, 1).setEasePunch();
        StartCoroutine(DisableText());
    }

    private void OnVictory()
    {
        displayText.enabled = true;
        displayText.text = "Victory";

        LeanTween.scale(displayText.gameObject, Vector3.one * 1.5f, 1).setEasePunch();
    }

    private void OnLose()
    {
        displayText.enabled = true;
        displayText.text = "Game Over";

        LeanTween.scale(displayText.gameObject, Vector3.one * 1.5f, 1).setEasePunch();
    }

   private IEnumerator DisableText()
    {
        yield return new WaitForSeconds(2);

        roundsDisplayText.enabled = false;
    }
}
