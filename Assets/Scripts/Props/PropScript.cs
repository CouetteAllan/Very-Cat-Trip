using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PropScript : MonoBehaviour
{
    public static event Action<PropScript> OnGatherProp;

    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent<PlayerController>(out var playerController))
        {
            OnGatherProp?.Invoke(this);
            Destroy(this.gameObject);
        }
    }
}
