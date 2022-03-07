using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShuttleCockSound : MonoBehaviour
{
    [SerializeField] SoundTab[] _sounds;

    public AudioClip GetClip(string tag) {
        var clip = _sounds[0].GetRandomClip();

        foreach(SoundTab tab in _sounds) {
            if (tab.IsTag(tag)) {
                clip = tab.GetRandomClip();
            }
        }

        return clip;
    }
}

[System.Serializable]
public class SoundTab
{
    [SerializeField] string _tag;
    [SerializeField] AudioClip[] _clips;

    public bool IsTag(string tag) {
        return _tag.ToLower() == tag.ToLower();
    }

    public AudioClip GetRandomClip() {
        return _clips[Random.Range(0, _clips.Length)];
    }
}
