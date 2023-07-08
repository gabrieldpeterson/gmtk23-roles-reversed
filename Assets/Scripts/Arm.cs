using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arm : MonoBehaviour
{
    [SerializeField] private float movementDistance = 4.0f;
    // [SerializeField] private float movementSpeed = 2.0f;
    [SerializeField] private float mouseDistanceTolerance = 0.1f;
    [SerializeField] private Color warningColor;
    [SerializeField] private Color slapColor;

    [SerializeField] private Sprite uprightArm;
    [SerializeField] private Sprite uprightArmWarning;
    [SerializeField] private Sprite shortArmSlap;
    [SerializeField] private Sprite longArmSlap;
    [SerializeField] private float yOffsetShortArmSlap = -2.0f;
    [SerializeField] private float yOffsetLongArmSlap = -3.0f;

    private Vector2 _startingPosition;
    private SpriteRenderer _spriteRenderer;
    private bool _canMove = true;
    private bool _lookingForMouse = false;
    private Arm[] _arms;
    private Color _startingColor;
    private float _armStartingYPosition;

    private GameManager _gameManager;
    private AudioController _audioController;

    public static Action MouseSlapped;

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
        _armStartingYPosition = transform.position.y;
        _audioController = FindFirstObjectByType<AudioController>();
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
        float cycle = Time.time * _gameManager.GetCurrentArmSpeed();
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
        _spriteRenderer.sprite = uprightArm;
        _spriteRenderer.sortingLayerName = "Cat Arms Moving";
        transform.position = new Vector3(transform.position.x, _armStartingYPosition, transform.position.z);
        
        foreach (Arm arm in _arms)
        {
            arm.EnableCanMove();
            arm.DisableLookingForMice();
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
        //_spriteRenderer.color = _startingColor;
        _spriteRenderer.sprite = uprightArm;
    }

    IEnumerator Slap(GameObject mouse)
    {
        //_spriteRenderer.color = warningColor;
        _spriteRenderer.sprite = uprightArmWarning;
        
        yield return new WaitForSeconds(_gameManager.GetCurrentSlapDelay());
        
        UpdateArmSpriteWhileSlapping(mouse);
        
        _audioController.PlayAudioClip(_audioController.whack, 1f);

        // If successfully dodged
        if (mouse.GetComponent<Mouse>().IsDucking())
        {
            _gameManager.UpdateScore();
            yield return new WaitForSeconds(_gameManager.GetDelayBeforeGettingUp());
            ResetBothArms();
            _gameManager.ResumeGame();
        }
        else
        {
            _spriteRenderer.color = slapColor;
            MouseSlapped?.Invoke();
            mouse.GetComponent<Mouse>().StopAnimation();
        }
    }

    private void UpdateArmSpriteWhileSlapping(GameObject mouse)
    {
        Mouse mouseScript = mouse.GetComponent<Mouse>();
        _spriteRenderer.sortingLayerName = "Cat Arms Slapping";
        ResetColor();

        if (mouseScript.GetRow() == Mouse.Row.Front)
        {
            _spriteRenderer.sprite = longArmSlap;
            transform.position += new Vector3(0f, yOffsetLongArmSlap, 0f);
        }
        else if (mouseScript.GetRow() == Mouse.Row.Back)
        {
            _spriteRenderer.sprite = shortArmSlap;
            transform.position += new Vector3(0f, yOffsetShortArmSlap, 0f);
        }
        else
        {
            Debug.LogWarning("Arm sprite error");
            _spriteRenderer.sprite = uprightArm;
        }
    }
}
