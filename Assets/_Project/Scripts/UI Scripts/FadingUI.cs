using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadingUI : MonoBehaviour
{
    [SerializeField] private CanvasGroup fadingCanvas;

    public static FadingUI Instance;

    private void Awake()
    {
        Instance = this;
    }
    public void FadeIn()
    {
        StartCoroutine(FadeInRoutine());
    }
    public void FadeOut()
    {
        LeanTween.alphaCanvas(fadingCanvas, 0, 2);
    }
    private IEnumerator FadeInRoutine()
    {
        LeanTween.alphaCanvas(fadingCanvas, 1, 2);

        yield return new WaitForSeconds(2);

        SceneLoader.Instance.LoadScene(1);

    }
}
