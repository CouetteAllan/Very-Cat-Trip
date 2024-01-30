using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class gradiantscript : MonoBehaviour
{
    [SerializeField] private Gradient _colorGradient;
    [SerializeField] private Image _fillImage;

    private void Update()
    {
        _fillImage.color = _colorGradient.Evaluate(_fillImage.fillAmount);
    }
}
