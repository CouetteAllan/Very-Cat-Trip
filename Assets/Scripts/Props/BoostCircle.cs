using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoostCircle : MonoBehaviour
{
    [SerializeField] private float _speedBoost = 2.0f;
    [SerializeField] private Animator _animator;
    [SerializeField] private ParticleSystem _particles;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.TryGetComponent(out PlayerMovement player))
        {
            player.SpeedBoost(_speedBoost,transform.right);
            _animator.SetTrigger("Boost");
            _particles.Play();
        }
    }
}
