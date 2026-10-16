using UnityEngine;

public static class AudioUtils {

    // Plays an audio clip at the camera position in a temporary object.
    public static void Play(AudioClip clip, float volume = 1.0f) {
        GameObject audioObject = new GameObject("OneShotAudio");
        audioObject.transform.position = Camera.main.transform.position;
        AudioSource source = audioObject.AddComponent<AudioSource>();

        source.clip = clip;
        source.volume = volume;
        source.Play();

        Object.Destroy(audioObject, clip.length);
    }

    // Plays a random audio clip from the array.
    public static void PlayRandom(AudioClip[] clips, float volume = 1.0f) {
        Play(UnseededRandomUtils.FromArray(clips), volume);
    }

}