using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * 
 * Example var hitReg = new HitRegister(1, new vector3(0,0,0));
 * 
 */

public class HitRegister
{
    float _damage;
    Vector3 _knockBackDirection;
    float _frameHit;

    public HitRegister(float damage, Vector3 knockBackDirection)
    {
        _damage = damage;
        _knockBackDirection = knockBackDirection;
        _frameHit = Time.frameCount;
    }

    public float GetDamage()
    {
        return _damage;
    }

    public Vector3 GetKnockbackDirection()
    {
        return _knockBackDirection;
    }

    public float GetHitOnFrame() {
        return _frameHit;
    }
}
