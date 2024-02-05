using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoostCircle : MonoBehaviour
{
    [SerializeField] private float _speedBoost = 2.0f;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.TryGetComponent(out PlayerMovement player))
        {
            player.SpeedBoost(_speedBoost,transform.right);
        }
    }
}
