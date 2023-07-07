using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arm : MonoBehaviour
{
    [SerializeField] private float movementDistance = 4.0f;
    [SerializeField] private float movementSpeed = 2.0f;
    [SerializeField] private float mouseDistanceTolerance = 0.1f;

    private Vector2 _startingPosition;
    private bool _canMove = true;
    private bool _lookingForMouse = false;
    private Arm[] _arms;

    private GameManager _gameManager;

    private void OnEnable()
    {
        GameManager.prepareSlap += LookForMice;
    }

    private void OnDisable()
    {
        GameManager.prepareSlap -= LookForMice;
    }

    void Start()
    {
        _startingPosition = transform.position;
        _gameManager = FindFirstObjectByType<GameManager>();
        _arms = FindObjectsByType<Arm>(FindObjectsSortMode.None);
    }

    
    void Update()
    {
        if (!_canMove) { return; }
        MoveArm();
        
        if (!_lookingForMouse) { return; }
        FindNextMouse();
    }

    private void MoveArm()
    {
        const float tau = Mathf.PI * 2;
        float cycle = Time.time * movementSpeed;
        float rawSinWave = Mathf.Sin(cycle * tau);
        float offset = rawSinWave * movementDistance;
        transform.position = new Vector2(_startingPosition.x + offset, _startingPosition.y);
    }

    private void FindNextMouse()
    {
        List<GameObject> mice = _gameManager.GetMice();

        foreach (GameObject mouse in mice)
        {
            if (Vector2.Distance(new Vector2(transform.position.x, 0f), new Vector2(mouse.transform.position.x, 0f)) <= mouseDistanceTolerance)
            {
                Debug.Log($"{name} at {transform.position} is slapping {mouse.name} at {mouse.transform.position}");
                ToggleBothArms();
                break;
            }
        }
    }

    private void ToggleBothArms()
    {
        foreach (Arm arm in _arms)
        {
            arm.ToggleLookingForMice();
            arm.ToggleMovement();
        }
    }

    private void LookForMice()
    {
        _lookingForMouse = true;
    }

    public void ToggleLookingForMice()
    {
        _lookingForMouse = !_lookingForMouse;
    }

    public void ToggleMovement()
    {
        _canMove = !_canMove;
    }
}
