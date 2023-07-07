using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arm : MonoBehaviour
{
    [SerializeField] private float movementDistance = 4.0f;
    [SerializeField] private float movementSpeed = 2.0f;

    private Vector2 _startingPosition;
    
    void Start()
    {
        _startingPosition = transform.position;
    }

    
    void Update()
    {
        MoveArm();
    }

    private void MoveArm()
    {
        const float tau = Mathf.PI * 2;
        float cycle = Time.time * movementSpeed;
        float rawSinWave = Mathf.Sin(cycle * tau);
        float offset = rawSinWave * movementDistance;
        transform.position = new Vector2(_startingPosition.x + offset, _startingPosition.y);
    }
}
