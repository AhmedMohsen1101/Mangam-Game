using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraStateMachine : StateMachineBehaviour
{
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        CameraManager.Instance.GetCameraController.GetAnimator.ResetTrigger(CameraManager.Instance.GetCameraController.Default);
        CameraManager.Instance.GetCameraController.GetAnimator.ResetTrigger(CameraManager.Instance.GetCameraController.Shake);
    }
}
