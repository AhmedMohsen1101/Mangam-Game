using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    private VariableJoystick joystick;
    private PlayerController playerController;

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
    }

    private void FixedUpdate()
    {
        if (playerController == null || joystick == null)
            return;

        playerController.Move(new Vector3(joystick.Horizontal, 0, joystick.Vertical));
    }

}
