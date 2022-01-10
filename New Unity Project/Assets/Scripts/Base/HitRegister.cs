using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitRegister
{
    float _damage;
    Vector3 _knockBackDirection;

    public HitRegister(float damage, Vector3 knockBackDirection)
    {
        _damage = damage;
        _knockBackDirection = knockBackDirection;
    }

    public float GetDamage()
    {
        return _damage;
    }

    public Vector3 GetKnockbackDirection()
    {
        return _knockBackDirection;
    }
}
