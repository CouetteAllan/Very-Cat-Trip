using Dreamteck.Forever;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class FinishManager : Singleton<FinishManager>
{
    [SerializeField] private FinishScript _finishScript;
    [SerializeField] private CinemachineConfiner _cam;
    [SerializeField] private HighLevelPathGenerator _pathGenerator;

    public void Start()
    {
        FinishScript.OnFinishSpawn += FinishScript_OnFinishSpawn;
        GlitterManager.OnMaxGlitterReached += GlitterManager_OnMaxGlitterReached;
    }

    private void GlitterManager_OnMaxGlitterReached()
    {
        StopGenerate();
    }

    private void FinishScript_OnFinishSpawn(FinishScript finish)
    {
        _cam.m_BoundingVolume = finish.GetCameraCOnfiner();
    }

    public void StopGenerate()
    {
        LevelGenerator.instance.levelIteration = LevelGenerator.LevelIteration.OrderedFinite;
        LevelGenerator.instance.pathGenerator = _pathGenerator;
        LevelGenerator.instance.levelRandomizer = null;
        LevelGenerator.instance.LoadLevel(1,true);

    }

    private void OnDisable()
    {
        FinishScript.OnFinishSpawn -= FinishScript_OnFinishSpawn;
        GlitterManager.OnMaxGlitterReached -= GlitterManager_OnMaxGlitterReached;

    }
}
