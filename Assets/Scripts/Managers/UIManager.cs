using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : Singleton<UIManager>
{
    [SerializeField] private Image _fillBarGlitter;


    private void Start()
    {

    }

    public void UpdateGlitter(int currentGlitter, int maxGlitter)
    {
        _fillBarGlitter.fillAmount = (float)currentGlitter / (float)maxGlitter;
    }
}
