using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private TMP_Text highScoreText;
    [SerializeField] private Canvas gameOverCanvas;
    
    [SerializeField] private List<GameObject> mice;

    [SerializeField] private float minCycleTime = 2.0f;
    [SerializeField] private float maxCycleTime = 5.0f;

    [SerializeField] private float startingSlapDelay = 1.5f;
    [SerializeField] private float delayBeforeGettingUp = 1.0f;
    [SerializeField] private float mouseDuckDistance = -1.2f;

    [SerializeField] private float startingArmSpeed = 0.3f;

    [SerializeField] private float armSpeedIncrease = 0.2f;
    [SerializeField] private float slapDelayDecrease = -0.1f;

    [SerializeField] private int scoreToChangeMusic = 5;

    private float _currentSlapDelay;
    private float _currentArmSpeed;

    private AudioController _audioController;

    private int _score = 0;

    public static event Action PrepareSlap;
    
    void Start()
    {
        StartCoroutine(CountUntilSlap());
        _currentSlapDelay = startingSlapDelay;
        _currentArmSpeed = startingArmSpeed;
        DisplayHighScore();
        _audioController = FindFirstObjectByType<AudioController>();
        _audioController.ChangeMusic(_audioController.peacefulMusic, 0.5f);
    }

    private void OnEnable()
    {
        Arm.MouseSlapped += GameOver;
    }

    private void OnDisable()
    {
        Arm.MouseSlapped -= GameOver;
    }

    IEnumerator CountUntilSlap()
    {
        float timeUntilSlap = UnityEngine.Random.Range(minCycleTime, maxCycleTime);
        yield return new WaitForSeconds(timeUntilSlap);
        Debug.Log("Prepping slap");
        PrepareSlap?.Invoke();
    }

    public List<GameObject> GetMice()
    {
        return mice;
    }

    public float GetCurrentSlapDelay()
    {
        return _currentSlapDelay;
    }

    public float GetCurrentArmSpeed()
    {
        return _currentArmSpeed;
    }

    public float GetDelayBeforeGettingUp()
    {
        return delayBeforeGettingUp;
    }

    public float GetMouseDuckDistance()
    {
        return mouseDuckDistance;
    }

    public void ResumeGame()
    {
        _currentSlapDelay += slapDelayDecrease;
        _currentArmSpeed += armSpeedIncrease;

        StartCoroutine(CountUntilSlap());
    }

    public void UpdateScore()
    {
        _score++;
        scoreText.text = $"Score\n{_score}";

        if (_score == scoreToChangeMusic)
        {
            _audioController.ChangeMusic(_audioController.intenseMusic, 0.5f);
            Debug.Log("Change music");
        }
    }

    private void DisplayHighScore()
    {
        int currentHighScore = PlayerPrefs.GetInt("HighScore", 0);
        highScoreText.text = $"High Score\n{currentHighScore}";
    }

    public void GameOver()
    {
        int currentHighScore = PlayerPrefs.GetInt("HighScore", 0);
        if (_score > currentHighScore)
        {
            PlayerPrefs.SetInt("HighScore", _score);
            highScoreText.text = $"High Score\n{_score}";
        }

        gameOverCanvas.GameObject().SetActive(true);
    }

    public void ResetGame()
    {
        SceneManager.LoadScene(0);
    }

    public void PlayGame()
    {
        SceneManager.LoadScene(1);
    }
}
