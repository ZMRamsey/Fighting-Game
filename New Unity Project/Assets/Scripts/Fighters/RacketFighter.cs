using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RacketFighter : FighterController
{
    [SerializeField] GameObject _subWooferPrefab;
    GameObject _subWooferObject;

    public override void OnSuperMechanic() {
        base.OnSuperMechanic();
        _subWooferObject.GetComponent<SubWoofer>().ResetShuttle(false);
        GameManager.Get().OnSpecial(GameManager.Get().GetEventManager().GetRacketSuper(), _filter);
    }

    public override void OnSuperEnd(bool instant) {
        if (instant && _subWooferObject != null) {
            _subWooferObject.SetActive(false);
        }
    }

    public override void OnSuperEvent() {
        _subWooferObject.GetComponent<SubWoofer>().ResetShuttle(false);
        _subWooferObject.SetActive(true);
        _subWooferObject.transform.position = transform.position;

        var dir = _superMove.GetHitDirection();
        if (_renderer.flipX) {
            dir.x *= -1;
        }

        _subWooferObject.GetComponent<Rigidbody>().velocity = dir;
    }

    public override void InitializeFighter() {
        base.InitializeFighter();

        _subWooferObject = Instantiate(_subWooferPrefab, transform.position, Quaternion.identity);
        _subWooferObject.SetActive(false);
    }

    
}
