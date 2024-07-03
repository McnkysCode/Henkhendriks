using UnityEngine;
using UnityEngine.SceneManagement;

public class OnCollision : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            SceneManager.LoadScene("DeathScreen");
            Debug.Log("Player has been Caught");
        }
    }
}