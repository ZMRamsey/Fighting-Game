using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class OptionsManager : MonoBehaviour, IDataPersistence
{
    [SerializeField] Slider musicVolumeSlider;
    [SerializeField] Slider sfxVolumeSlider;
    public AudioMixer musicMixer;
    public AudioMixer sfxMixer;

    public GameObject _musicSource;
    private AudioSource _musicSrc;

    //Initiates default values
    float _musicVol = 1.0f;
    float _sfxVol = 1.0f;

    bool _fullscreen = true;
    int _resolution = 1920;
    int _quality = 2;

    public void LoadData(OptionsData data)
    {
        this._musicVol = data._musicVol;
        this._sfxVol = data._sfxVol;
        this._fullscreen = data._fullScreen;
        this._resolution = data._resolution;
        this._quality = data._quality;
    }

    public void SaveData(ref OptionsData data)
    {
        data._musicVol = this._musicVol;
        data._sfxVol = this._sfxVol;
        data._fullScreen = this._fullscreen;
        data._resolution = this._resolution;
        data._quality = this._quality;
    }

    // Start is called before the first frame update
    void Start()
    {
        _musicSrc = _musicSource.GetComponent<AudioSource>();
        _musicSrc.volume = this._musicVol;
        musicVolumeSlider.value = this._musicVol;
        sfxVolumeSlider.value = this._sfxVol;
    }
    public void SetMusicVolume()
    {
        musicMixer.SetFloat("musicVol", Mathf.Log10(musicVolumeSlider.value) * 20);
        _musicSrc.volume = musicVolumeSlider.value;
        _musicVol = musicVolumeSlider.value;
    }

    public void SetSFXVolume()
    {
        sfxMixer.SetFloat("sfxVolume", Mathf.Log10(sfxVolumeSlider.value)*20);
        _sfxVol = sfxVolumeSlider.value;
    }
    // Update is called once per frame
    void Update()
    {
        //if (GlobalInputManager.Get().GetDownInput())
        //{
        //    _musicVol -= 0.05f;
        //    if(_musicVol < 0f)
        //    {
        //        _musicVol = 0f;
        //    }
        //    _musicSrc.volume = _musicVol;
        //}
        //else if (GlobalInputManager.Get().GetUpInput())
        //{
        //    _musicVol += 0.05f;
        //    if(_musicVol > 100.0f)
        //    {
        //        _musicVol = 100.0f;
        //    }
        //    _musicSrc.volume = _musicVol;
        //}
    }
}
