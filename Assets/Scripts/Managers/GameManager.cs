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
    Pause,
    Win
}

public class GameManager : Singleton<GameManager>
{

    public static event Action<GameState> OnGameStateChanged;

    

    public GameState CurrentState { get; private set; } = GameState.GameOver;
    public PlayerController PlayerController => _player;
    private PlayerController _player;


    private void Start()
    {
        ChangeGameState(GameState.BeforeStartGame);
        StartCoroutine(StartGame());
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
                Time.timeScale = 1.0f;
                Time.fixedDeltaTime = Time.timeScale * 0.01f;
                break;
            case GameState.StartGame:
                Time.timeScale = 1.0f;
                Time.fixedDeltaTime = Time.timeScale * 0.01f;
                SoundManager.Instance.PlayMusic("SadMusic");
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
                SoundManager.Instance.PlayMusic("GameOver");
                break;

            case GameState.Pause:
                Time.timeScale = 0.0f;
                Cursor.visible = true;
                break;

            case GameState.Win:
                Time.timeScale = 0.1f;
                Time.fixedDeltaTime = Time.timeScale * 0.01f;
                break;
        }
        OnGameStateChanged?.Invoke(newState);
        Debug.Log("Game State: " + CurrentState.ToString());
    }


    private IEnumerator StartGame()
    {
        yield return new WaitUntil(() => LevelGenerator.instance.ready == true);
        ChangeGameState(GameState.StartGame);
    }

    public void SetResume()
    {
        ChangeGameState(GameState.InGame);
    }

    public void SetPlayer(PlayerController player) => _player = player;
}
