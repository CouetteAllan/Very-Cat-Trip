using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PropType
{
    Heart,
    Glitter
}

public class PropScript : MonoBehaviour
{
    public static event Action<PropType> OnGatherProp;

    [SerializeField] private Collider _collider;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private PropType _type = PropType.Glitter;

    private delegate IEnumerator GatherCoroutine();
    private GatherCoroutine _gather;

    private void Start()
    {
        switch (_type)
        {
            case PropType.Heart:
                _gather = HeartFadeCoroutine;
                break;
            case PropType.Glitter:
                _gather = GlitterFadeCoroutine;
                break;

        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<PlayerController>(out var playerController))
        {
            if (_type == PropType.Glitter)
                OnGatherProp?.Invoke(this._type);
            else
                playerController.ChangeHealth(1);
            StartCoroutine(_gather());
        }
    }

    private IEnumerator GlitterFadeCoroutine()
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

    private IEnumerator HeartFadeCoroutine()
    {
        _collider.enabled = false;

        float startTime = Time.time;
        float endTime = 0.3f;

        while (startTime + endTime >= Time.time)
        {
            this.transform.localScale += new Vector3(1, 1) * 1.6f * Time.deltaTime;
            Color newcol = spriteRenderer.color;
            newcol.a -= 2.0f * Time.deltaTime;
            spriteRenderer.color = newcol;

            yield return null;

        }

        Destroy(gameObject);

    }


}
