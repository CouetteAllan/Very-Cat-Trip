using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIHealthHandling : MonoBehaviour
{
    [SerializeField] private Image _healthPrefab;
    private List<Image> _images = new List<Image>();
    private int _maxHealth;

    private void Start()
    {
        GameManager.OnGameStateChanged += GameManager_OnGameStateChanged;
        PlayerController.OnChangeHealth += PlayerController_OnChangeHealth;
    }

    private void PlayerController_OnChangeHealth(int currentHealth)
    {
        Initialize(currentHealth);
        
    }

    private void GameManager_OnGameStateChanged(GameState newState)
    {
        if(newState == GameState.StartGame)
        {
            _maxHealth = GameManager.Instance.PlayerController.MaxHealth;
            Initialize(GameManager.Instance.PlayerController.MaxHealth);
        }
    }

    private void Initialize(int maxHealth)
    {
        if(_images.Count > 0)
        {
            foreach(Image image in _images)
            {
                Destroy(image.gameObject);
            }
            _images.Clear();
        }

        for (int i = 0; i < maxHealth; i++)
        {
            var newImage = Instantiate(_healthPrefab, this.transform);
            _images.Add(newImage);
        }
    }


    private void OnDestroy()
    {
        GameManager.OnGameStateChanged -= GameManager_OnGameStateChanged;
        PlayerController.OnChangeHealth -= PlayerController_OnChangeHealth;

    }
}
