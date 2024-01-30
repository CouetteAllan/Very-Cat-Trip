using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PropScript : MonoBehaviour
{
    public static event Action<PropScript> OnGatherProp;

    [SerializeField] private Collider _collider;
    [SerializeField] private SpriteRenderer spriteRenderer;

    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent<PlayerController>(out var playerController))
        {
            OnGatherProp?.Invoke(this);
            StartCoroutine(FadeCoroutine());
        }
    }

    private IEnumerator FadeCoroutine()
    {
        _collider.enabled = false;

        float startTime = Time.time;
        float endTime = 0.4f;
        while (startTime + endTime >= Time.time)
        {
            this.transform.position += (Vector3.up * 12.0f * Time.deltaTime);
            Color newcol = spriteRenderer.color;
            newcol.a -= 2.0f * Time.deltaTime;
            spriteRenderer.color = newcol;
            
            yield return null;
        }

        Destroy(gameObject);
    }
}
