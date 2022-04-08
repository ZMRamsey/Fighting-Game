using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class OptionsManager : MonoBehaviour, IDataPersistence
{
    public UIButton _fullscreenToggle;
    public UIButton _1080p;
    public UIButton _720p;
    public UIButton _576p;


    public Slider[] settingsSliders;
    int sliderIndex = 0;

    [SerializeField] Slider musicVolumeSlider;
    [SerializeField] Slider sfxVolumeSlider;
    [SerializeField] Slider fullscreenSlider;
    [SerializeField] Slider resolutionSlider;

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
        //Screen.fullScreen = _fullscreen;
        Screen.fullScreen = true;
        //InitialiseFullScreen();
        InitialiseResolution();
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

    void InitialiseResolution()
    {
        if(_resolution == 1920)
        {
            Screen.SetResolution(1920, 1080, _fullscreen);
            resolutionSlider.value = 2;
        }
        else if(_resolution == 1280)
        {
            Screen.SetResolution(1280, 720, _fullscreen);
            resolutionSlider.value = 1;
        }
        else
        {
            Screen.SetResolution(1024, 576, _fullscreen);
            resolutionSlider.value = 0;
        }
    }

    

    void InitialiseFullScreen()
    {
        if (_fullscreen)
        {
            Screen.fullScreen = true;
            fullscreenSlider.value = 1;
        }
        else
        {
            Screen.fullScreen = false;
            fullscreenSlider.value = 0;
        }
    }

    public void ToggleFullScreen()
    {
        _fullscreen = !_fullscreen;
        Screen.fullScreen = _fullscreen;
        Debug.Log(_fullscreen);
    }

    //public void ChangeResolution()
    //{
    //    if(resolutionSlider.value == 2)
    //    {
    //        _resolution = 1920;
    //    }
    //    else if(resolutionSlider.value == 1)
    //    {
    //        _resolution = 1280;
    //    }
    //    else
    //    {
    //        _resolution = 1024;
    //    }
    //    InitialiseResolution();
    //    Debug.Log(_resolution);
    //}

    void SetTo1080p()
    {
        _resolution = 1920;
        InitialiseResolution();
        Debug.Log(_resolution);
    }
    void SetTo720p()
    {
        _resolution = 1280;
        InitialiseResolution();
        Debug.Log(_resolution);
    }
    void SetTo576p()
    {
        _resolution = 1024;
        InitialiseResolution();
        Debug.Log(_resolution);
    }

    // Update is called once per frame
    void Update()
    {
        if (_fullscreenToggle.OnClick())
        {
            //ToggleFullScreen();
            print("WJHAT HTHAT HFUCK"); 
        }

        if (_1080p.OnClick())
        {
            SetTo1080p();
        }
        else if (_720p.OnClick())
        {
            SetTo720p();
        }
        else if (_576p.OnClick())
        {
            SetTo576p();
        }
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
