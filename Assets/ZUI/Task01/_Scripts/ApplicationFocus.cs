using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApplicationFocus : MonoBehaviour
{
    [SerializeField] private RectTransform shareScreen;

    [SerializeField] private RectTransform smallScreen;

    [SerializeField] private RectTransform bigScreen;
    private void OnApplicationFocus(bool focus)
    {
        if (focus)
        {
            Debug.Log("Big");
            shareScreen.anchoredPosition = smallScreen.anchoredPosition;

            shareScreen.sizeDelta = new Vector2(bigScreen.rect.width, bigScreen.rect.height);

        }
        else
        {
            Debug.Log("Small");
            shareScreen.anchoredPosition = smallScreen.anchoredPosition;
            shareScreen.sizeDelta = new Vector2(smallScreen.rect.width, smallScreen.rect.height);
        }
    }
}
