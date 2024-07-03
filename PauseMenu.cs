using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool GameIsPaused = false;

    public GameObject pauseScreen;

    private void Update()
    {
        // Check for pause input
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // Toggle between pausing and resuming the game
            if (GameIsPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }

        // Manage cursor visibility based on pause state
        if (GameIsPaused == true)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else if (GameIsPaused == false)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    // Function to resume the game
    public void Resume()
    {
        pauseScreen.SetActive(false); // Hide pause screen
        Time.timeScale = 1f; // Resume normal time scale
        GameIsPaused = false; // Update pause state
    }

    // Function to return to the main menu
    public void Menu()
    {
        SceneManager.LoadScene("StartScreen"); // Load the start screen scene
    }

    // Function to pause the game
    private void Pause()
    {
        pauseScreen.SetActive(true); // Show pause screen
        Time.timeScale = 0f; // Set time scale to zero to pause the game
        GameIsPaused = true; // Update pause state
    }

    // Function to load the main menu scene
    public void LoadMenu()
    {
        Time.timeScale = 1f; // Resume normal time scale
        SceneManager.LoadScene("MainMenu"); // Load the main menu scene
    }

    // Function to quit the game
    public void QuitGame()
    {
        Debug.Log("Quitting game..."); // Log a message indicating quitting the game
        Application.Quit(); // Quit the application
    }
}