using UnityEngine;

public class MainMenuController : MonoBehaviour
{
    public void Play() {
        UnityEngine.SceneManagement.SceneManager.LoadScene( 2 );
    }
    public void Quit() {
        Application.Quit();
    }
}
