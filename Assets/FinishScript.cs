using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishScript : MonoBehaviour
{
    public static event Action<FinishScript> OnFinishSpawn;

    public Collider cameraConfiner;


    // Start is called before the first frame update
    void Start()
    {
        OnFinishSpawn?.Invoke(this);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if ((collision.gameObject.TryGetComponent<PlayerController>(out PlayerController player)))
        {
            GameManager.Instance.ChangeGameState(GameState.Win);
        }
    }

    public Collider GetCameraCOnfiner() => cameraConfiner;
}
