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

    private void Start()
    {
        if(_inputs == null)
        {
            _inputs = new MInputsAction();
            _inputs.Enable();
        }

        _rb = GetComponent<Rigidbody>();
        GameManager.OnGameStateChanged += GameManager_OnGameStateChanged;

            
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
}
