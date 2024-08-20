using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void Play() 
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void Replay() {
        SceneManager.LoadScene(1);
    }


    public void BackToMenu() {
        SceneManager.LoadScene(0);
    }

}
