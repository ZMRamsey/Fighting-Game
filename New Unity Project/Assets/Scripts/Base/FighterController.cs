using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum FighterState { none, attacking}
public abstract class FighterController : MonoBehaviour
{
    FighterState _myState;

    [Header("Health")]
    [SerializeField] float _maxHealth;
    float _health;

    [Header("Settings")]
    [SerializeField] float _speed;
    [SerializeField] float _jumpForce;
    [SerializeField] float _jumpHeight;
    [SerializeField] bool _canAirDash;
    [SerializeField] bool _canGroundDash;

    [Header("Components")]
    [SerializeField] Animator _animator;
    //InputHandler
    Rigidbody _rigidbody;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    public virtual void ProcessInput()
    {

    }

    public virtual void OnDamageRecieved()
    {

    }

    public float GetHealth()
    {
        return _health;
    }
}
