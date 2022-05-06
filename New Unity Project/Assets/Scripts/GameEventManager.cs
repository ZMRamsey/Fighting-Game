using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameEventManager : MonoBehaviour
{
    [SerializeField] GameEvent _impactFlash;
    [SerializeField] GameEvent _darkness;

    [Header("Supers")]
    [SerializeField] GameEvent _dannySuper;
    [SerializeField] GameEvent _hunterSuper;
    [SerializeField] GameEvent _racketSuper;
    [SerializeField] GameEvent _esmeSuper;
    [SerializeField] GameEvent _gansSuper;
    [SerializeField] GameEvent _raySuper;

    public GameEvent GetImpactFlash() {
        return _impactFlash;
    }

    public GameEvent GetDarkness() {
        return _darkness;
    }

    public GameEvent GetRacketSuper() {
        return _racketSuper;
    }

    public GameEvent GetEsmetSuper() {
        return _esmeSuper;
    }

    public GameEvent GetRaySuper()
    {
        return _raySuper;
    }

    public GameEvent GetHunterSuper()
    {
        return _hunterSuper;
    }

    public GameEvent GetGanzSuper()
    {
        return _gansSuper;
    }

    public GameEvent GetDannySuper()
    {
        return _dannySuper;
    }

    public void DisableAll() {
        _impactFlash.DisableScreen();
        _darkness.DisableScreen();
        _racketSuper.DisableScreen();
    }

    
}

[System.Serializable]
public class GameEvent
{
    [SerializeField] GameObject _screen;

    public void SetOrientation(FighterFilter filter) {
        if(filter == FighterFilter.one) {
            _screen.transform.localScale = new Vector3(1, 1, 1);
            _screen.GetComponent<Image>().color = Color.red;
        }
        else {
            _screen.transform.localScale = new Vector3(-1, 1, 1);
            _screen.GetComponent<Image>().color = Color.blue;
        }
    }

    public void EnableScreen() {
        if (_screen) {
            _screen.SetActive(true);
        }
    }

    public void DisableScreen() {
        if (_screen) {
            _screen.SetActive(false);
        }
    }
}