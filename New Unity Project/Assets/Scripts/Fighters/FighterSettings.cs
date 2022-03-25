using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Fighter Settings", menuName = "Badminton/FighterSettings", order = 1)]
public class FighterSettings : ScriptableObject
{
    [Header("Moves")]
    [SerializeField] FighterMove _smashMove;
    [SerializeField] FighterMove _chipMove;
    [SerializeField] FighterMove _driveMove;
    [SerializeField] FighterMove _liftMove;
    [SerializeField] FighterMove _specialMove;
    [SerializeField] InfluenceType _hitType;

    [Header("Base Settings")]
    [SerializeField] float _speed;
    [SerializeField] float _height;
    [SerializeField] float _meterIncreaseValue;
    [SerializeField] int _maxJumps;

    [Header("Air Settings")]
    [SerializeField] float _jumpForce;
    [SerializeField] float _fallMultiplier;
    [SerializeField] float _jumpFalloff;
    [SerializeField] float _airSpeed;
    [SerializeField] float _fastFallSpeed = 20;

    [Header("Functions")]
    [SerializeField] LayerMask _groundLayers;
    [SerializeField] bool _hasDash;
    [SerializeField] bool _hasJump;

    public void InitializeSettings() {
        _smashMove.SetUp(ShotType.smash);
        _liftMove.SetUp(ShotType.lift);
        _driveMove.SetUp(ShotType.drive);
        _chipMove.SetUp(ShotType.chip);
        _specialMove.SetUp(ShotType.special);
    }

    public float GetSpeed() {
        return _speed;
    }

    public float GetHeight() {
        return _height;
    }

    public float GetMeterIncreaseValue() {
        return _meterIncreaseValue;
    }

    public float GetMaxJumps() {
        return _maxJumps;
    }

    public float GetJumpForce() {
        return _jumpForce;
    }

    public float GetFallMultiplier() {
        return _fallMultiplier;
    }

    public float GetFallOff() {
        return _jumpFalloff;
    }

    public float GetAirSpeed() {
        return _airSpeed;
    }

    public float GetFastFallSpeed() {
        return _fastFallSpeed;
    }

    public bool HasJump() {
        return _hasJump;
    }

    public bool HasDash() {
        return _hasDash;
    }

    public FighterMove GetSmashMove() {
        return _smashMove;
    }

    public FighterMove GetChipMove() {
        return _chipMove;
    }

    public FighterMove GetDriveMove() {
        return _driveMove;
    }

    public FighterMove GetLiftMove() {
        return _liftMove;
    }

    public FighterMove GetSuperMove() {
        return _specialMove;
    }

    public LayerMask GetGroundMask() {
        return _groundLayers;
    }

    public InfluenceType GetHitType() {
        return _hitType;
    }

}
