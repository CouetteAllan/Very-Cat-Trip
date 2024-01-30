using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rayqdr.CatInputs;

public class PlayerController : MonoBehaviour
{
    private MInputsAction _inputs;
    public MInputsAction Inputs {
        get 
        {
            if( _inputs == null)
            {
                _inputs = new MInputsAction();
                _inputs.Enable();
            }

            return _inputs;
        }
    }

    private Rigidbody _rb;
    private bool _isTransformed;
    private Animator _animator;

    private void Start()
    {
        if(_inputs == null)
        {
            _inputs = new MInputsAction();
            _inputs.Enable();
        }

        _rb = GetComponent<Rigidbody>();
        _animator = GetComponent<Animator>();

        GameManager.OnGameStateChanged += GameManager_OnGameStateChanged;
        GlitterManager.OnGlitterTresholdUpdate += GlitterManager_OnGlitterTresholdUpdate;
    }

    private void GlitterManager_OnGlitterTresholdUpdate(bool tresholdAchieved)
    {
        if(!_isTransformed && tresholdAchieved)
        {
            _isTransformed = true;
            //Change anims;
            _animator.SetTrigger("Transform");
        }
    }

    private void GameManager_OnGameStateChanged(GameState newState)
    {
        switch (newState)
        {
            case GameState.MainMenu:
                break;
            case GameState.BeforeStartGame:
                _rb.isKinematic = true;
                break;
            case GameState.StartGame:
                _rb.isKinematic = false;
                break;
            case GameState.InGame:
                _rb.isKinematic = false;
                break;
            case GameState.GameOver:
                break;
            case GameState.Pause:
                break;
        }
    }

    private void OnDisable()
    {
        GameManager.OnGameStateChanged -= GameManager_OnGameStateChanged;
        GlitterManager.OnGlitterTresholdUpdate -= GlitterManager_OnGlitterTresholdUpdate;
    }
}
