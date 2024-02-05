using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using Cinemachine.PostFX;
using UnityEngine.Rendering;

public class FXManager : Singleton<FXManager>
{
    [SerializeField] private CinemachineVolumeSettings _cam;
    [SerializeField] private VolumeProfile _dayProfile;
    [SerializeField] private VolumeProfile _nightProfile;

    public void ChangeProfile(bool day)
    {
        _cam.m_Profile = day ? _dayProfile : _nightProfile;
    }
}
