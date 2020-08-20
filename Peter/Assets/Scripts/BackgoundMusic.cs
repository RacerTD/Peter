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

    private void Start()
    {
        if (source == null)
        {
            source = gameObject.AddComponent<AudioSource>();
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
    }
}
