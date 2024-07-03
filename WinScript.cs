using UnityEngine;
using UnityEngine.SceneManagement;

public class WinScript : MonoBehaviour
{
    public void LoadMenu()
    {
        // Load the game scene
        SceneManager.LoadScene(0);
    }

    public void QuitGame()
    {
        //quit the application
        Application.Quit();
    }

    public void Update()
    {
        //cursor locking
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}