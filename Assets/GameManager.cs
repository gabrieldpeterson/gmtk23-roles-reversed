using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> mice;

    [SerializeField] private float minCycleTime = 2.0f;
    [SerializeField] private float maxCycleTime = 5.0f;

    public static event Action prepareSlap;
    
    void Start()
    {
        StartCoroutine(CountUntilSlap());
    }

    IEnumerator CountUntilSlap()
    {
        float timeUntilSlap = UnityEngine.Random.Range(minCycleTime, maxCycleTime);
        yield return new WaitForSeconds(timeUntilSlap);
        Debug.Log("Prepping slap");
        prepareSlap?.Invoke();
    }

    public List<GameObject> GetMice()
    {
        return mice;
    }
}
