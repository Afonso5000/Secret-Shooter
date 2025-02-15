using UnityEngine;

public class BackgroundMusic : MonoBehaviour
{
    private static BackgroundMusic instance;

    void Awake()
    {
        // Ensure only one instance exists (prevents duplicate music)
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Keeps music playing between scenes
        }
        else
        {
            Destroy(gameObject); // Destroy duplicate music players
        }
    }

    void Start()
    {
        // Play the music if an AudioSource is attached
        AudioSource audio = GetComponent<AudioSource>();
        if (audio != null && !audio.isPlaying)
        {
            audio.Play();
        }
    }
}
