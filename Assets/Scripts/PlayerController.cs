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
    private List<GameObject> _mice;

    private void Start()
    {
        _gameManager = FindFirstObjectByType<GameManager>();
        _mice = _gameManager.GetMice();
    }

    void Update()
    {
        if (!_canDuck) { return; }
        
        InputSystem.onAnyButtonPress.CallOnce(keyPress => Duck(keyPress));
        _canDuck = false;
    }

    public void ToggleCanDuck()
    {
        _canDuck = !_canDuck;
    }

    private void Duck(InputControl key)
    {
        char keyChar = key.ToString().ToLower()[key.ToString().Length - 1];
        Debug.Log(keyChar);
        switch (keyChar)
        {
            case 's':
                _mice[0].GetComponent<Mouse>().Duck();
                break;
            case 'd':
                _mice[1].GetComponent<Mouse>().Duck();
                break;
            case 'f':
                _mice[2].GetComponent<Mouse>().Duck();
                break;
            case 'j':
                _mice[3].GetComponent<Mouse>().Duck();
                break;
            case 'k':
                _mice[4].GetComponent<Mouse>().Duck();
                break;
            case 'l':
                _mice[5].GetComponent<Mouse>().Duck();
                break;
            default:
                _canDuck = true;
                break;

            
        }
    }
}
