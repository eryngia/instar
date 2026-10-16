using UnityEngine;

// Manages soundtrack playing across scenes.
public class MusicManager : MonoBehaviour {
    public static MusicManager Instance { get; private set; }

    [SerializeField] private AudioClip backgroundMusicClip;
    [SerializeField] private float volume = 0.5f;

    private AudioSource audioSource;

    private void Awake() {
        if (Instance == null) {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        } else {
            Destroy(gameObject);
        }

        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = backgroundMusicClip;
        audioSource.loop = true;
        audioSource.volume = volume;
        audioSource.playOnAwake = false;
    }

    private void Start() {
        audioSource.Play();
    }

    public void SetVolume(float newVolume) {
        volume = newVolume;
        audioSource.volume = volume;
    }

    public void Stop() {
        audioSource.Stop();
    }

    public void Play() {
        if (!audioSource.isPlaying)
            audioSource.Play();
    }
}
