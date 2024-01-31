
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuScript : MonoBehaviour
{
    public RectTransform titleScreen;
    public RectTransform tutoScreen;


    public void LoadScene()
    {
        titleScreen.gameObject.SetActive(false);
        tutoScreen.gameObject.SetActive(true);
    }


    public void Play()
    {
        SceneManager.LoadScene(1);
    }
}
