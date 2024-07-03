using UnityEngine;

public class SpriteFacingPlayer : MonoBehaviour
{
    private void Update()
    {
        // Get the main camera
        Camera mainCamera = Camera.main;

        if (mainCamera != null)
        {
            // Calculate the direction from the sprite to the camera
            Vector3 directionToCamera = mainCamera.transform.position - transform.position;
            directionToCamera.Normalize(); // Normalize to get a unit vector

            // Make the sprite's up vector parallel to the camera's forward vector
            transform.forward = directionToCamera;
        }
        else
        {
            Debug.LogWarning("Main camera not found!");
        }
    }
}