using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class PlayerController : MonoBehaviour
{
    private GameManager _gameManager;
    private bool _canDuck = true;

    private void Start()
    {
        _gameManager = FindFirstObjectByType<GameManager>();
    }

    void Update()
    {
        if (!_canDuck) { return; }
        
        InputSystem.onAnyButtonPress.CallOnce(keyPress => Duck(keyPress));
        _canDuck = false;
    }

    private void Duck(InputControl key)
    {
        char keyChar = key.ToString().ToLower()[key.ToString().Length - 1];
        Debug.Log(keyChar);
    }
}
