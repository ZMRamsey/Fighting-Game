using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        }
        else {
            _screen.transform.localScale = new Vector3(-1, 1, 1);
        }
    }

    public void EnableScreen() {
        _screen.SetActive(true);
    }

    public void DisableScreen() {
        _screen.SetActive(false);
    }
}