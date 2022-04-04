using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DanFighter : FighterController
{
    [SerializeField] Transform _superLeft, _superRight;
    Transform _currentSuper;
    bool _inFunnyMode;
    float _superTimer;

    public override void InitializeFighter() {
        base.InitializeFighter();
        _currentSuper = _superRight;

        if(GetFilter() == FighterFilter.one) {
            _currentSuper = _superLeft;
        }

        _currentSuper.gameObject.SetActive(false);
        _inFunnyMode = false;
    }

    public override void OnFighterUpdate() {
        if (_superTimer > 0 && _inFunnyMode) {
            _superTimer -= Time.deltaTime;

            if(_superTimer <= 0) {
                OnSuperEnd(false);
            }
        }
    }

    public override void OnSuperMechanic() {
        base.OnSuperMechanic();

        _inFunnyMode = true;
        _superTimer = 4;
        _currentSuper.gameObject.SetActive(true);
    }

    public bool IfInSuper() {
        return _inFunnyMode;
    }

    public override void OnSuperEnd(bool instant) {
        base.OnSuperEnd(instant);
        _inFunnyMode = false;
        if (_currentSuper) {
            _currentSuper.gameObject.SetActive(false);
        }
    }
}
