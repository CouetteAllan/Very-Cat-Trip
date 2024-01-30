using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;
using UnityEngine.SceneManagement;
using Dreamteck.Forever;

public enum GameState
{
    MainMenu,
    BeforeStartGame,
    StartGame,
    InGame,
    GameOver,
    Pause
}

public class GameManager : Singleton<GameManager>
{

    public static event Action<GameState> OnGameStateChanged;

    [SerializeField] private int _maxGlitter = 50;
    public int MaxGlitter {
        get => _maxGlitter;
        private set => _maxGlitter = value;
    }

    public GameState CurrentState { get; private set; } = GameState.GameOver;
    public PlayerController PlayerController { get; private set; }

    public int Glitter { get; private set; } = 0;

    private void Start()
    {
        PropScript.OnGatherProp += PropScript_OnGatherProp;
        ChangeGameState(GameState.BeforeStartGame);
        StartCoroutine(StartGame());
    }

    private void PropScript_OnGatherProp(PropScript prop)
    {
        GatherGlitter();
    }

    public void ChangeGameState(GameState newState)
    {
        if (newState == CurrentState)
            return;

        CurrentState = newState;
        switch (CurrentState)
        {
            case GameState.MainMenu:
                Time.timeScale = 1.0f;
                Cursor.visible = true;
                break;

            case GameState.BeforeStartGame:
                break;
            case GameState.StartGame:
                Time.timeScale = 1.0f;

                break;
            case GameState.InGame:
                Time.timeScale = 1.0f;
                Time.fixedDeltaTime = Time.timeScale * 0.01f;
                Cursor.visible = false;
                break;
            case GameState.GameOver:
                Time.timeScale = 0.2f;
                Time.fixedDeltaTime = Time.timeScale * 0.01f;
                //faire des trucs de game over
                Cursor.visible = true;
                break;
        }
        OnGameStateChanged?.Invoke(newState);
        Debug.Log("Game State: " + CurrentState.ToString());
    }

    private void GatherGlitter()
    {
        Glitter++;
        UIManager.Instance.UpdateGlitter(Glitter,MaxGlitter);
    }

    private IEnumerator StartGame()
    {
        yield return new WaitUntil(() => LevelGenerator.instance.ready == true);
        ChangeGameState(GameState.StartGame);
    }
}
