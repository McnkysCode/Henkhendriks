using UnityEngine;

public class PlayAudioOnClick : MonoBehaviour
{
    public AudioClip audioClip; // Assign the audio clip in the Unity Editor
    private AudioSource audioSource;

    private void Start()
    {
        // Get the AudioSource component attached to this GameObject
        audioSource = GetComponent<AudioSource>();

        // If the AudioSource component is not found, add one
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        // Assign the audio clip to the AudioSource component
        audioSource.clip = audioClip;

        // Make sure the audio clip does not play on awake
        audioSource.playOnAwake = false;
    }

    public void PlayAudio()
    {
        // Check if the audio clip is assigned
        if (audioClip != null)
        {
            // Play the audio clip
            audioSource.Play();
        }
        else
        {
            // Log a warning if the audio clip is not assigned
            Debug.LogWarning("No audio clip assigned!");
        }
    }
}