using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CloudGeneratorScript : MonoBehaviour
{
    [SerializeField] private GameObject[] _clouds;
    [SerializeField] private float _cloudTimer;
    private TimerSimple _timerScript;
    private GameObject _selectedCloud;
    private void Start()
    {
        _timerScript = GetComponent<TimerSimple>();

        _timerScript.SetTimer(_cloudTimer, SpawnRandomCloud);
    }


    private void SpawnRandomCloud()
    {
        int randomIndex = UnityEngine.Random.Range(0, _clouds.Length);
        GameObject spawnedCloud = Instantiate(_clouds[randomIndex],this.transform.position,Quaternion.identity);
        _timerScript.SetTimer(4.0f,() => Destroy(_selectedCloud));

    }

}


