using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class FighterMove
{
    [SerializeField] string _animationPath;
    [SerializeField] float _frames;
    [SerializeField] float _hitFrame;
    [SerializeField] AnimationClip _clip;
    [SerializeField] Vector3 _hitDirection;
    [SerializeField] bool _mutesVelocity;
    ShotType _shotType;

    public string GetPath() {
        return _animationPath;
    }

    public bool GetMutes() {
        return _mutesVelocity;
    }

    public void SetUp(ShotType type) {
        _shotType = type;
    }

    public ShotType GetType() {
        return _shotType;
    }

    public string GetClipName() {
        return _clip.name;
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

    public void SetHitDirection(Vector3 dir) {
        _hitDirection = dir;
    }
}
