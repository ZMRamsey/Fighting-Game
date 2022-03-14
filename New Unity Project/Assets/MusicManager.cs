using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    [SerializeField] AudioClip[] _musics;
    [SerializeField] AudioSource _source;

    private void Start() {
        _source.clip = _musics[Random.Range(0, _musics.Length)];
        _source.Play();
    }
}
