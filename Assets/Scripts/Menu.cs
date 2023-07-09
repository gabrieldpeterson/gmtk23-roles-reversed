using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    [SerializeField] private TMP_Text localHighScoreText;
    
    void Start()
    {
        int currentHighScore = PlayerPrefs.GetInt("HighScore", 0);
        localHighScoreText.text = $"Local High Score : {currentHighScore}";
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }
}
