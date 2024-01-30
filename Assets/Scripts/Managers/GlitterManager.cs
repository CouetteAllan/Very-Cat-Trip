using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class GlitterManager : Singleton<GlitterManager>
{
    public static event Action<bool> OnGlitterTresholdUpdate;

    [SerializeField] private int _maxGlitter = 50;
    public int MaxGlitter
    {
        get => _maxGlitter;
        private set => _maxGlitter = value;
    }
    public int Glitter { get; private set; } = 0;

    private void Start()
    {
        PropScript.OnGatherProp += PropScript_OnGatherProp;
    }

    private void PropScript_OnGatherProp(PropScript prop)
    {
        GatherGlitter();
        bool midGlitter = Glitter >= (float)MaxGlitter / 2.0f;
        if(midGlitter)
        {
            OnGlitterTresholdUpdate?.Invoke(midGlitter);
        }
        
    }


    private void GatherGlitter()
    {
        Glitter++;
        UIManager.Instance.UpdateGlitter(Glitter, MaxGlitter);
    }


    private void OnDisable()
    {
        PropScript.OnGatherProp -= PropScript_OnGatherProp;
    }
}
