using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public void PlayGame() 
    {
        SceneManager.LoadScene(0);
    }

    public void QuitApp() 
    {
        Application.Quit();
    }

    public void Home()
    {
        SceneManager.LoadScene(1);
    }

}
