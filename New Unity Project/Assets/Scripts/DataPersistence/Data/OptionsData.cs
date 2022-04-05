using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class OptionsData
{

    public float _defaultValue;
    public bool _fullScreen;
    public int _resolution;
    public int _quality;

    public float _musicVol;
    public float _sfxVol;

    public OptionsData()
    {
        this._fullScreen = true;
        this._resolution = 1920;
        this._quality = 2;

        this._musicVol = 100.0f;
        this._sfxVol = 100.0f;
    }
}
