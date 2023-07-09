using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jasnah : MonoBehaviour
{
    private Animator _jasnahAnimator;
    
    void Start()
    {
        _jasnahAnimator = GetComponent<Animator>();
    }

    public void AboutToSlap()
    {
        _jasnahAnimator.SetBool("isWarning", true);
    }

    public void FinishedSlapping()
    {
        _jasnahAnimator.SetBool("isWarning", false);
    }
}
