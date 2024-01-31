using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class GlitterManager : Singleton<GlitterManager>
{
    public static event Action<bool> OnGlitterTresholdUpdate;
    public static event Action OnMaxGlitterReached;

    [SerializeField] private int _maxGlitter = 50;
    [SerializeField] private SpriteRenderer _dayBackground;
    [SerializeField] private SpriteRenderer _nightBackground;

    private bool _maxGlitterReached = false;
    private bool _changedOnce = false;

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
        bool threshold = Glitter >= (float)MaxGlitter / 2.0f;
        if(threshold)
        {
            OnGlitterTresholdUpdate?.Invoke(threshold);
            if (!_changedOnce)
            {
                _changedOnce = true;
                StartCoroutine(ChangeBackground(true));
                StartCoroutine(ChangeMusic(true));
            }
            
        }

        bool negativeThreshold = Glitter <= (float)MaxGlitter * 0.35f && GameManager.Instance.PlayerController.IsTransformed;
        if(negativeThreshold)
        {
            OnGlitterTresholdUpdate?.Invoke(false);
            StartCoroutine(ChangeBackground(false));
        }

        bool reachMaxGlitter = Glitter >= MaxGlitter;
        if(reachMaxGlitter && !_maxGlitterReached)
        {
            _maxGlitterReached = true;
            OnMaxGlitterReached?.Invoke();
        }
        
    }


    private void GatherGlitter()
    {
        Glitter++;
        UIManager.Instance.UpdateGlitter(Glitter, MaxGlitter);
        SoundManager.Instance.Play("Glitter");
    }


    private void OnDisable()
    {
        PropScript.OnGatherProp -= PropScript_OnGatherProp;
    }

    private IEnumerator ChangeBackground(bool isDay)
    {

        float alphaFactor = isDay ? 1.0f : -1.0f;
        float startTime = Time.time;
        float endTime = 2.0f;
        while(startTime + endTime > Time.time)
        {

            Color newCol = _dayBackground.color;
            newCol.a += (0.3f * alphaFactor) * Time.deltaTime;  
            _dayBackground.color = newCol;
            yield return null;
        }

        yield break;

    }

    private IEnumerator ChangeMusic(bool isDay)
    {
        SoundManager.Instance.StopMusic("SadMusic");
        yield return new WaitForSeconds(1.0f);
        SoundManager.Instance.PlayMusic("HappyMusic");
        Debug.Log("changededed");
        
    }
}
