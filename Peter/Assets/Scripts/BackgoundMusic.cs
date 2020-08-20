using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class BackgoundMusic : MonoBehaviour
{
    public List<AudioClip> trackList = new List<AudioClip>();
    private int trackListIndex = 0;
    public int TrackListIndex
    {
        get => trackListIndex;
        set
        {
            if (value >= trackList.Count)
            {
                trackListIndex = 0;
            }
            else
            {
                trackListIndex = value;
            }
        }
    }
    private AudioSource source;
    [SerializeField] protected AudioMixerGroup mix;
    [SerializeField] [Range(0, 1)] protected float volume = 1f;

    private void Start()
    {
        if (source == null)
        {
            source = gameObject.AddComponent<AudioSource>();
            source.outputAudioMixerGroup = mix;
            source.volume = volume;
        }
    }

    private void Update()
    {
        if (!source.isPlaying && trackList.Count > 0)
        {
            source.clip = trackList[TrackListIndex];
            source.Play();
            TrackListIndex++;
        }

        if (GameManager.Instance.CurrentGameState == GameState.Dead || GameManager.Instance.CurrentGameState == GameState.LoadingScene)
        {
            source.volume -= 1 * Time.deltaTime;
        }
    }
}
