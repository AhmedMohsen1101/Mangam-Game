using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    private Coroutine coroutine;
    public static CameraManager Instance;

    private CameraController cameraController;

    public CameraController GetCameraController
    {
        get
        {
            if(cameraController == null)
            {
                cameraController = GameObject.FindObjectOfType<CameraController>();
            }
            return cameraController;
        }
    }
    private void Start()
    {
        Instance = this;
    }


    private IEnumerator CameraDelayShake(float sec)
    {
        GetCameraController.CameraTrigger(GetCameraController.Shake);
        yield return new WaitForSeconds(sec);
        GetCameraController.CameraTrigger(GetCameraController.Default);
    }

    [ContextMenu("Shake Camera")]
    public void ShakeCamera(float sec)
    {
        if(coroutine != null)
        {
            StopCoroutine(coroutine);
        }

        coroutine = StartCoroutine(CameraDelayShake(sec));
    }
}
