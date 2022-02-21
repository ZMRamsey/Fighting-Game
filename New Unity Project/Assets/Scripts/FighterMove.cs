using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class FighterMove
{
    [SerializeField] string _animationPath;
    [SerializeField] string _clipName;
    [SerializeField] ShotType _shotType;
    [SerializeField] float _frames;
    [SerializeField] float _hitFrame;
    [SerializeField] AnimationClip _clip;
    [SerializeField] Vector3 _hitDirection;

    public string GetPath() {
        return _animationPath;
    }

    public ShotType GetType() {
        return _shotType;
    }

    public string GetClipName() {
        return _clipName;
    }

    public float GetFrames() {
        return _frames;
    }

    public float GetHitFrame() {
        return _hitFrame;
    }

    public AnimationClip GetClip() {
        return _clip;
    }

    public Vector3 GetHitDirection()
    {
        return _hitDirection;
    }
}
