using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionsManager : MonoBehaviour, IDataPersistence
{

    float _musicVol = 100.0f;
    float _sfxVol = 100.0f;

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
        
    }

    // Update is called once per frame
    void Update()
    {
        if (GlobalInputManager.Get().GetSubmitInput())
        {
            _musicVol -= 0.1f;
        }
    }
}
