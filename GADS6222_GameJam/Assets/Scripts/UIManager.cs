using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public void PlayGame() 
    {
        SceneManager.LoadScene("GameLevell");
    }

    public void QuitApp() 
    {
        Application.Quit();
    }

    public void Home()
    {
        SceneManager.LoadScene("StartMenu");
    }

}
