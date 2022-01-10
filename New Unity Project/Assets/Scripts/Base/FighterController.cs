using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum FighterAction { none, attacking, blocking};
public enum FighterStance { standing, crouching, air };
public enum FighterState { inControl, restricted};
public abstract class FighterController : MonoBehaviour
{
    [Header("Health")]
    [SerializeField] float _maxHealth;
    float _health;

    [Header("Settings")]
    [SerializeField] LayerMask _groundLayers;
    [SerializeField] float _speed;
    [SerializeField] float _jumpForce;
    [SerializeField] float _jumpHeight;
    [SerializeField] bool _canAirDash;
    [SerializeField] bool _canGroundDash;

    [Header("Controller Values")]
    [SerializeField] Vector3 _controllerVelocity;
    float _yVelocity;
    float _commandMeter;

    FighterAction _myAction;
    FighterStance _myStance;
    FighterState _myState;

    [Header("Components")]
    [SerializeField] Animator _animator;
    //InputHandler
    Rigidbody _rigidbody;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        //_inputHandler = GetComponent<InputHandler>();
    }

    public void InitializeFighter()
    {
        _health = _maxHealth;
    }

    void Update()
    {
        ProcessInput();

        if (_myStance == FighterStance.standing)
        {
            OnGroundMovement();
        }

        if (_myStance == FighterStance.air)
        {
            OnAirMovement();
        }
    }

    void FixedUpdate()
    {
        _rigidbody.velocity = _controllerVelocity;
    }


    public virtual void OnGroundMovement()
    {
        //Get input from the handler
        var controllerInput = new Vector3(0, 0, 0);
        controllerInput.Normalize();

        controllerInput *= _speed;

        _controllerVelocity = new Vector3(controllerInput.x, _rigidbody.velocity.y, _rigidbody.velocity.z);
    }

    public virtual void OnAirMovement()
    {
        //Y velocity will increase as the player comes down
        if (_rigidbody.velocity.y < 0)
        {
            _yVelocity = Physics.gravity.y * 2;
        }
    }

    public virtual void OnDash()
    {

    }

    public virtual void ProcessInput()
    {
        
    }

    public virtual void ProcessHitRegister(HitRegister register)
    {
        ApplyDamage(register.GetDamage());
        ProcessKnockback(register.GetKnockbackDirection());
    }

    void ApplyDamage(float damage)
    {
        _health -= Mathf.Abs(damage);

        if(_health < 0)
        {
            _health = 0;
        }
    }

    public void ProcessKnockback(Vector3 knockbackDirection)
    {
        //ProcessKnockback
    }

    public bool IsGrounded()
    {
        //Draw Raycast Down, Elevate character Hip Point When Grounded
        return false;
    }

    public float GetHealth()
    {
        return _health;
    }

    public FighterStance GetFighterStance()
    {
        return _myStance;
    }

    public FighterAction GetFighterAction()
    {
        return _myAction;
    }
}
