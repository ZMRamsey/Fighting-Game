using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class FighterMove
{
    [SerializeField] string _animationPath;
    [SerializeField] ShotType _shotType;

    public string GetPath() {
        return _animationPath;
    }

    public ShotType GetType() {
        return _shotType;
    }
}
