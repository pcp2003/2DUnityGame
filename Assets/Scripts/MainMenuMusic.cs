using UnityEngine;
using UnityEngine.UIElements.Experimental;

public class MainMenuMusic : MonoBehaviour
{
    
    private AudioSource audioSource;
    public static float volume = 1.0f;
    public float startingVolume;
    
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        startingVolume = audioSource.volume;
    }

    void Update() {
        audioSource.volume = startingVolume*volume;
        Debug.Log(audioSource.volume);
    }
}
