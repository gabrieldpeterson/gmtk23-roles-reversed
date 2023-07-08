using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> mice;

    [SerializeField] private float minCycleTime = 2.0f;
    [SerializeField] private float maxCycleTime = 5.0f;

    [SerializeField] private float startingSlapDelay = 1.5f;
    [SerializeField] private float delayBeforeGettingUp = 1.0f;

    private float _currentSlapDelay;

    public static event Action PrepareSlap;
    
    void Start()
    {
        StartCoroutine(CountUntilSlap());
        _currentSlapDelay = startingSlapDelay;
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

    public float GetDelayBeforeGettingUp()
    {
        return delayBeforeGettingUp;
    }
}