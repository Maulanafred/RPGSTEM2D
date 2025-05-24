using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlModeManager : MonoBehaviour
{
    public static ControlModeManager instance;

    public bool isScopeMode = false;

    void Awake()
    {
        if (instance == null) instance = this;
    }

    public void SetScopeMode(bool state)
    {
        isScopeMode = state;
    }
}

