using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum FighterAction { none, attacking, blocking, jumping, damage };
public enum FighterStance { standing, crouching, air, blow };
public enum FighterState { inControl, restricted };
public abstract class FighterController : MonoBehaviour
{
    [Header("Health")]
    [SerializeField] float _maxHealth;
    float _health;

    [Header("Settings")]
    [SerializeField] LayerMask _groundLayers;
    [SerializeField] float _height;
    [SerializeField] float _speed;
    [SerializeField] float _jumpForce;
    [SerializeField] float _fallMultiplier;
    [SerializeField] bool _canAirDash;
    [SerializeField] bool _canGroundDash;
    [SerializeField] bool _onJump;

    [Header("Controller Values")]
    [SerializeField] Vector3 _controllerVelocity;
    float _yVelocity;
    float _commandMeter;
    RaycastHit _groundHit;

    FighterAction _myAction;
    FighterStance _myStance;
    FighterState _myState;

    [Header("Components")]
    [SerializeField] Animator _animator;
    InputHandler _inputHandler;
    Rigidbody _rigidbody;

    private void Awake() {
        _rigidbody = GetComponent<Rigidbody>();
        _inputHandler = GetComponent<InputHandler>();
    }

    public void InitializeFighter() {
        _health = _maxHealth;
    }

    void Update() {
        ProcessInput();

        if (_myStance == FighterStance.standing) {
            OnGroundMovement();
        }

        if (_myStance == FighterStance.air || _myAction == FighterAction.jumping) {
            OnAirMovement();
        }
    }

    void FixedUpdate() {
        _rigidbody.velocity = _controllerVelocity;
    }

    void ResetAction() {
        _myAction = FighterAction.none;
    }


    public virtual void OnGroundMovement() {
        var xCalculation = 0.0f;

        AdjustControllerHeight();

        xCalculation *= _speed;

        if (_onJump) {
            _myAction = FighterAction.jumping;
            _yVelocity = _jumpForce;
        }

        _controllerVelocity = new Vector3(xCalculation, _yVelocity, 0);
    }

    void AdjustControllerHeight() {
        var rayDirection = transform.TransformDirection(Vector3.down);

        float targetDirVel = Vector3.Dot(rayDirection, _controllerVelocity);
        float groundDirVel = Vector3.Dot(rayDirection, Vector3.zero);

        float relativeVel = targetDirVel - groundDirVel;

        float x = _groundHit.distance - _height;
        float springForce = (x * 20) - (relativeVel * 0.01f);

        Vector3 result = rayDirection * springForce;
        _yVelocity = result.y;
    }

    public virtual void OnAirMovement() {
        if (_myAction != FighterAction.jumping) {

            if (_rigidbody.velocity.y < 0) {
                _yVelocity = Physics.gravity.y * _fallMultiplier;
            }
            else {
                _yVelocity = Physics.gravity.y;
            }
        }
        else {
            _yVelocity *= 0.94f;

            if (_rigidbody.velocity.y < 0) {
                _onJump = false;
                ResetAction();
            }
        }

        _controllerVelocity = new Vector3(_controllerVelocity.x, _yVelocity, 0);
    }

    public virtual void OnDash() {

    }

    public virtual void ProcessInput() {
        if (IsGrounded()) {
            _myStance = FighterStance.standing;
        }
        else {
            _myStance = FighterStance.air;
        }

    }

    public virtual void ProcessHitRegister(HitRegister register) {
        ApplyDamage(register.GetDamage());
        ProcessKnockback(register.GetKnockbackDirection());
    }

    void ApplyDamage(float damage) {
        _health -= Mathf.Abs(damage);

        if (_health < 0) {
            _health = 0;
        }
    }

    public void ProcessKnockback(Vector3 knockbackDirection) {
        //ProcessKnockback
    }

    public bool IsGrounded() {
        var groundCheck = Physics.Raycast(transform.position, Vector3.down, out _groundHit, _height, _groundLayers);
        Debug.DrawLine(transform.position, transform.position + Vector3.down, Color.red, _height * 2);
        return groundCheck;
    }

    public float GetHealth() {
        return _health;
    }

    public FighterStance GetFighterStance() {
        return _myStance;
    }

    public FighterAction GetFighterAction() {
        return _myAction;
    }
}
