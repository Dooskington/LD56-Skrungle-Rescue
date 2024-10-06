using UnityEngine;

public class AudioEvent : MonoBehaviour
{
    private static AudioSource Create(AudioClip clip)
    {
        GameObject gameObject = new GameObject($"Audio ({clip.name})");
        AudioSource source = gameObject.AddComponent<AudioSource>();
        source.clip = clip;
        source.playOnAwake = false;
        source.Pause();

        return source;
    }

    public static void Play(AudioClip clip)
    {
        if (clip == null)
        {
            return;
        }

        AudioSource source = Create(clip);

        // randomize pitch and volume slightly
        source.pitch = Random.Range(0.9f, 1.1f);
        source.volume = Random.Range(0.4f, 0.6f);

        source.Play();
    }

    public static void Play3D(AudioClip clip, Vector3 position)
    {
        if (clip == null)
        {
            return;
        }

        AudioSource source = Create(clip);
        source.transform.position = position;
        source.spatialBlend = 1.0f;
        source.minDistance = 5.0f;
        source.maxDistance = 50.0f;

        // randomize pitch and volume slightly
        source.pitch = Random.Range(0.9f, 1.1f);
        source.volume = Random.Range(0.9f, 1.0f);

        source.Play();

        Destroy(source.gameObject, clip.length);
    }
}
