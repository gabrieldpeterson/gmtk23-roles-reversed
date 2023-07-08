using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arm : MonoBehaviour
{
    [SerializeField] private float movementDistance = 4.0f;
    [SerializeField] private float movementSpeed = 2.0f;
    [SerializeField] private float mouseDistanceTolerance = 0.1f;
    [SerializeField] private Color warningColor;
    [SerializeField] private Color slapColor;

    private Vector2 _startingPosition;
    private SpriteRenderer _spriteRenderer;
    private bool _canMove = true;
    private bool _lookingForMouse = false;
    private Arm[] _arms;
    private Color _startingColor;

    private GameManager _gameManager;

    private void OnEnable()
    {
        GameManager.PrepareSlap += LookForMice;
    }

    private void OnDisable()
    {
        GameManager.PrepareSlap -= LookForMice;
    }

    void Start()
    {
        _startingPosition = transform.position;
        _gameManager = FindFirstObjectByType<GameManager>();
        _arms = FindObjectsByType<Arm>(FindObjectsSortMode.None);
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _startingColor = _spriteRenderer.color;
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
                StartCoroutine(Slap(mouse));
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

    private void ResetBothArms()
    {
        foreach (Arm arm in _arms)
        {
            arm.EnableCanMove();
            arm.DisableLookingForMice();
            arm.ResetColor();
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

    public void EnableCanMove()
    {
        _canMove = true;
    }

    public void DisableLookingForMice()
    {
        _lookingForMouse = false;
    }

    public void ResetColor()
    {
        _spriteRenderer.color = _startingColor;
    }

    IEnumerator Slap(GameObject mouse)
    {
        _spriteRenderer.color = warningColor;
        yield return new WaitForSeconds(_gameManager.GetCurrentSlapDelay());
        
        // If successfully dodged
        if (mouse.GetComponent<Mouse>().IsDucking())
        {
            yield return new WaitForSeconds(_gameManager.GetDelayBeforeGettingUp());
            ResetBothArms();
            _gameManager.ResumeGame();
        }
        else
        {
            _spriteRenderer.color = slapColor;
        }

    }
}
