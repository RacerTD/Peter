using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySound : MonoBehaviour
{
    [SerializeField] protected AudioClip audioClip;
    [SerializeField] private Vector3 soundPosition = Vector3.zero;
    [SerializeField] private GameObject soundObject;
    [SerializeField] protected PlaySoundEnumerator whereToPlay = PlaySoundEnumerator.PlayHere;
    [SerializeField] protected AudioType audioType = AudioType.Sfx;

    public void Play()
    {
        switch (whereToPlay)
        {
            case PlaySoundEnumerator.PlayAtPosition:
                AudioManager.Instance.PlayNewSound(audioType, audioClip, soundPosition);
                break;
            case PlaySoundEnumerator.PlayHere:
                AudioManager.Instance.PlayNewSound(audioType, audioClip, gameObject);
                break;
            case PlaySoundEnumerator.PlayAtCamera:
                AudioManager.Instance.PlayNewSound(audioType, audioClip, Camera.main.gameObject);
                break;
            case PlaySoundEnumerator.PlayAtSoundObject:
                AudioManager.Instance.PlayNewSound(audioType, audioClip, soundObject);
                break;
            default:
                break;
        }
    }

    [System.Serializable]
    public enum PlaySoundEnumerator
    {
        PlayAtPosition,
        PlayHere,
        PlayAtCamera,
        PlayAtSoundObject
    }
}
