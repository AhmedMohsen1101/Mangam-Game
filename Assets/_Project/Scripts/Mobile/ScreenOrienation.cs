using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenOrienation : MonoBehaviour
{
    public ScreenOrientation screenOrientation;
    void Start()
    {
        Screen.orientation = screenOrientation;
    }

}
