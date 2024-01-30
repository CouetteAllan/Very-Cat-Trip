using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatRotation : MonoBehaviour
{
    [SerializeField] private LayerMask _groundLayer;
    void Update()
    {
        if (RaycastHit(out RaycastHit2D raycastHit))
        {
            this.transform.up = Vector2.Lerp(this.transform.up,raycastHit.normal, Time.deltaTime * 5.0f);
        }
    }

    private bool RaycastHit(out RaycastHit2D raycastHit)
    {

        raycastHit = Physics2D.Raycast(this.transform.position, -transform.up, 20.0f, _groundLayer);
        return raycastHit;
    }
}
