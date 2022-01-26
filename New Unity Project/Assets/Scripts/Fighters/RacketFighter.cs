using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RacketFighter : FighterController
{
    [SerializeField] GameObject _subWooferPrefab;
    GameObject _subWooferObject;
    public override void OnSuperMechanic() {
        base.OnSuperMechanic();
        _subWooferObject.SetActive(true);
        _subWooferObject.GetComponent<SubWoofer>().ResetShuttle();
        _subWooferObject.transform.position = transform.position;

    }

    public override void InitializeFighter() {
        base.InitializeFighter();

        _subWooferObject = Instantiate(_subWooferPrefab, transform.position, Quaternion.identity);
        _subWooferObject.SetActive(false);
    }

    
}
