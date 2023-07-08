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
    private Vector2 _startingPosition;
    private float _duckDistance;

    private bool _isDucking = false;

    void Start()
    {
        _gameManager = FindFirstObjectByType<GameManager>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _playerController = FindFirstObjectByType<PlayerController>();
        _startingPosition = transform.position;
        _duckDistance = _gameManager.GetMouseDuckDistance();
    }

    public void Duck()
    {
        Debug.Log($"{name} ducked");
        transform.position += new Vector3(0f, _duckDistance, 0f);
        _isDucking = true;
        StartCoroutine(GetUp());
    }

    IEnumerator GetUp()
    {
        yield return new WaitForSeconds(_gameManager.GetDelayBeforeGettingUp());
        _isDucking = false;
        Debug.Log($"{name} got back up");
        _playerController.ToggleCanDuck();
        transform.position = _startingPosition;
    }

    public bool IsDucking()
    {
        return _isDucking;
    }
}