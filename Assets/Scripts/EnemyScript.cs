using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour, IDamageSource
{
    public Transform Transform => this.transform;

    //simple enemy standing here
    //Deals damage to the player


    private void OnTriggerEnter(Collider collision)
    {
        if(collision.gameObject.TryGetComponent<IDamageable>(out var player))
        {
            player.TryTakeDamage(1, this);
        }
    }
}
