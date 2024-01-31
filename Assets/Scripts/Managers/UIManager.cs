using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : Singleton<UIManager>
{
    [SerializeField] private Image _fillBarGlitter;
    [SerializeField] private RectTransform _pauseMenu;
    [SerializeField] private RectTransform _gameOverMenu;
    [SerializeField] private RectTransform _winMenu;

    private void Start()
    {
        GameManager.OnGameStateChanged += GameManager_OnGameStateChanged;
    }

    private void GameManager_OnGameStateChanged(GameState state)
    {
        switch (state)
        {
            case GameState.InGame:
                SetPause(false);
                SetGameOver(false);
                SetWin(false);
                break;

            case GameState.GameOver:
                SetPause(false);
                SetGameOver(true);
                SetWin(false);
                break;
            case GameState.Win:
                SetPause(false);
                SetGameOver(false);
                SetWin(true);
                break;
            case GameState.Pause:
                SetPause(true);
                SetGameOver(false);
                SetWin(false);
                break;

        }
    }

    public void UpdateGlitter(int currentGlitter, int maxGlitter)
    {
        _fillBarGlitter.fillAmount = (float)currentGlitter / (float)maxGlitter;
    }

    public void SetPause(bool active)
    {
        _pauseMenu.gameObject.SetActive(active);
    }

    public void SetGameOver(bool active)
    {
        _gameOverMenu.gameObject.SetActive(active);
        if (active)
            _gameOverMenu.GetComponent<Animator>().SetTrigger("Play");
    }

    public void SetWin(bool active)
    {
        _winMenu.gameObject.SetActive(active);
        if (active)
            _winMenu.GetComponent<Animator>().SetTrigger("Play");
    }

    private void OnDisable()
    {
        GameManager.OnGameStateChanged -= GameManager_OnGameStateChanged;

    }

    public void Reset()
    {
        SceneManager.LoadScene(0);
    }

    public void NextLevel()
    {
        SceneManager.LoadScene(2);
    }
}
