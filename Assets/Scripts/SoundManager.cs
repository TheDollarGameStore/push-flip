using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    private AudioSource normalSource;
    private AudioSource randomSource;
    private AudioSource pitchedSource;

    public static SoundManager instance;

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        AudioSource[] audioSources= GetComponents<AudioSource>();

        normalSource = audioSources[0];
        randomSource = audioSources[1];
        pitchedSource = audioSources[2];
    }

    public void PlayNormal(AudioClip clip)
    {
        normalSource.PlayOneShot(clip);
    }

    public void PlayRandom(AudioClip clip)
    {
        randomSource.pitch = Random.Range(0.9f, 1.1f);
        randomSource.PlayOneShot(clip);
    }

    public void PlayPitched(AudioClip clip, float pitch)
    {
        pitchedSource.pitch = pitch;
        pitchedSource.PlayOneShot(clip);
    }
}
