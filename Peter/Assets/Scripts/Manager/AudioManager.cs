using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : ManagerModule<AudioManager>
{
    public AudioMixerGroup Music;
    public AudioMixerGroup Sfx;

    public float MusicVolume { get; protected set; } = 1;
    public float SfxVolume { get; protected set; } = 1;

    //List of currently active sounds
    private List<AudioSource> sounds = new List<AudioSource>();

    //List of gameObjects that are here for just one sound
    private List<GameObject> objects = new List<GameObject>();

    /// <summary>
    /// Adds a new Sound to the game
    /// </summary>
    public void PlayNewSound(AudioType type, AudioClip audioFile, GameObject gameobject = null)
    {
        GameObject toUse = gameobject;
        if (toUse == null) toUse = Camera.main.gameObject;

        AudioSource source = toUse.AddComponent<AudioSource>();
        source.clip = audioFile;
        switch (type)
        {
            case AudioType.Music:
                source.outputAudioMixerGroup = Music;
                break;
            case AudioType.Sfx:
                source.outputAudioMixerGroup = Sfx;
                break;
            default:
                return;
        }
        source.Play();

        sounds.Add(source);
    }

    /// <summary>
    /// Adds a new sound to the game, that is not connected to an already existing object
    /// </summary>
    public void PlayNewSound(AudioType type, AudioClip audioFile, Vector3 position)
    {
        GameObject soundGameObject = Instantiate(new GameObject(), position, Quaternion.identity);
        objects.Add(soundGameObject);
        PlayNewSound(type, audioFile, soundGameObject);
    }

    private void Update()
    {
        for (int i = sounds.Count - 1; i >= 0; i--)
        {
            if (sounds[i].isPlaying) continue;
            int objectIndex = -1;
            if (objects.Contains(sounds[i].gameObject))
            {
                objectIndex = objects.IndexOf(sounds[i].gameObject);
            }
            Destroy(sounds[i]);
            if (objectIndex != -1)
            {
                Destroy(objects[objectIndex]);
                objects.RemoveAt(objectIndex);
            }
            sounds.RemoveAt(i);
        }
    }
}

public enum AudioType
{
    Music,
    Sfx
}