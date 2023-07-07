using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> mice;

    [SerializeField] private float minCycleTime = 2.0f;
    [SerializeField] private float maxCycleTime = 5.0f;
    
    void Start()
    {
        StartCoroutine(CountUntilSlap());
    }
    
    void Update()
    {
        
    }

    IEnumerator CountUntilSlap()
    {
        float timeUntilSlap = Random.Range(minCycleTime, maxCycleTime);
        yield return new WaitForSeconds(timeUntilSlap);
        Debug.Log("Slap");
    }
}
