using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class Mouse : MonoBehaviour
{
    public enum Row
    {
        Front,
        Back
    };
    
    [SerializeField] private Sprite _defaultSprite;
    [SerializeField] private Sprite _duckSprite;
    [SerializeField] private Row row;

    private GameManager _gameManager;
    private SpriteRenderer _spriteRenderer;
    private PlayerController _playerController;
    private Vector2 _startingPosition;
    private float _duckDistance;
    private Animator _anim;
    private int _animationColor;

    private bool _isDucking = false;
    private bool _mouseWasSlapped = false;

    void Start()
    {
        _gameManager = FindFirstObjectByType<GameManager>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _playerController = FindFirstObjectByType<PlayerController>();
        _startingPosition = transform.position;
        _duckDistance = _gameManager.GetMouseDuckDistance();
        
        PlayIdleAnimationFromRandomStart();
    }

    private void OnEnable()
    {
        Arm.MouseSlapped += MouseSlapped;
    }

    private void OnDisable()
    {
        Arm.MouseSlapped -= MouseSlapped;
    }

    private void PlayIdleAnimationFromRandomStart()
    {
        _anim = GetComponent<Animator>();
        List<string> animations = new List<string> {"MouseIdleBlack", "MouseIdleGrey", "MouseIdleTan"};
        float randomStart = Random.Range(0f, 1f);
        _animationColor = Random.Range(0, 3);
        
        _anim.Play(animations[_animationColor], -1, randomStart);
    }

    public void Duck()
    {
        if (_mouseWasSlapped) { return; }
        
        Debug.Log($"{name} ducked");
        transform.position += new Vector3(0f, _duckDistance, 0f);
        _isDucking = true;
        
        StartCoroutine(GetUp());
    }

    IEnumerator GetUp()
    {
        yield return new WaitForSeconds(_gameManager.GetDelayBeforeGettingUp() * 2);
        _isDucking = false;
        Debug.Log($"{name} got back up");
        _playerController.ToggleCanDuck();
        transform.position = _startingPosition;
    }

    public bool IsDucking()
    {
        return _isDucking;
    }

    public Row GetRow()
    {
        return row;
    }

    private void MouseSlapped()
    {
        _mouseWasSlapped = true;
    }

    public void StopAnimation()
    {
        _anim.enabled = false;
    }
}