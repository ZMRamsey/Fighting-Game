using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Fighter Audio", menuName = "Badminton/FighterAudio", order = 1)]
public class FighterAudio : ScriptableObject
{
    [SerializeField] AudioClip _jumpUpSFX, _jumpDownSFX;
    [SerializeField] AudioClip[] _hitSounds;
    [SerializeField] AudioClip[] _damageSounds;
    [SerializeField] AudioClip[] _swingSounds;
    [SerializeField] AudioClip[] _leftFootSounds;
    [SerializeField] AudioClip[] _rightFootSounds;
    [SerializeField] AudioClip[] _grabSound;
    [SerializeField] AudioClip[] _squeakSound;
    [SerializeField] float _jumpSFXVolume = 1;
    [SerializeField] float _swingSFXVolume = 1;

    public AudioClip JumpUpSFX { get => _jumpUpSFX; }
    public AudioClip JumpDownSFX { get => _jumpDownSFX; }
    public AudioClip[] HitSounds { get => _hitSounds; }
    public AudioClip[] DamageSounds { get => _damageSounds; }
    public AudioClip[] SwingSounds { get => _swingSounds; }
    public AudioClip[] LeftFootSounds { get => _leftFootSounds; }
    public AudioClip[] RightFootSounds { get => _rightFootSounds; }
    public AudioClip[] GrabSound { get => _grabSound; }
    public AudioClip[] SqueakSound { get => _squeakSound; }
    public float JumpSFXVolume { get => _jumpSFXVolume;}
    public float SwingSFXVolume { get => _swingSFXVolume; }
}
