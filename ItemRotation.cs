using UnityEngine;

public class ItemRotation : MonoBehaviour
{
    [SerializeField] private float rotationSpeed = 50f;

    private void Update()
    {
        // Rotate the GameObject around the Y axis at the specified speed
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
        transform.Rotate(Vector3.forward, rotationSpeed * Time.deltaTime);
    }
}