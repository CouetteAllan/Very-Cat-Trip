using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TimerSimple : MonoBehaviour
{
    private Action _timerCallback;
    private float _timer = 0.0f;

    public void SetTimer(float t, Action timerCallback)
    {
        _timer = t;
        _timerCallback = timerCallback;
    }

    private void Update()
    {
        if(_timer >= 0.0f)
        {
            _timer -= Time.deltaTime;
            if(_timer <= 0.0f)
            {
                _timerCallback();
            }
        }
    }
}
