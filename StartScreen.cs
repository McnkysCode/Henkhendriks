using UnityEngine;
using UnityEngine.SceneManagement;

public class StartScreen : MonoBehaviour
{
    public void StartGame()
    {
        // Load the game scene
        SceneManager.LoadScene(1);
    }

    public void QuitGame()
    {
        //quit the application
        Application.Quit();
    }

    public void Update()
    {
        //cursor locked
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}