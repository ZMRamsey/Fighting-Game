using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEventManager : MonoBehaviour
{
    [SerializeField] GameEvent _impactFlash;
    [SerializeField] GameEvent _racketSuper;

    public GameEvent GetImpactFlash() {
        return _impactFlash;
    }

    public GameEvent GetRacketSuper() {
        return _racketSuper;
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