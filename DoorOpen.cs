using UnityEngine;

public class DoorOpen : MonoBehaviour
{
    public GameObject Door_;

    //opent de deur
    public void OpenDoor()
    {
        if (Door_ != null)
        {
            gameObject.SetActive(false);
        }
        else
        {
            Debug.LogWarning("Door reference is null in DoorOpener script");
        }
    }
}