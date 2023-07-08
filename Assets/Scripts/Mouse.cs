using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Mouse : MonoBehaviour
{
    [SerializeField] private Sprite _defaultSprite;
    [SerializeField] private Sprite _duckSprite;

    private GameManager _gameManager;
    private SpriteRenderer _spriteRenderer;
    private PlayerController _playerController;

    private bool _isDucking = false;

    void Start()
    {
        _gameManager = FindFirstObjectByType<GameManager>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _playerController = FindFirstObjectByType<PlayerController>();
    }

    public void Duck()
    {
        Debug.Log($"{name} ducked");
        StartCoroutine(GetUp());
    }

    IEnumerator GetUp()
    {
        yield return new WaitForSeconds(_gameManager.GetDelayBeforeGettingUp());
        Debug.Log($"{name} got back up");
        _playerController.ToggleCanDuck();
    }

    public bool IsDucking()
    {
        return _isDucking;
    }
}