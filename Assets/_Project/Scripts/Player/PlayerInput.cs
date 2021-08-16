using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
public class PlayerInput : MonoBehaviour
{
    private VariableJoystick joystick;
    private PlayerController playerController;
    private CinemachineVirtualCamera cinemachineVirtual;

    private Vector3 movement;
    private void OnEnable()
    {
        if (playerController == null)
        {
            playerController = GetComponent<PlayerController>();
        }

        if(joystick == null)
        {
            joystick = GameObject.FindObjectOfType<VariableJoystick>();
        }

        cinemachineVirtual = GameObject.Find("Player Virtual Camera").GetComponent<CinemachineVirtualCamera>();

        if(cinemachineVirtual != null)
        {
            cinemachineVirtual.Follow = this.transform;
            cinemachineVirtual.LookAt = this.transform;
        }
    }

    private void FixedUpdate()
    {
        if (playerController == null || joystick == null)
            return;

        movement = Vector3.Lerp(movement, new Vector3(joystick.Horizontal, 0, joystick.Vertical), Time.fixedDeltaTime * 10);

        playerController.Move(movement);
    }

}
