using UnityEngine;
using UnityEngine.UI;

public class Collectable : MonoBehaviour
{
    public static int collectedFruitTotal = 0;
    public Toggle toggle;
    public DoorOpen OpenDoor;

    private void OnTriggerEnter(Collider other)
    {
        // Check if the entering collider is tagged as "Player"
        if (other.CompareTag("Player"))
        {
            // Increment the total collected fruit count
            collectedFruitTotal++;

            // Set the toggle UI element to "on"
            toggle.isOn = true;

            // Destroy this game object (the collectable)
            Destroy(gameObject);

            // Check if all fruits have been collected
            CheckAllCollectedFruit();
        }
    }

    // Check if all fruits have been collected
    private void CheckAllCollectedFruit()
    {
        // If all fruits have been collected (total is 10), open the door
        if (collectedFruitTotal == 10)
        {
            OpenDoor.OpenDoor(); // Call the OpenDoor function in the DoorOpen script
        }
    }
}