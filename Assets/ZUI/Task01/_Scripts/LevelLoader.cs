using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

using UnityEngine.UI;
public class LevelLoader : MonoBehaviour
{
    public Slider loadingBar;

    public void LoadLevel(string sceneName)
    {
        StartCoroutine(Loading(sceneName));
    }
    IEnumerator Loading(string sceneName)
    {
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneName);

        while (!asyncOperation.isDone)
        {
            float progress = Mathf.Clamp01(asyncOperation.progress / 0.9f);
            loadingBar.value = progress;
            yield return null;
        }
    }
}
